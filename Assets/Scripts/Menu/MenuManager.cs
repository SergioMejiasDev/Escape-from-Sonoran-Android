using System.Collections;
using UnityEngine;

/// <summary>
/// Class that controls the main functions of the main menu.
/// </summary>
public class MenuManager : MonoBehaviour
{
    public static MenuManager manager;

    public delegate void LanguageDelegate(string language);
    public static event LanguageDelegate OnLanguageChange;

    [SerializeField] GameObject enemy = null;
    [SerializeField] Transform generationPoint = null;
    [SerializeField] GameObject panelLanguage = null;
    [SerializeField] GameObject panelMenu = null;

    private void Awake()
    {
        manager = this;

        if (SaveManager.saveManager.firstTime)
        {
            panelMenu.SetActive(true);
        }

        else
        {
            panelLanguage.SetActive(true);
        }
    }

    void Start()
    {
        GameManager.gameManager.InitialFade();

        StartCoroutine(Generate());
    }

    private void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            GameManager.gameManager.CloseGame();
        }
    }

    /// <summary>
    /// Function that is responsible for changing the language of all the texts in the game.
    /// </summary>
    /// <param name="language">The code of the language that we want to activate.</param>
    public void ChangeLanguage(string language)
    {
        MultilanguageManager.multilanguageManager.ChangeLanguage(language);

        OnLanguageChange(language);
    }

    /// <summary>
    /// Function that we call when we change the language from the options menu.
    /// </summary>
    public void NextLanguage()
    {
        switch (SaveManager.saveManager.activeLanguage)
        {
            case "EN":
                ChangeLanguage("ES");
                break;

            case "ES":
                ChangeLanguage("EN");
                break;
        }
    }

    /// <summary>
    /// Function that is called to generate enemies.
    /// </summary>
    void GenerateEnemy()
    {
        Destroy(Instantiate(enemy, generationPoint.position, Quaternion.identity), 12);
    }

    /// <summary>
    /// Coroutine that calls the function to generate enemies after a few seconds.
    /// </summary>
    /// <returns></returns>
    IEnumerator Generate()
    {
        while (true)
        {
            GenerateEnemy();
            yield return new WaitForSeconds(Random.Range(3, 15));
        }
    }
}
