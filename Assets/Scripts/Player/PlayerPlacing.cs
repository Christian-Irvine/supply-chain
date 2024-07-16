using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerPlacing : MonoBehaviour
{
    public enum InvalidReason
    {
        None,
        OverlapBuilding,
        WrongFloor
    }

    [SerializeField] private Camera cam;
    [SerializeField] private float rayDistance = 50;
    [SerializeField] private LayerMask clickRayIgnoreMask;
    [SerializeField] private LayerMask tileCheckIgnoreMask;
    [SerializeField] private GameObject tempCube;
    private InvalidReason invalidReason;

    void Start()
    {
        InputSystem.actions.FindAction("Place").performed += ctx => OnClick(ctx);
    }

    private void FixedUpdate()
    {
        if (BuildingManager.Instance.PickedBuilding == null) return;

        RaycastHit hit = CheckPlacementRaycast();

        if (hit.collider != null)
        {
            Vector3 hitGridPos = GridManager.Instance.GridToWorldPosition(new Vector3(hit.point.x, 0, hit.point.z) - BuildingManager.Instance.EvenOffsets);
            PlacementGhost.Instance.GhostModel.gameObject.SetActive(true);
            Vector3 ghostPos = new Vector3(hit.point.x, 0, hit.point.z) - BuildingManager.Instance.EvenOffsets;
            PlacementGhost.Instance.gameObject.transform.position = GridManager.Instance.GridToWorldPosition(ghostPos) + BuildingManager.Instance.EvenOffsets;

            bool placementValid = CanPlaceStructure(hitGridPos, BuildingManager.Instance.PickedBuilding);

            PlacementGhost.Instance.SetValidity(placementValid);
        }
        else
        {
            PlacementGhost.Instance.GhostModel.gameObject.SetActive(false);
        }
    }

    private void OnClick(InputAction.CallbackContext ctx)
    {
        RaycastHit hit = CheckPlacementRaycast();
        if (hit.collider == null) return;

        BuildingDataSO buildingData = BuildingManager.Instance.PickedBuilding;
        GameObject prefab = buildingData.prefab;

        Vector3 hitGridPos = GridManager.Instance.GridToWorldPosition(new Vector3(hit.point.x, 0, hit.point.z) - BuildingManager.Instance.EvenOffsets);

        if (CanPlaceStructure(hitGridPos, buildingData))
        {
            GridManager.Instance.PlaceBuilding(prefab, new Vector3(hit.point.x, 0, hit.point.z) - BuildingManager.Instance.EvenOffsets);
        }
    }

    /// <summary>
    /// Returns the hit of what the player clicks on
    /// </summary>
    /// <returns></returns>
    private RaycastHit CheckPlacementRaycast()
    {
        Ray ray = cam.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit hit;
        Physics.Raycast(ray, out hit, rayDistance, ~clickRayIgnoreMask);

        return hit;
    }

    private bool CanPlaceStructure(Vector3 centreGridPosition, BuildingDataSO buildingData)
    {
        Vector3Int bottomLeftPosition = GetBottomLeftPosition(centreGridPosition, buildingData.size);

        for (int x = bottomLeftPosition.x; x < bottomLeftPosition.x + buildingData.size.x; x++)
        {
            for (int z = bottomLeftPosition.z; z < bottomLeftPosition.z + buildingData.size.y; z++)
            {
                RaycastHit hit = RaycastGridCell(new Vector3(x, 2, z));

                if (hit.collider == null) return false;

                if (hit.collider.CompareTag("Building"))
                {
                    invalidReason = InvalidReason.OverlapBuilding;
                    return false;
                }

                foreach(FloorType type in buildingData.floorTypes)
                {
                    if (!hit.collider.CompareTag($"{type}Floor"))
                    {
                        invalidReason = InvalidReason.WrongFloor;
                        return false;
                    }
                }
            }
        }

        invalidReason = InvalidReason.None;
        return true;

        // Instantiate(tempCube, new Vector3(x, 0, z), Quaternion.identity);
    }

    private Vector3Int GetBottomLeftPosition(Vector3 centreGridPosition, Vector2Int size)
    {
        Vector3Int bottomLeftPosition = new Vector3Int(
            GetBottomLeftNumber(Mathf.RoundToInt(centreGridPosition.x), size.x),
            Mathf.RoundToInt(centreGridPosition.y),
            GetBottomLeftNumber(Mathf.RoundToInt(centreGridPosition.z), size.y)
        );

        return bottomLeftPosition;
    }

    private int GetBottomLeftNumber(int position, int length)
    {
        return position - Mathf.CeilToInt(((float)length / 2) - 1);
    }

    private RaycastHit RaycastGridCell(Vector3 position)
    {
        RaycastHit hit;
        Physics.Raycast(position, Vector3.down, out hit, 3, ~tileCheckIgnoreMask);
        return hit;
    }
}
