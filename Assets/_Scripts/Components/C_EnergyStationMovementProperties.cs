using Unity.Entities;
using Unity.Mathematics;

public struct C_EnergyStationMovementProperties : IComponentData
{
    public float3 Direction;
    public float Speed;
}