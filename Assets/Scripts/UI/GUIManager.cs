using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIManager : MonoBehaviour
{
    public static GUIManager Instance;

    [SerializeField] private GameObject hud;
    [SerializeField] private GameObject buildMenu;
    [SerializeField] private GameObject storage;
    [SerializeField] private GameObject ingredientStore;


    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        Instance = this;
    }

    
}
