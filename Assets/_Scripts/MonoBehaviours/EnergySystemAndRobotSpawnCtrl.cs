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

    public Button randomSpawnButton;
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
        randomSpawnButton.onClick.AddListener(onClickOfRandomSpawnButton);
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

    public SpawnCategory getSelectedSpawnCategory()
    {
        SpawnCategory spawnCategory;
        Enum.TryParse<SpawnCategory>(spawnCategoryDropDown.options[spawnCategoryDropDown.value].text, out spawnCategory);
        return spawnCategory;
    }

    private void onClickOfRandomSpawnButton()
    {

    }
}
