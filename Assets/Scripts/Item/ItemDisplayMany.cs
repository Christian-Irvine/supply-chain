using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class ItemDisplayMany : MonoBehaviour
{
    [SerializeField] private BuildingInventory buildingInventory;
    [SerializeField] private float scaleSize;
    [SerializeField] private List<Transform> displayPositions = new List<Transform>();
    public int MaxCount { get => displayPositions.Count; }

    private ItemDataSO currentItem;
    private List<GameObject> itemModels = new List<GameObject>();

    private void Start()
    {
        buildingInventory.InputStackModified.AddListener(OnInputStackChange);
        buildingInventory.InputStackCountChange.AddListener(UpdateDisplayCount);
    }

    private void OnInputStackChange()
    {
        if (buildingInventory.InputStacks.Count > 0)
        {
            ItemDataSO newItem = buildingInventory.InputStacks[0].Item;

            ChangeItem(newItem);
        }
    }

    public void ChangeItem(ItemDataSO newItem)
    {
        currentItem = newItem;

        itemModels.ForEach(item => { Destroy(item); });
        itemModels.Clear();

        for (int i = 0; i < MaxCount; i++) 
        {
            ItemObject item = Instantiate(currentItem.worldItem);
            item.transform.parent = displayPositions[i].transform;
            item.transform.localPosition = Vector3.zero;
            item.transform.localScale = Vector3.one * scaleSize;

            itemModels.Add(item.gameObject);
        }

        UpdateDisplayCount();
    }

    private void UpdateDisplayCount()
    {
        if (buildingInventory.InputStacks.Count > 0)
        {
            for (int i = 0; i < itemModels.Count; i++)
            {
                itemModels[i].SetActive(i < buildingInventory.InputStacks[0].Count);
            }
        }
        else
        {
            itemModels.ForEach (item => item.SetActive(false));
        }
    }
}
