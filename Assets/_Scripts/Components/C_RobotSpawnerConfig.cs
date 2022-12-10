using Unity.Entities;

public struct C_RobotSpawnerConfig : IComponentData
{
    public Entity Prefab;
    public int NumberOfCategory1RobotsToSpawn;
    public int NumberOfCategory2RobotsToSpawn;
    public int NumberOfCategory3RobotsToSpawn;
    public float MinSpeed;
    public float MaxSpeed;
}