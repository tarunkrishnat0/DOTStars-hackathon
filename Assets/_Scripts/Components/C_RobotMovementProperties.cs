using Unity.Entities;
using Unity.Mathematics;

public struct C_RobotMovementProperties : IComponentData
{
    public float3 Direction;
    public float Speed;
}