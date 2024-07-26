using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Conveyor : MonoBehaviour
{
    [SerializeField] private ItemSlot itemSlot;
    [SerializeField] private BuildingObject buildingObject;
    private BuildingInventory pullInventory;
    private BuildingObject pushInventory;

    private IEnumerator Start()
    {
        buildingObject.UpdateNeighbors.AddListener(CheckNeighbors);

        yield return null;

        TickManager.Instance.BeltPull.AddListener(PullItems);
        TickManager.Instance.BeltPush.AddListener(PushItems);
    }

    // Runs before PullItems
    private void PushItems()
    {
        if (pushInventory == null) return;


    }

    // Runs after PushItems
    private void PullItems()
    {
        if (pullInventory == null) return;


    }

    // Should check front and behind to see if any buildings exist for pulling and pushing to
    private void CheckNeighbors()
    {
        Debug.Log("I am a conveyor and I have neighbors probably");
    }
}
