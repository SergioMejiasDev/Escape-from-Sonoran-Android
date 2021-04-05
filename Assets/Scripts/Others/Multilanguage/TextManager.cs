using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Active class in each text of the game and that is in charge of modifying it according to the active language.
/// </summary>
public class TextManager : MonoBehaviour
{
    [SerializeField] MultilanguageText multilanguageText = null;

    private void OnEnable()
    {
        ChangeLanguage(MultilanguageManager.multilanguageManager.activeLanguage);
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