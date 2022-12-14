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

    //ToDo: Enabling Burst Compile is causing build issues.
    //[BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var spawnerConfig = SystemAPI.GetSingleton<C_RobotSpawnerConfig>();
        var spawnerCount = SystemAPI.GetSingleton<C_RobotsSpawnCount>();
        var gameConfig = SystemAPI.GetSingleton<C_GameConfig>();
        var random = SystemAPI.GetSingletonRW<C_GameRandom>();
        
        var ecb = SystemAPI.GetSingleton<BeginInitializationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);

        EntityQuery query = state.EntityManager.CreateEntityQuery(ComponentType.ReadOnly(typeof(T_Robot)));
        //EntityQuery query = SystemAPI.QueryBuilder().WithAll<T_Robot>().Build();
        query.AddSharedComponentFilter(new T_Robot() { spawnCategory = SpawnCategory.DUMB_ROBOT });

        var robotsCat1Count = query.CalculateEntityCount();
        query.ResetFilter();
        query.AddSharedComponentFilter(new T_Robot() { spawnCategory = SpawnCategory.SEMI_SMART_ROBOT });
        var robotsCat2Count = query.CalculateEntityCount();
        query.ResetFilter();
        query.AddSharedComponentFilter(new T_Robot() { spawnCategory = SpawnCategory.SMART_ROBOT });
        var robotsCat3Count = query.CalculateEntityCount();
        //Debug.Log($"CAT1={robotsCat1Count}, CAT2={robotsCat2Count}, CAT3={robotsCat3Count}");

        int totalNumberOfDumbRobotsToSpawn = spawnerCount.NumberOfDumbRobotsToSpawn;
        // Debug.Log("totalNumberOfDumbRobotsToSpawn : " + totalNumberOfDumbRobotsToSpawn + " robotsCat1Count : " + robotsCat1Count);
        for (int index = robotsCat1Count; index < totalNumberOfDumbRobotsToSpawn; index++)
        {
            var position = random.ValueRW.random.NextFloat3(gameConfig.TerrainMinBoundaries.x, gameConfig.TerrainMaxBoundaries.x);
            position.y = 1f;
            Utilities.SpawnRobot(position, SpawnCategory.DUMB_ROBOT, spawnerConfig, ecb, random);
        }

        int totalNumberOfCategory2RobotsToSpawn = spawnerCount.NumberOfSemiSmartRobotsToSpawn;
        for (int index = robotsCat2Count; index < totalNumberOfCategory2RobotsToSpawn; index++)
        {
            var position = random.ValueRW.random.NextFloat3(gameConfig.TerrainMinBoundaries.x, gameConfig.TerrainMaxBoundaries.x);
            position.y = 1f;
            Utilities.SpawnRobot(position, SpawnCategory.SEMI_SMART_ROBOT, spawnerConfig, ecb, random);
        }

        int totalNumberOfCategory3RobotsToSpawn = spawnerCount.NumberOfSmartRobotsToSpawn;
        for (int index = robotsCat3Count; index < totalNumberOfCategory3RobotsToSpawn; index++)
        {
            var position = random.ValueRW.random.NextFloat3(gameConfig.TerrainMinBoundaries.x, gameConfig.TerrainMaxBoundaries.x);
            position.y = 1f;
            Utilities.SpawnRobot(position, SpawnCategory.SMART_ROBOT, spawnerConfig, ecb, random);
        }

        SpawnCategory spawnCategory = SystemAPI.GetSingleton<C_CurrentSpawnCategoryOfMouseClick>().spawnCategory;
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
                            Utilities.SpawnRobot(position, spawnCategory, spawnerConfig, ecb, random);
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
            var spawnCategory1 = state.EntityManager.GetSharedComponentManaged<T_Robot>(hatProperties.Parent).spawnCategory;
            //Debug.Log("spawnCategory1 : " + spawnCategory1.ToString());
            Color color = spawnCategory1 switch
            {
                SpawnCategory.DUMB_ROBOT => Color.red,
                SpawnCategory.SEMI_SMART_ROBOT => Color.green,
                SpawnCategory.SMART_ROBOT => Color.blue
            };
            state.EntityManager.SetComponentData(entity, new URPMaterialPropertyBaseColor() { Value = (Vector4)color });
        }
    }
}
