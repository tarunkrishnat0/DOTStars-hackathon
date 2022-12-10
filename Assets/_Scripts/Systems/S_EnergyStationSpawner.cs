using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Rendering;
using Unity.Transforms;

[UpdateInGroup(typeof(InitializationSystemGroup))]
[BurstCompile]
public partial struct S_EnergyStationSpawner : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<C_EnergyStationSpawnerConfig>();
    }
    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
    }
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var energyStationConfig = SystemAPI.GetSingleton<C_EnergyStationSpawnerConfig>();
        var gameConfig = SystemAPI.GetSingleton<C_GameConfig>();
        var random = SystemAPI.GetSingletonRW<C_GameRandom>();

        var ecb = SystemAPI.GetSingleton<BeginInitializationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);
        
        var query = SystemAPI.QueryBuilder().WithAll<T_EnergyStation>().Build();
        var energySystemsCount = query.CalculateEntityCount();

        int totalNumberOfEnergySystemsToSpawn = energyStationConfig.NumberOfEnergyStationsToSpawn + 
            EnergySystemAndRobotSpawnCtrl.instance.numberOfEnergySystemsToSpawn;
        for (int i = energySystemsCount; i < totalNumberOfEnergySystemsToSpawn; i++)
        {
            var position = Utilities.GetRandomPositionInGrid(random, gameConfig.TerrainMinBoundaries.x, gameConfig.TerrainMaxBoundaries.x);
            Utilities.SpawnEnergyStation(energyStationConfig.Prefab, position, ecb, random, energyStationConfig.MinSpeed, energyStationConfig.MaxSpeed);
        }

        SpawnCategory spawnCategory = EnergySystemAndRobotSpawnCtrl.instance.getSelectedSpawnCategory();
        if (spawnCategory != SpawnCategory.ENERGY_SYSTEM)
        {
            return;
        }

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
                        Utilities.SpawnEnergyStation(energyStationConfig.Prefab, position, ecb, random, energyStationConfig.MinSpeed, energyStationConfig.MaxSpeed);
                    }
                }
            }
            inputs.Clear();
        }

        // state.Enabled = false;
    }
}
