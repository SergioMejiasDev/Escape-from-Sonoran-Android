using UnityEngine;

/// <summary>
/// Class that is in charge of modifying the texts according to the active language.
/// </summary>
public class MultilanguageManager : MonoBehaviour
{
    public static MultilanguageManager multilanguageManager;

    public string activeLanguage;

    private void Awake()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("LanguageManager");

        if (objs.Length > 1)
        {
            Destroy(gameObject);
        }

        else
        {
            multilanguageManager = this;

            DontDestroyOnLoad(gameObject);
        }
    }

    void Start()
    {
        activeLanguage = SaveManager.saveManager.activeLanguage;
    }

    /// <summary>
    /// Function that is responsible for changing the language of all the texts in the game.
    /// </summary>
    /// <param name="newLanguage">The code of the language that we want to activate.</param>
    public void ChangeLanguage(string newLanguage)
    {
        activeLanguage = newLanguage;

        SaveManager.saveManager.activeLanguage = newLanguage;
        SaveManager.saveManager.firstTime = true;
        SaveManager.saveManager.SaveOptions();
    }
}
