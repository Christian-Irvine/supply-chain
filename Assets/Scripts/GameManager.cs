using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [SerializeField] private int money;
    public int Money { get => money; set
        {
            money = value;
            MoneyChange?.Invoke();
        }
    }

    public UnityEvent MoneyChange;

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);

        Instance = this;
    }

    private IEnumerator Start()
    {
        yield return null;

        Money = Money; // Calling the money change invoke
    }
}
