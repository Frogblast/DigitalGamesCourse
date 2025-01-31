using System;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static event Action OnPlayerDeath;

    public static void TriggerPlayerDeath()
    {
        OnPlayerDeath?.Invoke();
    }
}
