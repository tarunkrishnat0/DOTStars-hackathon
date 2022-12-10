using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

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

        var ecb = new EntityCommandBuffer(Unity.Collections.Allocator.Temp);
        for(int i = 0; i < spawnerConfig.NumberOfRobotsToSpawn; i++)
        {
            var entity = ecb.Instantiate(spawnerConfig.Prefab);
            var position = random.ValueRW.random.NextFloat3(gameConfig.TerrainMinBoundaries.x, gameConfig.TerrainMaxBoundaries.x);
            position.y = 1f;
            ecb.SetComponent(entity, new LocalTransform()
            {
                Position = position,
                Rotation = quaternion.RotateY(random.ValueRW.random.NextFloat(0, 360)),
                Scale = 1,
            });
        }
        ecb.Playback(state.EntityManager);
        ecb.Dispose();

        state.Enabled = false;
    }
}