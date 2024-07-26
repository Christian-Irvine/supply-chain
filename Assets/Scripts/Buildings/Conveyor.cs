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
    [SerializeField] private BuildingObject pushBuilding;
    [SerializeField] private BuildingObject pullBuilding;

    private IEnumerator Start()
    {
        buildingObject.CheckNeighbors.AddListener(CheckNeighbors);

        yield return null;

        TickManager.Instance.BeltPull.AddListener(PullItems);
        TickManager.Instance.BeltPush.AddListener(PushItems);
    }

    // Runs before PullItems
    private void PushItems()
    {
        if (pushBuilding == null) return;
    }

    // Runs after PushItems
    private void PullItems()
    {
        if (pullBuilding == null) return;
    }

    // Should check front and behind to see if any buildings exist for pulling and pushing to
    private void CheckNeighbors()
    {
        // Getting Push Position
        List<Collider> pushPosItems = GridManager.Instance.GetBuildingsAtGridPosition(GridManager.Instance.RoundVector3ToInt(pushCheckPosition.position));

        pushBuilding = null;

        foreach (Collider collider in pushPosItems)
        {
            BuildingObject pushObject = pushPosItems[0].GetComponent<BuildingObject>();
            if (pushObject != null)
            {
                pushBuilding = pushObject;
                continue;
            }
        }

        // Getting Pull Position
        List<Collider> pullPosItems = GridManager.Instance.GetBuildingsAtGridPosition(GridManager.Instance.RoundVector3ToInt(pullCheckPosition.position));

        pullBuilding = null;

        foreach (Collider collider in pullPosItems)
        {
            BuildingObject pullObject = pullPosItems[0].GetComponent<BuildingObject>();
            if (pullObject != null)
            {
                pullBuilding = pullObject;
                continue;
            }
        }
        
    }
}
