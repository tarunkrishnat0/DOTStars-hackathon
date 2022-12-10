using Unity.Entities;

public struct C_RobotSpawnerConfig : IComponentData
{
    public Entity Prefab;
    public float MinSpeed;
    public float MaxSpeed;
}