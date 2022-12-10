using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;

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

        for(int index = 0; index < spawnerConfig.NumberOfRobotsToSpawn; index++)
        {
            var entity = ecb.Instantiate(spawnerConfig.Prefab);
            var position = random.ValueRW.random.NextFloat3(gameConfig.TerrainMinBoundaries.x, gameConfig.TerrainMaxBoundaries.x);
            position.y = 1f;
            ecb.SetComponent(entity, new LocalTransform()
            {
                Position = position,
                Rotation = quaternion.LookRotation(position, math.up()),
                Scale = 1,
            });

            SpawnCategory spawnCategory;
            if (index % 3 == 0)
            {
                spawnCategory = SpawnCategory.ROBOT_CATEGORY_1;
            }
            else if(index % 3 == 1)
            {
                spawnCategory = SpawnCategory.ROBOT_CATEGORY_2;
            }
            else
            {
                spawnCategory = SpawnCategory.ROBOT_CATEGORY_3;
            }

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

        //ecb.Playback(state.EntityManager);
        //ecb.Dispose();

        state.Enabled = false;
    }
}