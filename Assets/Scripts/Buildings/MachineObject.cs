using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEditor.Rendering.CameraUI;

/// <summary>
/// Class to process a machines items
/// </summary>
public class MachineObject : MonoBehaviour
{
    [SerializeField] private BuildingInventory inventory;
    [Tooltip("Bigger number is faster"), SerializeField] private float craftSpeed;

    private RecipeSO currentRecipe;
    private bool recipeCountValid;
    private bool isCrafting = false;

    void Start()
    {
        inventory.InputStackModified.AddListener(OnInputItemChange);
        inventory.InputStackCountChange.AddListener(OnInputCountChange);

        inventory.OutputStackModified.AddListener(OnOutputChange);
        inventory.OutputStackCountChange.AddListener(OnOutputChange);
    }

    // An ItemStack being changed to a new ItemStack or null
    private void OnInputItemChange()
    {
        currentRecipe = GetValidRecipe();

        if (currentRecipe == null) return;

        recipeCountValid = CheckRecipeCount();

        TryCraft();
    }

    // The count of an ItemStack changing but remaining the same
    private void OnInputCountChange()
    {
        recipeCountValid = CheckRecipeCount();

        TryCraft();
    }

    // The output changing in any way
    private void OnOutputChange()
    {
        TryCraft();
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

    private bool CheckRecipeCount()
    {
        if (currentRecipe == null) return false;

        foreach (RecipeItem recipeItem in currentRecipe.inputs)
        {
            ItemStack inventoryStack = inventory.GetInputStack(recipeItem.itemData); // inventory.InputStacks.Find(itemStack => itemStack.Item == recipeItem.itemData

            if (inventoryStack.Count < recipeItem.count) return false;
        }

        return true;
    }

    // Checks if there is space in the output slot for the current recipe to craft
    private bool CheckOutputSpace()
    {
        foreach(RecipeItem recipeOutputStack in currentRecipe.outputs)
        {
            ItemStack stack = inventory.GetOutputStack(recipeOutputStack.itemData);

            if (stack == null)
            {
                if (inventory.OutputStackAmount == inventory.OutputStacks.Count)
                {
                    return false;
                } 
            }
            else
            {
                if (recipeOutputStack.count + stack.Count > inventory.GetMaxStackSize(stack.Item))
                {
                    return false;
                }
            }
        }

        return true;
    }

    private void TryCraft()
    {
        if (!recipeCountValid || currentRecipe == null || !CheckOutputSpace())
        {
            // Cancelling the craft because it is invalid
            if (isCrafting)
            {
                StopCoroutine(CraftItem());
            }
            return;
        }
        // Returning because craft is already going on and is still valid
        if (isCrafting) return;

        StartCoroutine(CraftItem());
    }

    private IEnumerator CraftItem()
    {
        isCrafting = true;
        Debug.Log("Crafting!");

        yield return new WaitForSeconds(currentRecipe.craftSpeed / craftSpeed);
        ChangeCraftItems();
        isCrafting = false;

        // Tries to craft again if possible
        TryCraft();
    }

    private void ChangeCraftItems()
    {
        RecipeSO craftedRecipe = currentRecipe;

        // Removing items from inputs
        craftedRecipe.inputs.ForEach(input => 
        {
            inventory.ChangeInputStackCount(input.itemData, -input.count);
        });

        Debug.Log(craftedRecipe.outputs);

        // Adding items to outputs
        craftedRecipe.outputs.ForEach(output =>
        {
            ItemStack stack = inventory.GetOutputStack(output.itemData);

            if (stack != null)
            {
                inventory.ChangeOutputStackCount(stack.Item, output.count);
            }
            else
            {
                inventory.AddOutputStack(output.itemData, output.count);
            }
        });

        Debug.Log("Crafted new items");
    }
}
