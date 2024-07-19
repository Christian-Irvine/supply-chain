using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementGhost : MonoBehaviour
{
    public static PlacementGhost Instance;

    [SerializeField] private float ghostHeight;
    [SerializeField] private GameObject ghostModel;
    public GameObject GhostModel { get => ghostModel; set => ghostModel = value; }

    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private Material validMaterial;
    [SerializeField] private Material invalidMaterial;

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);

        Instance = this;
    }

    public void SetScale(Vector2Int scale)
    {
        GhostModel.transform.localScale = new Vector3(scale.x + 0.05f, ghostHeight, scale.y + 0.05f);
    }

    public void SetValidity(bool validity)
    {
        meshRenderer.material = validity ? validMaterial : invalidMaterial;
    }
}
