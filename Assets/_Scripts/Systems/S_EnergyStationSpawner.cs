using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
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

        // var ecb = new EntityCommandBuffer(Unity.Collections.Allocator.Temp);

        var ecb = SystemAPI.GetSingleton<BeginInitializationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);

        for (int i = 0; i < energyStationConfig.NumberOfEnergyStationsToSpawn; i++)
        {
            var entity = ecb.Instantiate(energyStationConfig.Prefab);
            var position = random.ValueRW.random.NextFloat3(gameConfig.TerrainMinBoundaries.x, gameConfig.TerrainMaxBoundaries.x);
            position.y = 1f;
            ecb.SetComponent(entity, new LocalTransform()
            {
                Position = position,
                Rotation = quaternion.LookRotation(position, math.up()),
                Scale = 5,
            });

            ecb.AddComponent<T_EnergyStation>(entity);
        }

        //ecb.Playback(state.EntityManager);
        //ecb.Dispose();

        state.Enabled = false;
    }
}