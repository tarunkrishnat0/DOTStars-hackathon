using System;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;

public class Utilities
{
    public static Color RandomColor(float hue)
    {
        //Note: if you are not familiar with this concept, this is a "local function".
        //You can search for that term on the internet for more information.
        // 0.618034005f == 2 / (math.sqrt(5) + 1) == inverse of the golden ratio

        hue = (hue + 0.618034005f) % 1;
        var color = Color.HSVToRGB(hue, 1.0f, 1.0f);
        return color;
    }

    public static float3 GetRandomPositionInGrid(RefRW<C_GameRandom> random, float min, float max)
    {
        var position = random.ValueRW.random.NextFloat3(min, max);
        position.y = 0f;
        return position;
    }

    public static void SpawnEnergyStation(Entity prefab, float3 position, EntityCommandBuffer ecb, RefRW<C_GameRandom> random, float minSpeed, float maxSpeed, float health)
    {
        var entity = ecb.Instantiate(prefab);

        ecb.SetComponent(entity, new LocalTransform()
        {
            Position = position,
            Rotation = quaternion.LookRotation(position, math.up()),
            Scale = 5,
        });

        ecb.AddComponent<T_EnergyStation>(entity);
        var color = (UnityEngine.Vector4)Utilities.RandomColor(random.ValueRW.random.NextFloat(0f, 1f));
        ecb.AddComponent(entity, new URPMaterialPropertyBaseColor() { Value = color });
        ecb.AddComponent(entity, new URPMaterialPropertyEmissionColor() { Value = color });

        position.y = 0f;
        var direction = math.normalize(position);
        ecb.AddComponent(entity, new C_EnergyStationMovementProperties()
        {
            Direction = direction,
            Speed = random.ValueRW.random.NextFloat(minSpeed, maxSpeed),
        });

        ecb.AddComponent(entity, new C_EnergyStationHealthProperty() { EnergyStationHealth = health });

    }
}
