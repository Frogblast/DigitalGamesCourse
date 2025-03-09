using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public struct SettingsInput
{
    public bool OpenSettings;
}

public class SettingsManager : MonoBehaviour
{
    public GameObject settingsPanel;
    public Slider sensitivitySlider;
    private float defaultSensitivity = 0.08f;
    public PlayerCamera cameraController;

    private bool isSettingsOpen = false;

    void Start()
    {
        sensitivitySlider.value = PlayerPrefs.GetFloat("Sensitivity", defaultSensitivity); // load the sens settings

        cameraController.sensitivity = sensitivitySlider.value; // send saved sens to mouse at start

        sensitivitySlider.onValueChanged.AddListener(UpdateSensitivity); // listener if the slider changes update the sens and send to camera
    }

    public void UpdateUI(SettingsInput input)
    {
        
        if (input.OpenSettings == true)
        {
            Debug.Log("ESC pressed");
            ToggleSettingsMenu();
        }

    }

    void UpdateSensitivity(float value)
    {
        PlayerPrefs.SetFloat("Sensitivity", value); // save the latest sens set
        PlayerPrefs.Save();

        cameraController.sensitivity = value; // send to camera
        Debug.Log("Sensitivity set to: " + value);
    }

    void ToggleSettingsMenu()
    {
        isSettingsOpen = !isSettingsOpen;
        if (isSettingsOpen) 
        {
            Debug.Log("Open settings");
            Time.timeScale = 0f; // freeze game / does not freeze camera though
        } else 
        {
            Debug.Log("Close settings");
            Time.timeScale = 1f; // unfreeze game
        }

        EventManager.TriggerSettingMenu();

        // Lock or unlock cursor when menu is open
        if (isSettingsOpen)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        } 
        else
        {
            Debug.Log("lock cursor");
            Cursor.lockState= CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

}
