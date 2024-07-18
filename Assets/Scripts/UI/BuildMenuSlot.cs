using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// This class is used to put data on the buildings inside of the BuildMenu 
/// </summary>
public class BuildMenuSlot
{
    public BuildingDataSO buildingData;
    public Button button;
    public Label nameLabel;
    public Label costLabel;

    public BuildMenuSlot(BuildingDataSO buildingData, VisualTreeAsset template)
    {
        this.buildingData = buildingData;

        // Creating an instance of the building slot template
        TemplateContainer buildingButtonContainer = template.Instantiate();

        button = buildingButtonContainer.Q<Button>("Button");
        button.RegisterCallback<ClickEvent>(OnClick);

        nameLabel = buildingButtonContainer.Q<Label>("Name");
        costLabel = buildingButtonContainer.Q<Label>("Cost");
    }

    public void OnClick(ClickEvent evt)
    {
        Debug.Log($"Building {buildingData.displayName} has been selected");
    }
}
