using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class HUDManager : MonoBehaviour
{
    public static HUDManager Instance;

    [SerializeField] private string moneyTextElementName;
    [SerializeField] private string buildMenuElementName;
    [SerializeField] private string storageElementName;
    [SerializeField] private string ingredientStoreElementName;
    private VisualElement hud;
    private Label moneyText;
    private Button buildMenuButton;
    private Button storageButton;
    private Button ingredientStoreButton;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }

        Instance = this;

        hud = GetComponent<UIDocument>().rootVisualElement;
    }

    // Use OnEnable() for getting UI elements
    private void OnEnable()
    {
        moneyText = hud.Q<Label>(moneyTextElementName);

        buildMenuButton = hud.Q<Button>(buildMenuElementName);
        buildMenuButton.clicked += ToggleBuildMenu;

        storageButton = hud.Q<Button>(storageElementName);
        storageButton.clicked += ToggleStorageMenu;

        ingredientStoreButton = hud.Q<Button>(ingredientStoreElementName);
        ingredientStoreButton.clicked += ToggleIngredientMenu;
    }

    private void Start()
    {
        GameManager.Instance.MoneyChange.AddListener(UpdateMoneyText);
    }

    private void UpdateMoneyText()
    {
        if (moneyText == null) return;

        moneyText.text = $"{GameManager.Instance.Money:C0}";
    }

    private void ToggleBuildMenu()
    {
        GUIManager.Instance.ToggleBuildMenu();
    }

    private void ToggleStorageMenu()
    {
        GUIManager.Instance.ToggleStorageMenu();
    }

    private void ToggleIngredientMenu() 
    {
        GUIManager.Instance.ToggleIngredientStoreMenu();
    }
}
