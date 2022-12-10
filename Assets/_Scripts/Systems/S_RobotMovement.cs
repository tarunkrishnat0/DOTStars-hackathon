using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

[UpdateInGroup(typeof(SimulationSystemGroup))]
[BurstCompile]
public partial struct S_RobotMovement : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var gameConfig = SystemAPI.GetSingleton<C_GameConfig>();
        var deltaTime = SystemAPI.Time.DeltaTime;
        
        var query = SystemAPI.QueryBuilder().WithAll<T_EnergyStation, LocalTransform>().Build();
        var energyStationTransforms = query.ToComponentDataArray<LocalTransform>(Unity.Collections.Allocator.TempJob);

        var ecbEOS = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged).AsParallelWriter();

        new RobotMovementJob() {
            DeltaTime = deltaTime,
            GameConfig = gameConfig,
            energyStations = energyStationTransforms,
            ECB = ecbEOS
        }.ScheduleParallel();
    }
}

[BurstCompile]
[WithAll(typeof(T_Robot))]
public partial struct RobotMovementJob : IJobEntity
{
    public float DeltaTime;
    public C_GameConfig GameConfig;
    public EntityCommandBuffer.ParallelWriter ECB;
    [ReadOnly] public NativeArray<LocalTransform> energyStations;

    [BurstCompile]
    public void Execute(Entity entity, ref TransformAspect transform, ref C_RobotMovementProperties movementProperties, [EntityIndexInQuery] int sortKey)
    {
        float minDistance = float.MaxValue;
        int nearestEnergyStationIndex = 0;
        for (int i = 0; i < energyStations.Length; i++)
        {
            float distance = math.distance(energyStations[i].Position, transform.LocalPosition);
            if (distance < minDistance)
            {
                nearestEnergyStationIndex = i;
                minDistance = distance;
            }
        }

        if (math.distance(energyStations[nearestEnergyStationIndex].Position, transform.LocalPosition) < 2)
        {
            ECB.DestroyEntity(sortKey, entity);
            return;
        }

        float3 directionTowardsNearestEnergyStation = energyStations[nearestEnergyStationIndex].Position - transform.LocalPosition;
        directionTowardsNearestEnergyStation.y = 0;
        directionTowardsNearestEnergyStation = math.normalize(directionTowardsNearestEnergyStation);
        movementProperties.Direction = directionTowardsNearestEnergyStation;

        var position = transform.LocalPosition + movementProperties.Direction * movementProperties.Speed * DeltaTime;
        if (position.x > GameConfig.TerrainMaxBoundaries.x || position.x < GameConfig.TerrainMinBoundaries.x) {
            movementProperties.Direction.x = -1 * movementProperties.Direction.x;
        }
        
        if (position.z > GameConfig.TerrainMaxBoundaries.y || position.z < GameConfig.TerrainMinBoundaries.y)
        {
            movementProperties.Direction.z = -1 * movementProperties.Direction.z;
        }

        transform.LocalPosition += movementProperties.Direction * movementProperties.Speed * DeltaTime;
        transform.LocalRotation = quaternion.LookRotation(movementProperties.Direction, math.up());

    }
}