using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Progress;

public class PlayerItemInteraction : MonoBehaviour
{
    [SerializeField] private float interactionDistance;
    [SerializeField] private ItemSlot playerItemSlot;

    private void Start()
    {
        InputSystem.actions.FindAction("Interact").performed += ctx => Interact(ctx);
    }

    private void Interact(InputAction.CallbackContext ctx)
    {
        RaycastHit hit = CheckForwardObjectRaycast();

        if (hit.collider == null) return;

        ItemSlot slot = hit.collider.GetComponent<ItemSlot>();

        if (slot)
        {
            SwitchItems(slot);
            return;
        }

        BuildingInventory buildingInventory = hit.collider.GetComponent<BuildingInventory>();

        if (buildingInventory)
        {
            if (playerItemSlot.WorldItem != null)
            {
                DepositItems(buildingInventory);
            }
            else
            {
                TakeItems(buildingInventory);
            }
        }
    }

    private void SwitchItems(ItemSlot slot)
    {
        ItemObject tempItem = slot.WorldItem;
        slot.WorldItem = playerItemSlot.WorldItem;
        playerItemSlot.WorldItem = tempItem;
    }

    private void DepositItems(BuildingInventory buildingInventory)
    {
        int index = buildingInventory.InputStacks.FindIndex(stack => stack.Item == playerItemSlot.WorldItem.Item);

        if (index != -1)
        {
            buildingInventory.ChangeInputStackCount(playerItemSlot.WorldItem.Item, 1);
            playerItemSlot.WorldItem = null;
        }
        else
        {
            if (buildingInventory.AddInputStack(playerItemSlot.WorldItem.Item, 1))
            {
                playerItemSlot.WorldItem = null;
            }
        }
    }

    private void TakeItems(BuildingInventory buildingInventory)
    {
        if (buildingInventory.OutputStacks.Count > 0)
        {
            ItemDataSO item = buildingInventory.OutputStacks[0].Item;
            buildingInventory.ChangeOutputStackCount(item, -1);

            // Creates a new world item for the player to hold
            playerItemSlot.WorldItem = Instantiate(item.worldItem);
        }
    }

    private RaycastHit CheckForwardObjectRaycast()
    {
        RaycastHit hit;
        Physics.Raycast(transform.position, transform.forward, out hit, interactionDistance);

        return hit;
    }
}
