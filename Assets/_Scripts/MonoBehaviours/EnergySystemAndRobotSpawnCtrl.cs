using TMPro;
using Unity.Entities;
using UnityEngine;
using UnityEngine.UI;

public enum SpawnCategory
{
    DUMB_ROBOT,
    SEMI_SMART_ROBOT,
    SMART_ROBOT,
    ENERGY_SYSTEM
}

public class EnergySystemAndRobotSpawnCtrl : MonoBehaviour
{
    [Space]
    public int numberOfDumbRobotsToSpawn;
    public int numberOfSemiSmartRobotsToSpawn;
    public int numberOfSmartRobotsToSpawn;
    public int numberOfEnergySystemsToSpawn;

    [Space]
    public Button randomSpawnButtonForCategory1Robots;
    public Button randomSpawnButtonForCategory2Robots;
    public Button randomSpawnButtonForCategory3Robots;
    public Button randomSpawnButtonForEnergySystems;
    public TMP_Dropdown spawnCategoryDropDown;

    private World _world = null;
    private Entity _entity;

    private void Start()
    {
        _world = World.DefaultGameObjectInjectionWorld;
        
        if (_world.IsCreated && !_world.EntityManager.Exists(_entity))
        {
            _entity = _world.EntityManager.CreateEntity();
            _world.EntityManager.AddComponent<C_CurrentSpawnCategoryOfMouseClick>(_entity);
            _world.EntityManager.AddComponent<C_RobotsSpawnCount>(_entity);
            _world.EntityManager.AddComponent<C_EnergyStationsSpawnCount>(_entity);
        }
        SetRobotSpawnCountInConfig();
        SetEnergyStationSpawnCountInConfig();

        populateSpawnCategoryList();
        addButtonListeners();
    }

    private void populateSpawnCategoryList()
    {
        spawnCategoryDropDown.options.Clear();
        spawnCategoryDropDown.options.Add(new TMP_Dropdown.OptionData() { text = "<color=\"red\">" + SpawnCategory.DUMB_ROBOT.ToString() + "</color>" });
        spawnCategoryDropDown.options.Add(new TMP_Dropdown.OptionData() { text = "<color=\"green\">" + SpawnCategory.SEMI_SMART_ROBOT.ToString() + "</color>" });
        spawnCategoryDropDown.options.Add(new TMP_Dropdown.OptionData() { text = "<color=\"blue\">" + SpawnCategory.SMART_ROBOT.ToString() + "</color>" });
        spawnCategoryDropDown.options.Add(new TMP_Dropdown.OptionData() { text = SpawnCategory.ENERGY_SYSTEM.ToString() });

        spawnCategoryDropDown.onValueChanged.RemoveAllListeners();
        spawnCategoryDropDown.onValueChanged.AddListener(Dropdown_OnValueChanged);
        spawnCategoryDropDown.value = 2;
    }

    private void Dropdown_OnValueChanged(int index)
    {
        SpawnCategory spawnCategory = getSelectedSpawnCategory();
        SetCategoryInEntity(spawnCategory);
    }

    private void addButtonListeners()
    {
        randomSpawnButtonForCategory1Robots.onClick.AddListener(onClickOfRandomSpawnButtonForCategory1Robots);
        randomSpawnButtonForCategory2Robots.onClick.AddListener(onClickOfRandomSpawnButtonForCategory2Robots);
        randomSpawnButtonForCategory3Robots.onClick.AddListener(onClickOfRandomSpawnButtonForCategory3Robots);
        randomSpawnButtonForEnergySystems.onClick.AddListener(onClickOfRandomSpawnButtonForEnergySystems);
    }

    public SpawnCategory getSelectedSpawnCategory()
    {
        // SpawnCategory spawnCategory;
        // Enum.TryParse<SpawnCategory>(spawnCategoryDropDown.options[spawnCategoryDropDown.value].text, out spawnCategory);
        // return spawnCategory;
        string category = spawnCategoryDropDown.options[spawnCategoryDropDown.value].text;
        if (category.Contains(SpawnCategory.DUMB_ROBOT.ToString()))
            return SpawnCategory.DUMB_ROBOT;
        else if (category.Contains(SpawnCategory.SEMI_SMART_ROBOT.ToString()))
            return SpawnCategory.SEMI_SMART_ROBOT;
        else if (category.Contains(SpawnCategory.SMART_ROBOT.ToString()))
            return SpawnCategory.SMART_ROBOT;
        else
            return SpawnCategory.ENERGY_SYSTEM;
    }

    public int GetRobotsCount()
    {
        // Check only non destroyed robots
        EntityQuery query = _world.EntityManager.CreateEntityQuery(ComponentType.ReadOnly(typeof(T_Robot)), ComponentType.ReadOnly(typeof(C_RobotMovementProperties)));
        return query.CalculateEntityCount();
    }

    private void onClickOfRandomSpawnButtonForCategory1Robots()
    {
        numberOfDumbRobotsToSpawn = GetRobotsCount(SpawnCategory.DUMB_ROBOT) + 50;
        SetRobotSpawnCountInConfig();
    }

    private void onClickOfRandomSpawnButtonForCategory2Robots()
    {
        numberOfSemiSmartRobotsToSpawn = GetRobotsCount(SpawnCategory.SEMI_SMART_ROBOT) + 50;
        SetRobotSpawnCountInConfig();
    }

    private void onClickOfRandomSpawnButtonForCategory3Robots()
    {
        numberOfSmartRobotsToSpawn = GetRobotsCount(SpawnCategory.SMART_ROBOT) + 50;
        SetRobotSpawnCountInConfig();
    }

    private void onClickOfRandomSpawnButtonForEnergySystems()
    {
        numberOfEnergySystemsToSpawn = GetEnergyStationsCount() + 5;
        SetEnergyStationSpawnCountInConfig();
    }

    private int GetRobotsCount(SpawnCategory spawnCategory)
    {
        EntityQuery query = _world.EntityManager.CreateEntityQuery(ComponentType.ReadOnly(typeof(T_Robot)));
        query.AddSharedComponentFilter(new T_Robot() { spawnCategory = spawnCategory });
        return query.CalculateEntityCount();
    }

    private int GetEnergyStationsCount()
    {
        EntityQuery query = _world.EntityManager.CreateEntityQuery(ComponentType.ReadOnly(typeof(T_EnergyStation)));
        return query.CalculateEntityCount();
    }

    private void SetRobotSpawnCountInConfig()
    {
        _world.EntityManager.SetComponentData(_entity, new C_RobotsSpawnCount() {
            NumberOfDumbRobotsToSpawn = numberOfDumbRobotsToSpawn,
            NumberOfSemiSmartRobotsToSpawn = numberOfSemiSmartRobotsToSpawn,
            NumberOfSmartRobotsToSpawn = numberOfSmartRobotsToSpawn,
        });
    }

    private void SetEnergyStationSpawnCountInConfig()
    {
        _world.EntityManager.SetComponentData(_entity, new C_EnergyStationsSpawnCount()
        {
            NumberOfEnergyStationsToSpawn = numberOfEnergySystemsToSpawn,
        });
    }

    private void SetCategoryInEntity(SpawnCategory spawnCategory)
    {
        _world.EntityManager.SetComponentData(_entity, new C_CurrentSpawnCategoryOfMouseClick() { spawnCategory = spawnCategory });
    }

    private void OnDestroy()
    {
        if (_world.IsCreated && _world.EntityManager.Exists(_entity))
        {
            _world.EntityManager.DestroyEntity(_entity);
        }

        spawnCategoryDropDown.onValueChanged.RemoveAllListeners();
    }
}
