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

    [SerializeField] private BuildingInventory pullInventory;
    [SerializeField] private ItemSlot pullSlot;

    // This will be replaced later on when the system actually exists (in the optimization phase)
    private bool isOnScreen = true;

    private int maxTickCooldown;

    private Vector3 itemLocalStartPosition = Vector3.zero;

    private IEnumerator Start()
    {
        maxTickCooldown = Mathf.RoundToInt(60f / ((float)itemsPerMinute / 60));

        buildingObject.CheckNeighbors.AddListener(CheckNeighbors);

        yield return null;

        TickManager.Instance.BeltPull.AddListener(PullItems);
        TickManager.Instance.BeltPush.AddListener(PushItems);
    }

    // Runs before PullItems
    private void PushItems(int tickCount)
    {
        int localTickCount = tickCount % maxTickCooldown;

        if (localTickCount != 0) return;
        if (itemSlot.WorldItem == null) return;

        if (pushInventory != null) 
        {
            bool itemAddedToInput = pushInventory.TryAddItemToInput(itemSlot.WorldItem.Item);

            if (itemAddedToInput)
            {
                Debug.Log($"Pushing {pushInventory.InputStacks[0].Item.displayName} from {pushInventory.name}!");
                // Pushing item into machine
                itemSlot.DestroyWorldItem();
            }
        }
        if (pushSlot != null)
        {
            if (pushSlot.WorldItem != null) return;

            if (itemSlot.WorldItem.LastMoveTick == tickCount) return;

            pushSlot.WorldItem = itemSlot.WorldItem;
            pushSlot.WorldItem.LastMoveTick = tickCount;
            itemSlot.WorldItem = null;
        }
    }

    // Runs after PushItems
    private void PullItems(int tickCount)
    {
        int localTickCount = tickCount % maxTickCooldown;

        if (itemSlot.WorldItem != null)
        {
            UpdateItemPosition(localTickCount);
            return;
        }

        if (localTickCount != 0) return;

        if (pullInventory != null)
        {
            // If nothing to pull return
            if (pullInventory.OutputStacks.Count == 0) return;

            Debug.Log($"Pulling {pullInventory.OutputStacks[0].Item.displayName} from {pullInventory.name}!");

            ItemDataSO item = pullInventory.OutputStacks[0].Item;

            pullInventory.ChangeOutputStackCount(0, -1);

            itemSlot.CreateWorldItem(item);

            itemLocalStartPosition = itemSlot.DefaultItemDisplayPosition.localPosition + new Vector3(0, 0, 0.5f);
        }
        if (pullSlot != null)
        {
            // This is what is used to stop belts from pulling from other belts (they can only push onto other belts)
            if (!pullSlot.CanBePulledFrom) return;

            // Pull from item slots
        }
    }

    private void UpdateItemPosition(int localTickCount)
    {
        Transform centerTransform = itemSlot.DefaultItemDisplayPosition;
        
        if (!isOnScreen)
        {
            itemSlot.WorldItem.transform.localPosition = centerTransform.localPosition;
            return;
        } 

        int halfCooldown = maxTickCooldown / 2;

        Vector3 endPos = centerTransform.localPosition + new Vector3(0, 0, -0.5f);

        if (Vector3.Distance(itemSlot.WorldItem.transform.localPosition, endPos) < 0.05f) return;
        
        // Move to center
        if (localTickCount < halfCooldown)
        {
            if (localTickCount == 0 && itemLocalStartPosition == Vector3.zero) itemLocalStartPosition = itemSlot.WorldItem.transform.localPosition;

            Vector3 itemPosition = Vector3.Lerp(itemLocalStartPosition, centerTransform.localPosition, (float)localTickCount / halfCooldown);
            itemSlot.WorldItem.transform.localPosition = itemPosition;
        }
        // Move to end
        else
        {
            Vector3 itemPosition = Vector3.Lerp(centerTransform.localPosition, endPos, ((float)localTickCount - halfCooldown) / halfCooldown);
            itemSlot.WorldItem.transform.localPosition = itemPosition;

            if (localTickCount == maxTickCooldown) itemLocalStartPosition = Vector3.zero;
        }
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
