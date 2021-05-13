using System;

/// <summary>
/// Class with the possible options variables that can be saved.
/// </summary>
[Serializable]
public class SettingsData
{
    public string activeLanguage;
    public bool firstTime;
    public float musicVolume;
    public float sfxVolume;
}