using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerPlacing : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private float rayDistance = 50;
    [SerializeField] private LayerMask rayIgnoreMask;
    [SerializeField] private GameObject tempCube;

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
            foreach (FloorType type in BuildingManager.Instance.PickedBuilding.floorTypes)
            {
                if (hit.collider.CompareTag($"{type}Floor"))
                {
                    PlacementGhost.Instance.GhostModel.gameObject.SetActive(true);
                    Vector3 ghostPos = new Vector3(hit.point.x, 0, hit.point.z) - BuildingManager.Instance.EvenOffsets;

                    PlacementGhost.Instance.gameObject.transform.position = GridManager.Instance.GridToWorldPosition(ghostPos) + BuildingManager.Instance.EvenOffsets;
                    return;
                }
            }
        }

        PlacementGhost.Instance.GhostModel.gameObject.SetActive(false);
    }

    private void OnClick(InputAction.CallbackContext ctx)
    {
        RaycastHit hit = CheckPlacementRaycast();
        if (hit.collider == null) return;

        BuildingDataSO buildingData = BuildingManager.Instance.PickedBuilding;
        GameObject prefab = buildingData.prefab;

        Vector3 hitPos = GridManager.Instance.GridToWorldPosition(new Vector3(hit.point.x, 0, hit.point.z) - BuildingManager.Instance.EvenOffsets);

        if (CanPlaceStructure(hitPos, buildingData.size))
        {
            buildingData.floorTypes.ForEach(type =>
            {
                if (hit.collider.CompareTag($"{type}Floor"))
                {
                    GridManager.Instance.PlaceBuilding(prefab, new Vector3(hit.point.x, 0, hit.point.z) - BuildingManager.Instance.EvenOffsets);
                }
            });
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
        Physics.Raycast(ray, out hit, rayDistance, ~rayIgnoreMask);

        return hit;
    }

    private bool CanPlaceStructure(Vector3 centreGridPosition, Vector2Int size)
    {
        Vector3 bottomLeftPosition = GetBottomLeftPosition(centreGridPosition, size);

        Instantiate(tempCube, bottomLeftPosition, Quaternion.identity);
        return false;
    }

    private Vector3 GetBottomLeftPosition(Vector3 centreGridPosition, Vector2Int size)
    {
        Vector3 bottomLeftPosition = new Vector3(
            GetBottomLeftNumber(Mathf.RoundToInt(centreGridPosition.x), size.x),
            centreGridPosition.y,
            GetBottomLeftNumber(Mathf.RoundToInt(centreGridPosition.z), size.y)
        );

        return bottomLeftPosition;
    }

    private int GetBottomLeftNumber(int position, int length)
    {
        return position - Mathf.CeilToInt(((float)length / 2) - 1);
    }
}
