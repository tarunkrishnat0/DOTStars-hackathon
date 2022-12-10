using Unity.Entities;
using Unity.Rendering;
using UnityEngine;

public class RobotFooterProperties : MonoBehaviour
{
	class Baker : Baker<RobotFooterProperties>
	{
		public override void Bake(RobotFooterProperties authoring)
		{
			AddComponent(new C_RobotFooterProperties() { 
				Parent = GetEntity(GetParent(authoring))
			});

			AddComponent(new URPMaterialPropertyBaseColor() { Value = Vector4.one });
		}
	}
}