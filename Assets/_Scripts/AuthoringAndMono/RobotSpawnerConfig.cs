using Unity.Entities;
using UnityEngine;

public class RobotSpawnerConfig : MonoBehaviour
{
	public static RobotSpawnerConfig instance;

	public GameObject RobotPrefab;
	public float MaxSpeed;
	public float MinSpeed;

    class Baker : Baker<RobotSpawnerConfig>
	{
		public override void Bake(RobotSpawnerConfig authoring)
		{
			AddComponent(new C_RobotSpawnerConfig()
			{
				Prefab = GetEntity(authoring.RobotPrefab),
				MaxSpeed = authoring.MaxSpeed,
				MinSpeed = authoring.MinSpeed
			});
		}
	}
}