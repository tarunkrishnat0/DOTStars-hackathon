using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

[UpdateInGroup(typeof(InitializationSystemGroup))]
[UpdateAfter(typeof(S_EnergyStationSpawner))]
[BurstCompile]
public partial struct S_RobotSpawner : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<C_RobotSpawnerConfig>();
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var spawnerConfig = SystemAPI.GetSingleton<C_RobotSpawnerConfig>();
        var gameConfig = SystemAPI.GetSingleton<C_GameConfig>();
        var random = SystemAPI.GetSingletonRW<C_GameRandom>();

        // var ecb = new EntityCommandBuffer(Unity.Collections.Allocator.Temp);
        
        var ecb = SystemAPI.GetSingleton<BeginInitializationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);

        var query = SystemAPI.QueryBuilder().WithAll<T_Robot>().Build();
        var robotsCount = query.CalculateEntityCount();

        void SpawnRobot(float3 position)
        {
            var entity = ecb.Instantiate(spawnerConfig.Prefab);

            ecb.SetComponent(entity, new LocalTransform()
            {
                Position = position,
                Rotation = quaternion.LookRotation(position, math.up()),
                Scale = 1,
            });

            ecb.AddComponent<T_Robot>(entity);
            ecb.AddComponent<URPMaterialPropertyBaseColor>(entity);
            ecb.AddComponent<URPMaterialPropertyEmissionColor>(entity);

            position.y = 0f;
            var direction = math.normalize(position);
            ecb.AddComponent(entity, new C_RobotMovementProperties()
            {
                Direction = direction,
                Speed = random.ValueRW.random.NextFloat(spawnerConfig.MinSpeed, spawnerConfig.MaxSpeed),
            });
        }

        for(int i = robotsCount; i < spawnerConfig.NumberOfRobotsToSpawn; i++)
        {
            var position = random.ValueRW.random.NextFloat3(gameConfig.TerrainMinBoundaries.x, gameConfig.TerrainMaxBoundaries.x);
            position.y = 1f;
            SpawnRobot(position);
        }

        state.CompleteDependency();

        //ToDo: Check if we are spawning robots only

        PhysicsWorldSingleton physicsWorld = SystemAPI.GetSingleton<PhysicsWorldSingleton>();
        foreach (var inputs in SystemAPI.Query<DynamicBuffer<C_MouseClicksBuffer>>())
        {
            foreach (var input in inputs)
            {
                if (physicsWorld.CastRay(input.Value, out var hit))
                {
                    var position = math.round(hit.Position) + math.up();
                    NativeList<DistanceHit> distances = new NativeList<DistanceHit>(Allocator.Temp);
                    if (!physicsWorld.OverlapSphere(position + math.up(), 0.1f, ref distances, CollisionFilter.Default))
                    {
                        SpawnRobot(position);
                    }
                }
            }
            inputs.Clear();
        }

        //ecb.Playback(state.EntityManager);
        //ecb.Dispose();

        // state.Enabled = false;
    }
}