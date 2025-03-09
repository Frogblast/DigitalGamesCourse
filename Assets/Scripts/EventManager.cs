using System;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static event Action OnPlayerDeath;
    public static event Action OnWinningGame;
    public static event Action OnOpenMenu;
    public static event Action<bool> OnAnyMenuOpened; // track the states of menues if any menu is opened at all to not cover the death and win screen with the settings.
    
    private static bool isAnyMenuOpen = false;

    public static void SetAnyMenuOpen(bool isOpen)
    {
        isAnyMenuOpen = isOpen;
        OnAnyMenuOpened?.Invoke(isAnyMenuOpen);
    }

    public static void TriggerPlayerDeath()
    {
        OnPlayerDeath?.Invoke();
    }

    public static void TriggerWinCondition()
    {
        OnWinningGame?.Invoke();
    }

    public static void TriggerSettingMenu()
    {
        OnOpenMenu?.Invoke();
    }

    public static bool IsAnyMenuOpen() => isAnyMenuOpen;
   
}
