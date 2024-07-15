using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerPlacing : MonoBehaviour
{
    [SerializeField] Camera cam;
    [SerializeField] float rayDistance = 50;
    [SerializeField] LayerMask rayIgnoreMask;
    [SerializeField] private GameObject prefab;

    void Start()
    {
        InputSystem.actions.FindAction("Place").performed += ctx => OnClick(ctx);
    }

    private void OnClick(InputAction.CallbackContext ctx)
    {
        RaycastHit hit = CheckClickRaycast();
        if (hit.collider == null) return;

        if (hit.collider.CompareTag("FactoryFloor"))
        {
            GridManager.Instance.PlaceBuilding(prefab, new Vector3(hit.point.x, 0, hit.point.z));
        }
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
