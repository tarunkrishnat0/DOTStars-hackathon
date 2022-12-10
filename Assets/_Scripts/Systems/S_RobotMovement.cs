using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;

[UpdateInGroup(typeof(SimulationSystemGroup))]
[BurstCompile]
public partial struct S_RobotMovement : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var gameConfig = SystemAPI.GetSingleton<C_GameConfig>();
        var deltaTime = SystemAPI.Time.DeltaTime;
        
        var query = SystemAPI.QueryBuilder().WithAll<T_EnergyStation, LocalTransform, URPMaterialPropertyBaseColor>().Build();
        var energyStationTransforms = query.ToComponentDataArray<LocalTransform>(Allocator.TempJob);
        var energyStationColors = query.ToComponentDataArray<URPMaterialPropertyBaseColor>(Allocator.TempJob);

        var ecbEOS = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged).AsParallelWriter();

        new RobotMovementJob() {
            DeltaTime = deltaTime,
            GameConfig = gameConfig,
            EnergyStationTransforms = energyStationTransforms,
            EneryStationColors = energyStationColors,
            ECB = ecbEOS
        }.ScheduleParallel();
    }
}

[BurstCompile]
[WithAll(typeof(T_Robot))]
public partial struct RobotMovementJob : IJobEntity
{
    public float DeltaTime;
    public C_GameConfig GameConfig;
    public EntityCommandBuffer.ParallelWriter ECB;
    [ReadOnly] public NativeArray<LocalTransform> EnergyStationTransforms;
    [ReadOnly] public NativeArray<URPMaterialPropertyBaseColor> EneryStationColors;

    [BurstCompile]
    public void Execute(Entity entity, ref TransformAspect transform, ref C_RobotMovementProperties movementProperties,
        [EntityIndexInQuery] int sortKey, ref URPMaterialPropertyBaseColor baseColor, ref URPMaterialPropertyEmissionColor emissionColor,
        in T_Robot robotTag)
    {
        float maxDistance = 100f * math.sqrt(2);
        float minDistance = float.MaxValue;
        int nearestEnergyStationIndex = 0;

        if (EnergyStationTransforms.Length > 0)
        {
            for (int i = 0; i < EnergyStationTransforms.Length; i++)
            {
                float distance = math.distance(EnergyStationTransforms[i].Position, transform.LocalPosition);
                if (distance < minDistance)
                {
                    nearestEnergyStationIndex = i;
                    minDistance = distance;
                }
            }

            if (math.distance(EnergyStationTransforms[nearestEnergyStationIndex].Position, transform.LocalPosition) < 2)
            {
                ECB.DestroyEntity(sortKey, entity);
                return;
            }

            float3 directionTowardsNearestEnergyStation = EnergyStationTransforms[nearestEnergyStationIndex].Position - transform.LocalPosition;
            directionTowardsNearestEnergyStation.y = 0;
            directionTowardsNearestEnergyStation = math.normalize(directionTowardsNearestEnergyStation);

            float currentDistance = math.distance(EnergyStationTransforms[nearestEnergyStationIndex].Position, transform.LocalPosition);
            float closenessfactor = (maxDistance - currentDistance) / maxDistance;

            var energyStationTempColor = EneryStationColors[nearestEnergyStationIndex].Value;
            float3 energeStationColor = new float3(energyStationTempColor.x, energyStationTempColor.y, energyStationTempColor.z);
            float3 finalColor = math.lerp(new float3(1, 1, 1), energeStationColor, closenessfactor);

            baseColor.Value = new Vector4(finalColor.x, finalColor.y, finalColor.z, 1);
            emissionColor.Value = baseColor.Value;

            if (robotTag.spawnCategory == SpawnCategory.ROBOT_CATEGORY_3 ||
                (robotTag.spawnCategory == SpawnCategory.ROBOT_CATEGORY_2 && System.DateTime.Now.Second % 3 == 0) ||
                (robotTag.spawnCategory == SpawnCategory.ROBOT_CATEGORY_1 && System.DateTime.Now.Second % 7 == 0))
            {
                movementProperties.Direction = directionTowardsNearestEnergyStation;
            }
        }

        var position = transform.LocalPosition + movementProperties.Direction * movementProperties.Speed * DeltaTime;
        if (position.x > GameConfig.TerrainMaxBoundaries.x || position.x < GameConfig.TerrainMinBoundaries.x)
        {
            movementProperties.Direction.x = -1 * movementProperties.Direction.x;
        }

        if (position.z > GameConfig.TerrainMaxBoundaries.y || position.z < GameConfig.TerrainMinBoundaries.y)
        {
            movementProperties.Direction.z = -1 * movementProperties.Direction.z;
        }

        transform.LocalPosition += movementProperties.Direction * movementProperties.Speed * DeltaTime;
        transform.LocalRotation = quaternion.LookRotation(movementProperties.Direction, math.up());
    }
}