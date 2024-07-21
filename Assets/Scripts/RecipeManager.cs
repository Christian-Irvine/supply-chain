using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecipeManager : MonoBehaviour
{
    public static RecipeManager Instance;

    [SerializeField] List<RecipeSO> registeredRecipes = new List<RecipeSO>();
    public List<RecipeSO> RegisteredRecipes { get => registeredRecipes; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }

        Instance = this;
    }


}
