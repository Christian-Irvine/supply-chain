using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Class to process a machines items
/// </summary>
public class MachineObject : MonoBehaviour
{
    [SerializeField] private BuildingInventory inventory;

    private RecipeSO currentRecipe;

    void Start()
    {
        inventory.InputStackModified.AddListener(OnInputItemChange);
        inventory.InputStackCountChange.AddListener(OnInputCountChange);
    }

    // An ItemStack being changed to a new ItemStack or null
    private void OnInputItemChange()
    {
        int itemStacks = inventory.InputStacks.Count;

        if (itemStacks == 0) 
        {
            currentRecipe = null;
            return;
        }

        List<RecipeSO> possibleRecipes = new List<RecipeSO>();

        possibleRecipes = RecipeManager.Instance.RegisteredRecipes.Where(recipe => recipe.inputs.Count == itemStacks && recipe.inputs[0].itemData == inventory.InputStacks[0].Item).ToList();

        Debug.Log(possibleRecipes.Count);
    }

    // The count of an ItemStack changing but remaining the same
    private void OnInputCountChange()
    {

    }


}
