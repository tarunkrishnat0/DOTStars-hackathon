using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Authoring;
using Unity.Transforms;
using UnityEngine;

[UpdateInGroup(typeof(SimulationSystemGroup))]
[UpdateAfter(typeof(S_EnergyStationMovement))]
[BurstCompile]
public partial struct S_EnergyStationHealthCtrl : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<C_EnergyStationSpawnerConfig>();
    }
    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
    }
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        state.CompleteDependency();

        var spawnConfig = SystemAPI.GetSingleton<C_EnergyStationSpawnerConfig>();

        PhysicsWorldSingleton physicsWorld = SystemAPI.GetSingleton<PhysicsWorldSingleton>();
        var ecbEOS = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged).AsParallelWriter();

        new EnergyStationHealthJob()
        {
            ECB = ecbEOS,
            PhysicsWorld = physicsWorld,
            CollisionLayerBelongsTo = spawnConfig.CollisionLayerBelongsTo,
            CollisionLayerCollidesWith = spawnConfig.CollisionLayerCollidesWith,
        }.Schedule();
    }
}

[BurstCompile]
[WithAll(typeof(T_EnergyStation))]
public partial struct EnergyStationHealthJob : IJobEntity
{
    public EntityCommandBuffer.ParallelWriter ECB;
    public PhysicsWorldSingleton PhysicsWorld;
    [ReadOnly] public uint CollisionLayerBelongsTo;
    [ReadOnly] public uint CollisionLayerCollidesWith;

    [BurstCompile]
    public void Execute(Entity entity, ref TransformAspect transform, ref C_EnergyStationHealthProperty energyStationHealthProperty, [EntityIndexInQuery] int sortKey)
    {
        if (energyStationHealthProperty.EnergyStationHealth <= 0)
        {
            ECB.DestroyEntity(sortKey, entity);
            return;
        }
        NativeList<DistanceHit> distances = new NativeList<DistanceHit>(Allocator.Temp);
        CollisionFilter filter = CollisionFilter.Default;
        filter.BelongsTo = CollisionLayerBelongsTo;
        filter.CollidesWith = CollisionLayerCollidesWith;

        if (PhysicsWorld.OverlapSphere(transform.LocalPosition, 2.1f, ref distances, filter))
        {
            // Debug.Log("EnergyStation collision : " + distances.Length);
            energyStationHealthProperty.EnergyStationHealth -= (distances.Length-1)* 0.5f;
        }
    }
}