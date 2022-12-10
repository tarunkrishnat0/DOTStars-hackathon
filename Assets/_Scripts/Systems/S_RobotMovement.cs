using Unity.Burst;
using Unity.Entities;
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
        var deltaTime = SystemAPI.Time.DeltaTime;

        new RobotMovementJob() { DeltaTime = deltaTime }.ScheduleParallel();
    }
}

[BurstCompile]
[WithAll(typeof(T_Robot))]
public partial struct RobotMovementJob : IJobEntity
{
    public float DeltaTime;

    [BurstCompile]
    public void Execute(ref TransformAspect transform, in C_RobotMovementProperties movementProperties)
    {
        transform.LocalPosition += movementProperties.Direction * movementProperties.Speed * DeltaTime;
    }
}