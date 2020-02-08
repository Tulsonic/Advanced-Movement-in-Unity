using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SettingsData
{
    // Setting variables for PlayerCamera
    [HideInInspector] public float mouseSensitivity;
    [HideInInspector] public string level1CompleteTime;

    public SettingsData(OptionsSave optionsSave)
    {
        // PlayerCamera
        mouseSensitivity = optionsSave.mouseSensitivity;
        level1CompleteTime = optionsSave.level1CompleteTime;
    }
}
