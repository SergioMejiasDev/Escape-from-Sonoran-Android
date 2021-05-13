using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

/// <summary>
/// Class that manages saving and loading of scores in a binary file.
/// </summary>
public class SaveManager : MonoBehaviour
{
    public static SaveManager saveManager;

    [Header("Options")]
    public string activeLanguage = "EN";
    public bool firstTime = false;
    public float musicVolume = 1;
    public float sfxVolume = 1;

    private void Awake()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("SaveManager");

        if (objs.Length > 1)
        {
            Destroy(gameObject);
        }

        else
        {
            saveManager = this;

            DontDestroyOnLoad(gameObject);

            LoadOptions();

            Screen.sleepTimeout = SleepTimeout.NeverSleep;
        }
    }

    /// <summary>
    /// Function to save options values.
    /// </summary>
    public void LoadOptions()
    {
        SettingsData data = new SettingsData();

        string path = Application.persistentDataPath + "/Settings.sav";

        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            data = formatter.Deserialize(stream) as SettingsData;
            stream.Close();

            activeLanguage = data.activeLanguage;
            firstTime = data.firstTime;
            musicVolume = data.musicVolume;
            sfxVolume = data.sfxVolume;
        }
    }

    /// <summary>
    /// Function to load options values.
    /// </summary>
    public void SaveOptions()
    {
        SettingsData data = new SettingsData
        {
            activeLanguage = activeLanguage,
            firstTime = firstTime,
            musicVolume = musicVolume,
            sfxVolume = sfxVolume,
        };

        BinaryFormatter formatter = new BinaryFormatter();

        string path = Application.persistentDataPath + "/Settings.sav";

        FileStream fileStream = new FileStream(path, FileMode.Create);

        formatter.Serialize(fileStream, data);

        fileStream.Close();
    }
}