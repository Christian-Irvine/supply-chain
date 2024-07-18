using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.VFX;

/// <summary>
/// This class is used to put data on the buildings inside of the BuildMenu 
/// </summary>
public class BuildMenuSlot
{
    public BuildingDataSO buildingData;
    public Button button;
    public Label nameLabel;
    public Label costLabel;
    public VisualElement spriteElement;

    public BuildMenuSlot(BuildingDataSO buildingData, VisualTreeAsset template)
    {
        this.buildingData = buildingData;

        // Creating an instance of the building slot template
        TemplateContainer buildingButtonContainer = template.Instantiate();

        button = buildingButtonContainer.Q<Button>("Button");
        button.RegisterCallback<ClickEvent>(OnClick);

        nameLabel = buildingButtonContainer.Q<Label>("Name");
        nameLabel.text = buildingData.name;

        costLabel = buildingButtonContainer.Q<Label>("Cost");
        costLabel.text = $"{buildingData.purchaseCost:C0}";

        spriteElement = buildingButtonContainer.Q<VisualElement>("Sprite");
        spriteElement.style.backgroundImage = new StyleBackground(buildingData.sprite);
    }

    public void OnClick(ClickEvent evt)
    {
        Debug.Log($"Building {buildingData.displayName} has been selected");
    }
}
