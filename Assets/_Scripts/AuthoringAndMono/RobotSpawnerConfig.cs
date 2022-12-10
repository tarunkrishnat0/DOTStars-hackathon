using Unity.Entities;
using UnityEngine;

public class RobotSpawnerConfig : MonoBehaviour
{
	public static RobotSpawnerConfig instance;

	public GameObject RobotPrefab;
	public int NumberOfCategory1RobotsToSpawn;
	public int NumberOfCategory2RobotsToSpawn;
	public int NumberOfCategory3RobotsToSpawn;
	public float MaxSpeed;
	public float MinSpeed;

    class Baker : Baker<RobotSpawnerConfig>
	{
		public override void Bake(RobotSpawnerConfig authoring)
		{
			AddComponent(new C_RobotSpawnerConfig()
			{
				Prefab = GetEntity(authoring.RobotPrefab),
				NumberOfCategory1RobotsToSpawn = authoring.NumberOfCategory1RobotsToSpawn,
				NumberOfCategory2RobotsToSpawn = authoring.NumberOfCategory2RobotsToSpawn,
				NumberOfCategory3RobotsToSpawn = authoring.NumberOfCategory3RobotsToSpawn,
				MaxSpeed = authoring.MaxSpeed,
				MinSpeed = authoring.MinSpeed
			});
		}
	}
}