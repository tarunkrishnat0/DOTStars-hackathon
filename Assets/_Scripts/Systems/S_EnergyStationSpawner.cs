using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
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

        // var ecb = new EntityCommandBuffer(Unity.Collections.Allocator.Temp);

        var ecb = SystemAPI.GetSingleton<BeginInitializationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);

        for (int i = 0; i < energyStationConfig.NumberOfEnergyStationsToSpawn; i++)
        {
            var entity = ecb.Instantiate(energyStationConfig.Prefab);
            var position = random.ValueRW.random.NextFloat3(gameConfig.TerrainMinBoundaries.x, gameConfig.TerrainMaxBoundaries.x);
            position.y = 0f;
            ecb.SetComponent(entity, new LocalTransform()
            {
                Position = position,
                Rotation = quaternion.LookRotation(position, math.up()),
                Scale = 5,
            });

            ecb.AddComponent<T_EnergyStation>(entity);
            var color = (UnityEngine.Vector4)Utilities.RandomColor(random.ValueRW.random.NextFloat(0f, 1f));
            ecb.AddComponent(entity, new URPMaterialPropertyBaseColor() { Value = color });
            ecb.AddComponent(entity, new URPMaterialPropertyEmissionColor() { Value = color });

            position.y = 0f;
            var direction = math.normalize(position);
            ecb.AddComponent(entity, new C_EnergyStationMovementProperties()
            {
                Direction = direction,
                Speed = random.ValueRW.random.NextFloat(energyStationConfig.MinSpeed, energyStationConfig.MaxSpeed),
            });

            ecb.AddComponent(entity, new C_EnergyStationHealthProperty() { EnergyStationHealth = energyStationConfig.EnergyStationHealth });

        }

        //ecb.Playback(state.EntityManager);
        //ecb.Dispose();

        state.Enabled = false;
    }
}