using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

[UpdateAfter(typeof(SimulationSystemGroup))]
[UpdateAfter(typeof(S_EnergyStationSpawner))]
[BurstCompile]
public partial struct S_EnergyStationMovement : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<C_GameConfig>();
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

        new EnergyStationMovementJob()
        {
            DeltaTime = deltaTime,
            GameConfig = gameConfig,
        }.ScheduleParallel();
    }
}

[BurstCompile]
[WithAll(typeof(T_EnergyStation))]
public partial struct EnergyStationMovementJob : IJobEntity
{
    public float DeltaTime;
    public C_GameConfig GameConfig;

    [BurstCompile]
    public void Execute(ref TransformAspect transform, ref C_EnergyStationMovementProperties movementProperties)
    {
        var position = transform.LocalPosition + movementProperties.Direction * movementProperties.Speed * DeltaTime;
        if (position.x > GameConfig.TerrainMaxBoundaries.x || position.x < GameConfig.TerrainMinBoundaries.x)
        {
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