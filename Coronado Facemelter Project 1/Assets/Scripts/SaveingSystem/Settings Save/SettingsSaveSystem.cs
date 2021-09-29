using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SettingsSaveSystem
{
    public static void SaveSettings(SettingsMenu settings)
    {
        BinaryFormatter formatter = new BinaryFormatter();

        /*Application.persistantDataPath is a function that is built into unity that 
         * uses any file path for any operating system.
         */
        string path = Application.persistentDataPath + "/GameSettings.harambe";
        FileStream stream = new FileStream(path, FileMode.Create);

        SettingData data = new SettingData(settings);

        formatter.Serialize(stream, data);
        stream.Close();

        Debug.Log("Settings have been saved!");
    }

    public static SettingData LoadData()
    {
        string path = Application.persistentDataPath + "/GameSettings.harambe";

        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            SettingData data = formatter.Deserialize(stream) as SettingData;
            stream.Close();

            Debug.Log("Loading settings");

            return data;
        }else if (!File.Exists(path))
        {
            File.Create(path).Dispose();
            Debug.LogError("Save file not found in " + path);
            return null;
        }
        else
        {
            Debug.LogError("Unknow Error");
            return null;
        }
    }
}
