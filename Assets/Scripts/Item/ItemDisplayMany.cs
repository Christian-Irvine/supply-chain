using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class ItemDisplayMany : MonoBehaviour
{
    [SerializeField] private float scaleSize;
    [SerializeField] private List<Transform> displayPositions;
    public int MaxCount { get => displayPositions.Count; }

    private int activeCount = 0;
    public int ActiveCount { get => activeCount; set 
        {
            activeCount = value;
            UpdateDisplayCount();
        } 
    }

    private ItemDataSO currentItem;
    private List<GameObject> itemModels;

    public void ChangeItem(ItemDataSO newItem)
    {
        currentItem = newItem;

        itemModels.ForEach(item => Destroy(item));
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
        for (int i = 0; i < MaxCount; i++)
        {
            itemModels[i].SetActive(i < ActiveCount);
        }
    }
}
