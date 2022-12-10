using System;
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
    public static EnergySystemAndRobotSpawnCtrl instance;

    [Space]
    public int numberOfCategory1RobotsToSpawn;
    public int numberOfCategory2RobotsToSpawn;
    public int numberOfCategory3RobotsToSpawn;
    public int numberOfEnergySystemsToSpawn;

    [Space]
    public Button randomSpawnButtonForCategory1Robots;
    public Button randomSpawnButtonForCategory2Robots;
    public Button randomSpawnButtonForCategory3Robots;
    public Button randomSpawnButtonForEnergySystems;
    public TMP_Dropdown spawnCategoryDropDown;

    private World _world = null;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
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

        spawnCategoryDropDown.value = 2;
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

    private void onClickOfRandomSpawnButtonForCategory1Robots()
    {
        numberOfCategory1RobotsToSpawn = GetRobotsCount(SpawnCategory.DUMB_ROBOT) + 50;
    }

    private void onClickOfRandomSpawnButtonForCategory2Robots()
    {
        numberOfCategory2RobotsToSpawn = GetRobotsCount(SpawnCategory.SEMI_SMART_ROBOT) + 50;
    }

    private void onClickOfRandomSpawnButtonForCategory3Robots()
    {
        numberOfCategory3RobotsToSpawn = GetRobotsCount(SpawnCategory.SMART_ROBOT) + 50;
    }

    private void onClickOfRandomSpawnButtonForEnergySystems()
    {
        numberOfEnergySystemsToSpawn = GetEnergyStationsCount() + 5;
    }

    private int GetRobotsCount(SpawnCategory spawnCategory)
    {
        if(_world == null)
        {
            _world = World.DefaultGameObjectInjectionWorld;
        }

        EntityQuery query = _world.EntityManager.CreateEntityQuery(ComponentType.ReadOnly(typeof(T_Robot)));
        query.AddSharedComponentFilter(new T_Robot() { spawnCategory = spawnCategory });
        return query.CalculateEntityCount();
    }

    private int GetEnergyStationsCount()
    {
        if (_world == null)
        {
            _world = World.DefaultGameObjectInjectionWorld;
        }

        EntityQuery query = _world.EntityManager.CreateEntityQuery(ComponentType.ReadOnly(typeof(T_EnergyStation)));
        return query.CalculateEntityCount();
    }
}
