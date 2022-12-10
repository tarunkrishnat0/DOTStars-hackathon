using Unity.Entities;

public struct C_RobotSpawnerConfig : IComponentData
{
    public Entity Prefab;
    public int NumberOfRobotsToSpawn;
    public float MinSpeed;
    public float MaxSpeed;
}