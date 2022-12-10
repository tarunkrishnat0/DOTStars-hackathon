using Unity.Entities;
using Unity.Physics.Authoring;
using UnityEngine;

public class EnergyStationSpawnerConfig : MonoBehaviour
{
	public GameObject EnergyStation;
	public float MaxSpeed;
	public float MinSpeed;
	public float EnergyStationHealth;

	public PhysicsCategoryTags CollisionLayerBelongsTo;
	public PhysicsCategoryTags CollisionLayerCollidesWith;

	class Baker : Baker<EnergyStationSpawnerConfig>
	{
		public override void Bake(EnergyStationSpawnerConfig authoring)
		{
			AddComponent(new C_EnergyStationSpawnerConfig()
			{
				Prefab = GetEntity(authoring.EnergyStation),
				MaxSpeed = authoring.MaxSpeed,
				MinSpeed = authoring.MinSpeed,
				EnergyStationHealth = authoring.EnergyStationHealth,

				CollisionLayerBelongsTo = authoring.CollisionLayerBelongsTo.Value,
				CollisionLayerCollidesWith = authoring.CollisionLayerCollidesWith.Value,
			});
		}
	}
}