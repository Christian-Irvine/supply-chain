using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// The tick manager is used for moving parts in the game like belts. It calls the relevant tick events in a specific order so things can occur in that order.
/// For example belts should push items before trying to pull them so the push event is called first.
/// It also allows for things to happen on odd ticks, like redstone in minecraft if necessary.
/// The FixedUpdate runs at 60 ticks per second in this project (or is close as it can be running every 0.0167 seconds). (Default is 50 or every 0.02 seconds)
/// </summary>
public class TickManager : MonoBehaviour
{
    public static TickManager Instance;

    public UnityEvent BeltPush;
    public UnityEvent BeltPull;

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        Instance = this;
    }

    private void FixedUpdate()
    {
        BeltPush?.Invoke();
        BeltPull?.Invoke();
    }
}
