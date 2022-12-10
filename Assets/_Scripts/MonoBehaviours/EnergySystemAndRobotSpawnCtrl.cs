using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum SpawnCategory
{
    ROBOT_CATEGORY_1,
    ROBOT_CATEGORY_2,
    ROBOT_CATEGORY_3,
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
        spawnCategoryDropDown.options.Add(new TMP_Dropdown.OptionData() { text = SpawnCategory.ROBOT_CATEGORY_1.ToString() });
        spawnCategoryDropDown.options.Add(new TMP_Dropdown.OptionData() { text = SpawnCategory.ROBOT_CATEGORY_2.ToString() });
        spawnCategoryDropDown.options.Add(new TMP_Dropdown.OptionData() { text = SpawnCategory.ROBOT_CATEGORY_3.ToString() });
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
        SpawnCategory spawnCategory;
        Enum.TryParse<SpawnCategory>(spawnCategoryDropDown.options[spawnCategoryDropDown.value].text, out spawnCategory);
        return spawnCategory;
    }

    private void onClickOfRandomSpawnButtonForCategory1Robots()
    {
        numberOfCategory1RobotsToSpawn += 50;
    }

    private void onClickOfRandomSpawnButtonForCategory2Robots()
    {
        numberOfCategory2RobotsToSpawn += 50;
    }

    private void onClickOfRandomSpawnButtonForCategory3Robots()
    {
        numberOfCategory3RobotsToSpawn += 50;
    }

    private void onClickOfRandomSpawnButtonForEnergySystems()
    {
        numberOfEnergySystemsToSpawn += 5;
    }
}
