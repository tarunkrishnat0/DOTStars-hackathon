using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class GameConfig : MonoBehaviour
{
	public float2 TerrainMinBoundaries;
    public float2 TerrainMaxBoundaries;

    class Baker : Baker<GameConfig>
	{
		public override void Bake(GameConfig authoring)
		{
			AddComponent(new C_GameConfig()
			{
				TerrainMaxBoundaries = authoring.TerrainMaxBoundaries,
				TerrainMinBoundaries = authoring.TerrainMinBoundaries,
			});

			AddComponent(new C_GameRandom()
			{
				random = Unity.Mathematics.Random.CreateFromIndex(12345),
			});
		}
	}
}