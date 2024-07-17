using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GUIManager : MonoBehaviour
{
    public static GUIManager Instance;

    [SerializeField] private UIDocument hud;
    [SerializeField] private UIDocument buildMenu;
    [SerializeField] private UIDocument storage;
    [SerializeField] private UIDocument ingredientStore;
    [SerializeField] private UIDocument escapeMenu;


    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        Instance = this;
    }

    private void ToggleHUD()
    {
        bool newState = !hud.isActiveAndEnabled;

        hud.enabled = newState;
    }

    private void ToggleBuildMenu()
    {
        bool newState = !buildMenu.isActiveAndEnabled;

        buildMenu.enabled = newState;
        
        if (newState)
        {
            storage.enabled = false;
            ingredientStore.enabled = false;
        }
    }

    private void ToggleStorage()
    {
        bool newState = !storage.isActiveAndEnabled;

        storage.enabled = newState;

        if (newState)
        {
            buildMenu.enabled = false;
            ingredientStore.enabled = false;
        }
    }

    private void ToggleIngredientStore()
    {
        bool newState = !ingredientStore.isActiveAndEnabled;

        ingredientStore.enabled = newState;

        if (newState)
        {
            storage.enabled = false;
            buildMenu.enabled = false;
        }
    }

    private void ToggleEscapeMenu()
    {
        bool newState = !escapeMenu.isActiveAndEnabled;

        escapeMenu.enabled = newState;

        hud.enabled = !newState;

        if (newState)
        {
            ingredientStore.enabled = false;
            storage.enabled = false;
            buildMenu.enabled = false;
        }
    }
}
