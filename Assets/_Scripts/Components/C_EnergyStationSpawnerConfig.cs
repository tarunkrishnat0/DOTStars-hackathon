using Unity.Entities;

public struct C_EnergyStationSpawnerConfig : IComponentData
{
    public Entity Prefab;
    public int NumberOfEnergyStationsToSpawn;
    public float MinSpeed;
    public float MaxSpeed;
    public float EnergyStationHealth;

    public uint CollisionLayerBelongsTo;
    public uint CollisionLayerCollidesWith;
}