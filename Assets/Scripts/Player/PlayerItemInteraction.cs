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
        }
    }

    private void SwitchItems(ItemSlot slot)
    {
        WorldItem tempItem = slot.WorldItem;
        slot.WorldItem = playerItemSlot.WorldItem;
        playerItemSlot.WorldItem = tempItem;
    }

    private RaycastHit CheckForwardObjectRaycast()
    {
        RaycastHit hit;
        Physics.Raycast(transform.position, transform.forward, out hit, interactionDistance);

        return hit;
    }
}
