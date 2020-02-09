using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SettingsData
{
    // Setting variables for PlayerCamera
    [HideInInspector] public float mouseSensitivity;

    // Varialbles for level complete
    [HideInInspector] public string level1CompleteTime;
    [HideInInspector] public float realLevel1CompleteTime;

    public SettingsData(OptionsSave optionsSave)
    {
        // PlayerCamera
        mouseSensitivity = optionsSave.mouseSensitivity;

        // LevelComplete Time
        level1CompleteTime = optionsSave.level1CompleteTime;
        realLevel1CompleteTime = optionsSave.realLevel1CompleteTime;
    }
}
