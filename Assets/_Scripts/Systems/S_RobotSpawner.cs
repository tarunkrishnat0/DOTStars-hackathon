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
        
        var ecb = SystemAPI.GetSingleton<BeginInitializationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);

        var query = SystemAPI.QueryBuilder().WithAll<T_Robot>().Build();
        var robotsCount = query.CalculateEntityCount();

        void SpawnRobot(float3 position, SpawnCategory spawnCategory)
        {
            var entity = ecb.Instantiate(spawnerConfig.Prefab);
            ecb.SetComponent(entity, new LocalTransform()
            {
                Position = position,
                Rotation = quaternion.LookRotation(position, math.up()),
                Scale = 1,
            });

            ecb.AddComponent(entity, new T_Robot() { spawnCategory = spawnCategory });
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

        SpawnCategory spawnCategory;
        for (int index = robotsCount; index < spawnerConfig.NumberOfRobotsToSpawn; index++)
        {
            var position = random.ValueRW.random.NextFloat3(gameConfig.TerrainMinBoundaries.x, gameConfig.TerrainMaxBoundaries.x);
            position.y = 1f;
             
            if (index % 3 == 0)
            {
                spawnCategory = SpawnCategory.ROBOT_CATEGORY_1;
            }
            else if (index % 3 == 1)
            {
                spawnCategory = SpawnCategory.ROBOT_CATEGORY_2;
            }
            else
            {
                spawnCategory = SpawnCategory.ROBOT_CATEGORY_3;
            }

            SpawnRobot(position, spawnCategory);
        }


        spawnCategory = EnergySystemAndRobotSpawnCtrl.instance.getSelectedSpawnCategory();
        if (spawnCategory != SpawnCategory.ENERGY_SYSTEM)
        {
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
                            SpawnRobot(position, spawnCategory);
                        }
                    }
                }
                inputs.Clear();
            }
        }

        //ToDo: Is this needed or not?
        //state.CompleteDependency();

        // Set Colors
        foreach (var (hatProperties, entity) in SystemAPI.Query<C_RobotFooterProperties>().WithEntityAccess())
        {
            var spawnCategory1 = state.EntityManager.GetComponentData<T_Robot>(hatProperties.Parent).spawnCategory;
            //Debug.Log("spawnCategory1 : " + spawnCategory1.ToString());
            Color color = spawnCategory1 switch
            {
                SpawnCategory.ROBOT_CATEGORY_1 => Color.red,
                SpawnCategory.ROBOT_CATEGORY_2 => Color.green,
                SpawnCategory.ROBOT_CATEGORY_3 => Color.blue
            };
            state.EntityManager.SetComponentData(entity, new URPMaterialPropertyBaseColor() { Value = (Vector4)color });
        }
    }
}
