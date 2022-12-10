using Unity.Entities;

public struct C_RobotsSpawnCount : IComponentData
{
    public int NumberOfDumbRobotsToSpawn;
    public int NumberOfSemiSmartRobotsToSpawn;
    public int NumberOfSmartRobotsToSpawn;
}