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

    private void OnClick(InputAction.CallbackContext ctx)
    {
        RaycastHit hit = CheckClickRaycast();
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
    private RaycastHit CheckClickRaycast()
    {
        Ray ray = cam.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit hit;
        Physics.Raycast(ray, out hit, rayDistance, ~rayIgnoreMask);

        return hit;
    }
}
