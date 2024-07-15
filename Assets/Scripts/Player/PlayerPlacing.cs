using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerPlacing : MonoBehaviour
{
    [SerializeField] Camera cam;
    [SerializeField] float rayDistance = 50;
    [SerializeField] LayerMask rayIgnoreMask;

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
                    PlacementGhost.Instance.gameObject.transform.position = GridManager.Instance.GridToWorldPosition(new Vector3(hit.point.x, 0, hit.point.z));
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

        buildingData.floorTypes.ForEach(type =>
        {
            if (hit.collider.CompareTag($"{type}Floor"))
            {
                GridManager.Instance.PlaceBuilding(prefab, new Vector3(hit.point.x, 0, hit.point.z));
            }
        });
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
}
