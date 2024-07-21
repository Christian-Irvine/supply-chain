using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The Scriptable Object class for storing recipe data for machines
/// </summary>

[System.Serializable]
public class RecipeItem
{
    public ItemDataSO itemData;
    public int count = 1;
}

[CreateAssetMenu(fileName = "RecipeSO", menuName = "ScriptableObjects/Recipe", order = 1)]
public class RecipeSO : ScriptableObject
{
    public string recipeName;
    public List<RecipeItem> inputs;
    public List<RecipeItem> outputs;
    public float craftSpeed;
    public BuildingDataSO craftedInBuilding;
}
