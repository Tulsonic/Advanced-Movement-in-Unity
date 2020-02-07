using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    public static void SaveData(OptionsSave optionsSave)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/player.AMGD";
        FileStream fileStream = new FileStream(path, FileMode.Create);

        SettingsData settingsData = new SettingsData(optionsSave);

        formatter.Serialize(fileStream, settingsData);
    }

    public static SettingsData LoadData ()
    {
        string path = Application.persistentDataPath + "/player.AMGD";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream fileStream = new FileStream(path, FileMode.Open);

            SettingsData data = formatter.Deserialize(fileStream) as SettingsData;
            fileStream.Close();

            return data;
        } 
        else
        {
            Debug.LogError("Save file not found in " + path + ":(");
            return null;
        }
    }
}
