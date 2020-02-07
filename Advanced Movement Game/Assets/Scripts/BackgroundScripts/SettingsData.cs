using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SettingsData
{
    // Setting variables for PlayerCamera
    [HideInInspector] public float mouseSensitivity;

    public SettingsData(OptionsSave optionsSave)
    {
        // PlayerCamera
        mouseSensitivity = optionsSave.mouseSensitivity;
    }
}
