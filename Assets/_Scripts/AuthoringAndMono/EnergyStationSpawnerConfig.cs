using Unity.Entities;
using UnityEngine;

public class EnergyStationSpawnerConfig : MonoBehaviour
{
	public GameObject EnergyStation;
	public int NumberOfEnergyStationsToSpawn;
	public float MaxSpeed;
	public float MinSpeed;
	public float EnergyStationHealth;

	class Baker : Baker<EnergyStationSpawnerConfig>
	{
		public override void Bake(EnergyStationSpawnerConfig authoring)
		{
			AddComponent(new C_EnergyStationSpawnerConfig()
			{
				Prefab = GetEntity(authoring.EnergyStation),
				NumberOfEnergyStationsToSpawn = authoring.NumberOfEnergyStationsToSpawn,
				MaxSpeed = authoring.MaxSpeed,
				MinSpeed = authoring.MinSpeed,
				EnergyStationHealth = authoring.EnergyStationHealth
			});
		}
	}
}