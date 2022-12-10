using Unity.Entities;
using Unity.Physics;
using Unity.Physics.Authoring;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField] private PhysicsCategoryTags BelongsTo;
    [SerializeField] private PhysicsCategoryTags CollidesWith;

    private Camera _camera;
    private Entity _entity;
    private World _world;

    void Start()
    {
        _camera = Camera.main;
        _world = World.DefaultGameObjectInjectionWorld;
    }

    void Update()
    {
        if(Input.GetMouseButton(0))
        {
            RaycastInput input = GetRayCastInputFromMouseClickPosition();
            AddToMouseClicksBuffer(input);
        }
    }

    private void OnDestroy()
    {
        if (_world.IsCreated && _world.EntityManager.Exists(_entity))
        {
            _world.EntityManager.DestroyEntity(_entity);
        }
    }

    private RaycastInput GetRayCastInputFromMouseClickPosition()
    {
        if(_world.IsCreated && !_world.EntityManager.Exists(_entity) )
        {
            _entity = _world.EntityManager.CreateEntity();
            _world.EntityManager.AddBuffer<C_MouseClicksBuffer>(_entity);
        }

        var mouseClickPosition = Input.mousePosition;
        UnityEngine.Ray ray = _camera.ScreenPointToRay(mouseClickPosition);

        CollisionFilter filter = CollisionFilter.Default;
        filter.BelongsTo = BelongsTo.Value;
        filter.CollidesWith = CollidesWith.Value;

        RaycastInput input = new RaycastInput()
        {
            Start = ray.origin,
            Filter = filter,
            End = ray.GetPoint(_camera.farClipPlane)
        };

        return input;
    }

    private void AddToMouseClicksBuffer(RaycastInput input)
    {
        _world.EntityManager.GetBuffer<C_MouseClicksBuffer>(_entity).Add(new C_MouseClicksBuffer() { Value = input });
    }
}
