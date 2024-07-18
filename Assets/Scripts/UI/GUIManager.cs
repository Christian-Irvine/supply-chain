using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
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

    private void Start()
    {
        InputSystem.actions.FindAction("OpenBuildMenu").performed += ctx => ToggleBuildMenu(ctx);
        InputSystem.actions.FindAction("Escape").performed += ctx => OnEscapePress(ctx);
    }

    // Handles the press of the escape button
    private void OnEscapePress(InputAction.CallbackContext ctx = new InputAction.CallbackContext())
    {
        if (escapeMenu.isActiveAndEnabled) ToggleEscapeMenu();
        else if (BuildingManager.Instance.PickedBuilding != null) BuildingManager.Instance.PickedBuilding = null;
        else if (buildMenu.isActiveAndEnabled) ToggleBuildMenu();
        else ToggleEscapeMenu();
    }

    // Toggles the select building menu GUI (This is the correct method to do it)
    public void ToggleBuildMenu(InputAction.CallbackContext ctx = new InputAction.CallbackContext())
    {
        if (escapeMenu.enabled) return;

        bool newState = !buildMenu.isActiveAndEnabled;
        
        if (newState)
        {
            BuildMenuManager.Instance.EnableUI();
            storage.enabled = false;
            ingredientStore.enabled = false;
        }
        else
        {
            BuildMenuManager.Instance.DisableUI();
        }
    }

    public void ToggleStorageMenu()
    {
        if (escapeMenu.enabled) return;

        bool newState = !storage.isActiveAndEnabled;

        storage.enabled = newState;

        if (newState)
        {
            buildMenu.enabled = false;
            ingredientStore.enabled = false;
        }
    }

    public void ToggleIngredientStoreMenu()
    {
        if (escapeMenu.enabled) return;

        bool newState = !ingredientStore.isActiveAndEnabled;

        ingredientStore.enabled = newState;

        if (newState)
        {
            storage.enabled = false;
            buildMenu.enabled = false;
        }
    }

    public void ToggleEscapeMenu()
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
