using Unity.Entities;
using UnityEngine;

public class RobotSpawnerConfig : MonoBehaviour
{
	public GameObject RobotPrefab;
	public int NumberOfRobotsToSpawn;
	public float MaxSpeed;
	public float MinSpeed;

	class Baker : Baker<RobotSpawnerConfig>
	{
		public override void Bake(RobotSpawnerConfig authoring)
		{
			AddComponent(new C_RobotSpawnerConfig()
			{
				Prefab = GetEntity(authoring.RobotPrefab),
				NumberOfRobotsToSpawn = authoring.NumberOfRobotsToSpawn,
				MaxSpeed = authoring.MaxSpeed,
				MinSpeed = authoring.MinSpeed
			});
		}
	}
}