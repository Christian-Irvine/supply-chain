using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Conveyor : MonoBehaviour
{
    [SerializeField] private ItemSlot itemSlot;
    [SerializeField] private BuildingObject buildingObject;
    [SerializeField] private Transform pushCheckPosition;
    [SerializeField] private Transform pullCheckPosition;
    [SerializeField] private int itemsPerMinute;
    [SerializeField] private BuildingInventory pushInventory;
    [SerializeField] private ItemSlot pushSlot;

    private BuildingInventory pullInventory;
    private ItemSlot pullSlot;

    private int maxTickCooldown;
    private int cooldown;

    private IEnumerator Start()
    {
        maxTickCooldown = Mathf.RoundToInt(60f / ((float)itemsPerMinute / 60));
        cooldown = maxTickCooldown;

        buildingObject.CheckNeighbors.AddListener(CheckNeighbors);

        yield return null;

        TickManager.Instance.BeltPull.AddListener(PullItems);
        TickManager.Instance.BeltPush.AddListener(PushItems);
    }

    // Runs before PullItems
    private void PushItems()
    {
        TickCooldown();

        if (cooldown != 0) return;

        if (pushInventory != null) 
        {
            Debug.Log($"pushing into {pushInventory.name}");
        
        }
        if (pushSlot != null)
        {

        }
    }

    // Runs after PushItems
    private void PullItems()
    {
        if (cooldown != 0) return;

        if (pullInventory != null)
        {

        }
        if (pullSlot != null)
        {

        }
    }

    private void TickCooldown()
    {
        cooldown = (cooldown + 1) % maxTickCooldown;
    }

    // Should check front and behind to see if any buildings exist for pulling and pushing to
    private void CheckNeighbors()
    {
        // Getting Push Position
        List<Collider> pushPosItems = GridManager.Instance.GetBuildingsAtGridPosition(GridManager.Instance.RoundVector3ToInt(pushCheckPosition.position));

        pushInventory = null;
        pushSlot = null;

        foreach (Collider collider in pushPosItems)
        {
            BuildingInventory inventory = collider.GetComponent<BuildingInventory>();
            if (inventory != null)
            {
                pushInventory = inventory;
                break;
            }
            ItemSlot slot = collider.GetComponent<ItemSlot>();
            if (slot != null)
            {
                pushSlot = slot;
                break;
            }
        }

        // Getting Pull Position
        List<Collider> pullPosItems = GridManager.Instance.GetBuildingsAtGridPosition(GridManager.Instance.RoundVector3ToInt(pullCheckPosition.position));

        pullInventory = null;
        pullSlot = null;

        foreach (Collider collider in pullPosItems)
        {
            BuildingInventory inventory = collider.GetComponent<BuildingInventory>();
            if (inventory != null)
            {
                pullInventory = inventory;
                break;
            }
            ItemSlot slot = collider.GetComponent<ItemSlot>();
            if (slot != null)
            {
                pullSlot = slot;
                break;
            }
        }
    }
}
