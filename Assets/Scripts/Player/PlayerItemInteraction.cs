using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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
        Debug.Log("Depositing item!");

        int index = buildingInventory.InputStacks.FindIndex(stack => stack.Item == playerItemSlot.WorldItem.Item);

        if (index != -1)
        {
            if (buildingInventory.GetInputStack(playerItemSlot.WorldItem.Item).Count < buildingInventory.GetMaxStackSize(playerItemSlot.WorldItem.Item))
            {
                buildingInventory.ChangeInputStackCount(playerItemSlot.WorldItem.Item, 1);
                playerItemSlot.DestroyWorldItem();
                playerItemSlot.WorldItem = null;
            }
        }
        else
        {
            if (buildingInventory.AddInputStack(playerItemSlot.WorldItem.Item, 1))
            {
                playerItemSlot.DestroyWorldItem();
                playerItemSlot.WorldItem = null;
            }
        }
    }

    private void TakeItems(BuildingInventory buildingInventory)
    {
        // Takes from output stack if it can exist
        if (buildingInventory.OutputStackAmount > 0)
        {
            if (buildingInventory.OutputStacks.Count > 0)
            {
                Debug.Log(buildingInventory.OutputStacks.Count);
                ItemDataSO item = buildingInventory.OutputStacks[0].Item;
                Debug.Log(item.displayName);
                buildingInventory.ChangeOutputStackCount(item, -1);

                // Creates a new world item for the player to hold
                playerItemSlot.WorldItem = Instantiate(item.worldItem);
                playerItemSlot.WorldItem.Item = item;
            }
        }
        // However if only the input stack can exist the player can take from input. But never if the output can exist and just doesn't (e.g shelves where only input will exist)
        else
        {
            if (buildingInventory.InputStackAmount > 0)
            {
                if (buildingInventory.InputStacks.Count > 0)
                {
                    ItemDataSO item = buildingInventory.InputStacks[0].Item;
                    buildingInventory.ChangeInputStackCount(item, -1);

                    // Creates a new world item for the player to hold
                    playerItemSlot.WorldItem = Instantiate(item.worldItem);
                    playerItemSlot.WorldItem.Item = item;
                }
            }
        }
    }

    private RaycastHit CheckForwardObjectRaycast()
    {
        RaycastHit hit;
        Physics.Raycast(transform.position, transform.forward, out hit, interactionDistance);

        return hit;
    }
}
