using System;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static event Action OnPlayerDeath;
    public static event Action OnWinningGame;

    public static void TriggerPlayerDeath()
    {
        OnPlayerDeath?.Invoke();
    }

    public static void TriggerWinCondition()
    {
        OnWinningGame?.Invoke();
    }
}
