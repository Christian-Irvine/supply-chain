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
        currentRecipe = GetValidRecipe();

        Debug.Log(currentRecipe.recipeName);

        if (currentRecipe == null) return;
    }

    // The count of an ItemStack changing but remaining the same
    private void OnInputCountChange()
    {

    }

    private RecipeSO GetValidRecipe()
    {
        int itemStacks = inventory.InputStacks.Count;

        if (itemStacks == 0)
        {
            return null;
        }

        List<RecipeSO> possibleRecipes = new List<RecipeSO>();

        // Gets all recipes which has the same amount of slots as the current inventory has filled
        possibleRecipes = RecipeManager.Instance.RegisteredRecipes.Where(recipe => recipe.inputs.Count == itemStacks).ToList(); //  && recipe.inputs[0].itemData == inventory.InputStacks[0].Item

        foreach (RecipeSO recipe in possibleRecipes)
        {
            bool validItem = true;

            foreach (RecipeItem recipeItem in recipe.inputs)
            {
                if (!inventory.InputStacks.Any(itemStack => itemStack.Item == recipeItem.itemData))
                {
                    validItem = false;
                    break;
                }
            }

            if (validItem) return recipe;
        }

        return null;
    }
}
