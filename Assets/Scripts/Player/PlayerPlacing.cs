using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements.Experimental;

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
    [SerializeField] private float raycastHeight;
    private BuildingFacing currentFacing = BuildingFacing.forward;
    private InvalidReason invalidReason = InvalidReason.None;

    void Start()
    {
        InputSystem.actions.FindAction("Place").performed += ctx => OnClick(ctx);
        InputSystem.actions.FindAction("Rotate").performed += ctx => Rotate(ctx);
    }

    private void FixedUpdate()
    {
        if (BuildingManager.Instance.PickedBuilding == null) return;

        if (!BuildingManager.Instance.PickedBuilding.isRotatable) currentFacing = BuildingFacing.forward;

        RaycastHit hit = CheckPlacementRaycast();

        if (hit.collider != null)
        {
            Vector3 rotatedOffsets = BuildingManager.Instance.EvenOffsets;

            // Reverses the size if the rotation is left or right
            if (((int)currentFacing) % 2 != 0)
            {
                rotatedOffsets = new Vector3(rotatedOffsets.z, rotatedOffsets.y, rotatedOffsets.x);
            }

            Vector3 hitGridPos = GridManager.Instance.GridToWorldPosition(new Vector3(hit.point.x, 0, hit.point.z) - rotatedOffsets);
            PlacementGhost.Instance.GhostModel.gameObject.SetActive(true);
            Vector3 ghostPos = new Vector3(hit.point.x, 0, hit.point.z) - rotatedOffsets;
            PlacementGhost.Instance.gameObject.transform.position = GridManager.Instance.GridToWorldPosition(ghostPos) + rotatedOffsets;

            PlacementGhost.Instance.SetRotation(currentFacing);

            bool placementValid = CanPlaceStructure(hitGridPos, BuildingManager.Instance.PickedBuilding, currentFacing);

            PlacementGhost.Instance.SetValidity(placementValid);
            //Debug.Log("ghost: " + ghostPos);
        }
        else
        {
            PlacementGhost.Instance.GhostModel.gameObject.SetActive(false);
        }

        // Set text for invalidity based on invalidReason variable
    }

    // Tries to place something when mouse is clicked. I don't know how it works this entire placing system could do with restarting escpecially this method (please commit before you do)
    private void OnClick(InputAction.CallbackContext ctx)
    {
        BuildingDataSO buildingData = BuildingManager.Instance.PickedBuilding;

        if (buildingData == null) return;

        GameObject prefab = buildingData.prefab;

        RaycastHit hit = CheckPlacementRaycast();
        if (hit.collider == null) return;

        Vector3 rotatedOffsets = BuildingManager.Instance.EvenOffsets;

        // Special cases for the size if the rotation is left or right
        if ((((int)currentFacing) % 2) != 0) // odd number
        {
            rotatedOffsets = new Vector3(rotatedOffsets.z, rotatedOffsets.y, rotatedOffsets.x);
        }
        
        // This needs to be done before 1 and 2 are direction flipped but after its reversed so don't move it
        Vector3 placementCheckingGridPos = GridManager.Instance.GridToWorldPosition(new Vector3(hit.point.x, 0, hit.point.z) - rotatedOffsets);
        
        // Flipping the offsets if its facing right, or backward (It is necessary despite not knowing why)
        if (((int)currentFacing) == 1 || (int)currentFacing == 2) // 1 and 2
        {
            rotatedOffsets = new Vector3(rotatedOffsets.x * -1, rotatedOffsets.y, rotatedOffsets.z * -1);
        }

        // For if the x size is even when rotated left or right edge case (I don't know why)
        if ((((int)currentFacing) % 2) != 0 && (buildingData.size.x % 2) == 0)
        {
            rotatedOffsets = new Vector3(rotatedOffsets.x * -1, rotatedOffsets.y, rotatedOffsets.z);
        }

        if (!buildingData.isRotatable) currentFacing = BuildingFacing.forward;

        if (CanPlaceStructure(placementCheckingGridPos, buildingData, currentFacing))
        {
            GridManager.Instance.PlaceBuilding(prefab, new Vector3(hit.point.x, 0, hit.point.z) - rotatedOffsets, rotatedOffsets, currentFacing); // - rotatedOffsets
        }
    }

    // Changes the currentFacing value when rotated
    private void Rotate(InputAction.CallbackContext ctx)
    {
        BuildingDataSO buildingData = BuildingManager.Instance.PickedBuilding;

        if (buildingData == null) return;
        if (!buildingData.isRotatable)
        {
            currentFacing = BuildingFacing.forward;
            return;
        }

        currentFacing = (BuildingFacing)((((int)currentFacing) + 1) % 4);

        Debug.Log(currentFacing);
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

    private bool CanPlaceStructure(Vector3 centreGridPosition, BuildingDataSO buildingData, BuildingFacing facing)
    {
        Vector3Int bottomLeftPosition = GetBottomLeftPosition(centreGridPosition, buildingData.size, facing);

        Vector2Int newSize = buildingData.size;

        // Reverses the size if the rotation is left or right
        if (((int)facing) % 2 != 0)
        {
            newSize = new Vector2Int(newSize.y, newSize.x);
        }

        for (int x = bottomLeftPosition.x; x < bottomLeftPosition.x + newSize.x; x++)
        {
            for (int z = bottomLeftPosition.z; z < bottomLeftPosition.z + newSize.y; z++)
            {
                RaycastHit hit = RaycastGridCell(new Vector3(x, raycastHeight, z));

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

    private Vector3Int GetBottomLeftPosition(Vector3 centreGridPosition, Vector2Int size, BuildingFacing facing)
    {
        Vector2Int newSize = size;

        // Reverses the size if the rotation is left or right
        if (((int)facing) % 2 != 0)
        {
            newSize = new Vector2Int(newSize.y, newSize.x);
        }

        Vector3Int bottomLeftPosition = new Vector3Int(
            GetBottomLeftNumber(Mathf.RoundToInt(centreGridPosition.x), newSize.x),
            Mathf.RoundToInt(centreGridPosition.y),
            GetBottomLeftNumber(Mathf.RoundToInt(centreGridPosition.z), newSize.y)
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
        Physics.Raycast(position, Vector3.down, out hit, raycastHeight + 2, ~tileCheckIgnoreMask);
        return hit;
    }
}
