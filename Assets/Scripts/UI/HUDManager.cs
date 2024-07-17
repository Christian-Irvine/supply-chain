using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class HUDManager : MonoBehaviour
{
    public static HUDManager Instance;

    [SerializeField] private string moneyTextElementName;
    private Label moneyText;
    private VisualElement hud;

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);

        hud = GetComponent<UIDocument>().rootVisualElement;
    }

    // Use OnEnable() for getting UI elements
    private void OnEnable()
    {
        moneyText = hud.Q<Label>(moneyTextElementName);
        
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
}
