using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Alternative class to the Text Manager so that the texts change language just when a new language is entered.
/// </summary>
public class AdaptativeTextManager : MonoBehaviour
{
    [SerializeField] MultilanguageText multilanguageText = null;

    private void OnEnable()
    {
        MenuManager.OnLanguageChange += ChangeLanguage;

        ChangeLanguage(MultilanguageManager.multilanguageManager.activeLanguage);
    }

    private void OnDisable()
    {
        MenuManager.OnLanguageChange -= ChangeLanguage;
    }

    private void Start()
    {
        ChangeLanguage(MultilanguageManager.multilanguageManager.activeLanguage);
    }

    /// <summary>
    /// Function that modifies the text according to the active language.
    /// </summary>
    /// <param name="newLanguage">The code of the language that we want to activate.</param>
    void ChangeLanguage(string newLanguage)
    {
        Text text = GetComponent<Text>();

        switch (newLanguage)
        {
            case "EN":
                text.text = multilanguageText.english;
                break;
            case "ES":
                text.text = multilanguageText.spanish;
                break;
        }
    }
}
