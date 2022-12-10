using Unity.Entities;
using Unity.Mathematics;

public struct C_GameConfig : IComponentData
{
    public float2 TerrainMinBoundaries;
    public float2 TerrainMaxBoundaries;
}