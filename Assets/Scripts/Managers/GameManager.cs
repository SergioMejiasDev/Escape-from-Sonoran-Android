using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// Class from which most of the main functions of the game are managed.
/// </summary>
public class GameManager : MonoBehaviour
{
    #region Variables
    public static GameManager gameManager;

    [Header("Menu Panels")]
    [SerializeField] GameObject[] panels = null;

    [Header ("Pause")]
    [SerializeField] GameObject panelPause = null;
    [SerializeField] GameObject panelControllers = null;

    [Header("Game Over")]
    [SerializeField] GameObject player = null;
    Vector3 respawnPosition;
    [SerializeField] GameObject finalTrigger = null;
    [SerializeField] GameObject gameOver = null;
    Text gameOverText;
    [SerializeField] GameObject gameOverPanel = null;

    [Header("Narrative Texts")]
    [SerializeField] Text[] narrativeTexts = null;
    [SerializeField] float[] narrativeTimes = null;
    [SerializeField] Image logo = null;
    [SerializeField] Text skipText = null;
    bool canSkip = false;

    [Header("Dialogues")]
    [SerializeField] GameObject dialoguePanel = null;
    [SerializeField] GameObject[] dialogues = null;

    [Header("Scene Fading")]
    [SerializeField] GameObject fadePanel = null;
    Image fadeImage;
    [SerializeField] GameObject[] activableObjects = null;
    [SerializeField] GameObject levelTextPanel = null;
    Text levelText;

    [Header("Music")]
    [SerializeField] AudioSource ambientMusic = null;
    [SerializeField] AudioClip transitionMusic = null;
    [SerializeField] AudioClip levelMusic = null;
    #endregion

    void Awake()
    {
        Time.timeScale = 1;
        gameManager = this;

        ScaleScreen();

        if (gameOver != null)
        {
            gameOverText = gameOver.GetComponent<Text>();
        }

        if (fadePanel != null)
        {
            fadeImage = fadePanel.GetComponent<Image>();
            fadePanel.SetActive(true);
        }

        if (levelTextPanel != null)
        {
            levelText = levelTextPanel.GetComponent<Text>();
        }

        if (player != null)
        {
            respawnPosition = player.transform.position;
        }
    }

    /// <summary>
    /// Function called when the player cancels the narrative texts.
    /// </summary>
    public void CancelButton()
    {
        if (canSkip)
        {
            StopCoroutine(Narrative());

            StopCoroutine(SkipNarrative());

            for (int i = 0; i < narrativeTexts.Length; i++)
            {
                narrativeTexts[i].enabled = false;
            }

            ambientMusic.Stop();
            StartLevel();
        }
    }

    /// <summary>
    /// Function that we call from the main menu to open (or close) panels.
    /// </summary>
    /// <param name="panelToOpen">It has a numerical value depending on the panel you want to open. The others will close.</param>
    public void OpenPanel(GameObject panelToOpen)
    {
        for (int i = 0; i < panels.Length; i++)
        {
            panels[i].SetActive(false);
        }

        panelToOpen.SetActive(true);
    }

    /// <summary>
    /// Function called to close the game.
    /// </summary>
    public void CloseGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    /// <summary>
    /// Function called to load a new scene.
    /// </summary>
    /// <param name="buildIndex">The scene we want to load.</param>
    public void LoadScene(int buildIndex)
    {
        Time.timeScale = 1;

        SceneManager.LoadScene(buildIndex);
    }

    /// <summary>
    /// Function that enable and disable controlles on screen.
    /// </summary>
    /// <param name="customCursor">True if we want to enable, false if we want to disable.</param>
    public void ChangeCursor(bool customCursor)
    {
        if (panelControllers == null)
        {
            return;
        }

        if (customCursor)
        {
            panelControllers.SetActive(true);
        }

        else
        {
            panelControllers.SetActive(false);
        }
    }

    /// <summary>
    /// Function that is responsible for pausing the game.
    /// </summary>
    public void PauseGame()
    {
        Time.timeScale = 0;
        panelPause.SetActive(true);
        ChangeCursor(false);
    }

    /// <summary>
    /// Function that is responsible for resuming the game.
    /// </summary>
    public void ResumeGame()
    {
        Time.timeScale = 1;
        panelPause.SetActive(false);
        ChangeCursor(true);
    }

    /// <summary>
    /// Function that is called to start the narrative scenes.
    /// </summary>
    public void StartNarrative()
    {
        Cursor.visible = false;
        StartCoroutine(Narrative());
    }

    /// <summary>
    /// Function from which we activate the dialog panel.
    /// </summary>
    /// <param name="dialogue">Dialogue that will open when we call the function. It corresponds to the value of the dialog array.</param>
    public void StartDialogue(int dialogue)
    {
        dialoguePanel.SetActive(true);

        ChangeCursor(false);
        
        for (int i = 0; i < dialogues.Length; i++)
        {
            dialogues[i].SetActive(false);
        }

        dialogues[dialogue].SetActive(true);
    }

    /// <summary>
    /// Function we call to completely close the dialog panel.
    /// </summary>
    public void CloseDialogue()
    {
        dialoguePanel.SetActive(false);
        ChangeCursor(true);
    }

    /// <summary>
    /// Function from which we will call the coroutines and functions that prepare the beginning of the level.
    /// </summary>
    public void StartLevel()
    {
        canSkip = false;
        Cursor.visible = true;
        ChangeCursor(true);
        ActivateMusic();
        InitialFade();
        TextFading();
    }

    /// <summary>
    /// Function that calls the Game Over coroutine.
    /// </summary>
    public void GameOver()
    {
        panelControllers.SetActive(false);

        StartCoroutine(FadeGameOver());
    }

    /// <summary>
    /// Function we call to respawn the player at the checkpoint.
    /// </summary>
    public void QuitGameOver()
    {
        StartCoroutine(ContinueGame());
    }

    /// <summary>
    /// Function called when a CkeckPoint is reached.
    /// </summary>
    /// <param name="checkPointPosition">Position where the CheckPoint is located.</param>
    public void CheckPoint(Vector3 checkPointPosition)
    {
        respawnPosition = checkPointPosition;
    }

    /// <summary>
    /// Function that activates the music of the level.
    /// </summary>
    public void ActivateMusic()
    {
        ambientMusic.clip = levelMusic;
        ambientMusic.volume = 1;
        ambientMusic.Play();
    }

    /// <summary>
    /// Function that deactivates the music at the end of the level.
    /// </summary>
    public void DeactivateMusic()
    {
        StartCoroutine(FadeOutMusic());
    }

    /// <summary>
    /// Function that activates the initial transition of the scene.
    /// </summary>
    public void InitialFade()
    {
        StartCoroutine(FadeIn(2, 0));

        if (activableObjects != null)
        {
            for (int i = 0; i < activableObjects.Length; i++)
            {
                activableObjects[i].SetActive(true);
            }
        }
    }

    /// <summary>
    /// Function that activates the final transition of the scene.
    /// </summary>
    public void FinalFade()
    {
        StartCoroutine(FadeOut(2, 1));
    }

    /// <summary>
    /// Function that activates the text with the name of the level.
    /// </summary>
    public void TextFading()
    {
        StartCoroutine(TextFade());
    }

    /// <summary>
    /// Coroutine that is responsible for the transition between narrative scenes.
    /// </summary>
    /// <returns></returns>
    IEnumerator Narrative()
    {
        StartCoroutine(SkipNarrative());
        Cursor.visible = false;
        ambientMusic.clip = transitionMusic;
        ambientMusic.volume = 1;
        ambientMusic.Play();


        for (int i = 0; i < narrativeTexts.Length; i++)
        {
            Color imageColor = narrativeTexts[i].color;
            float alphaValue;

            while (narrativeTexts[i].color.a < 1)
            {
                if (!canSkip)
                {
                    yield break;
                }

                alphaValue = imageColor.a + (0.5f * Time.deltaTime);
                imageColor = new Color(imageColor.r, imageColor.g, imageColor.b, alphaValue);
                narrativeTexts[i].color = new Color(imageColor.r, imageColor.g, imageColor.b, alphaValue);
                yield return null;
            }

            yield return new WaitForSeconds(narrativeTimes[i]);

            while (narrativeTexts[i].color.a > 0)
            {
                if (!canSkip)
                {
                    yield break;
                }

                alphaValue = imageColor.a - (0.5f * Time.deltaTime);
                imageColor = new Color(imageColor.r, imageColor.g, imageColor.b, alphaValue);
                narrativeTexts[i].color = new Color(imageColor.r, imageColor.g, imageColor.b, alphaValue);
                yield return null;
            }
        }

        if (logo != null)
        {
            yield return new WaitForSeconds(1);
            Color imageColor = logo.color;
            float alphaValue;

            while (logo.color.a < 1)
            {
                if (!canSkip)
                {
                    yield break;
                }

                alphaValue = imageColor.a + (0.5f * Time.deltaTime);
                imageColor = new Color(imageColor.r, imageColor.g, imageColor.b, alphaValue);
                logo.color = new Color(imageColor.r, imageColor.g, imageColor.b, alphaValue);
                yield return null;
            }

            yield return new WaitForSeconds(3);

            while (logo.color.a > 0)
            {
                if (!canSkip)
                {
                    yield break;
                }

                alphaValue = imageColor.a - (0.5f * Time.deltaTime);
                imageColor = new Color(imageColor.r, imageColor.g, imageColor.b, alphaValue);
                logo.color = new Color(imageColor.r, imageColor.g, imageColor.b, alphaValue);
                yield return null;
            }
        }

        while (ambientMusic.volume > 0.01f)
        {
            if (!canSkip)
            {
                yield break;
            }

            ambientMusic.volume -= Time.deltaTime / 2;
            yield return null;
        }

        yield return new WaitForSeconds(2);

        if (!canSkip)
        {
            yield break;
        }

        StartLevel();
    }

    /// <summary>
    /// Coroutine where the Game Over message and the corresponding panel appear.
    /// </summary>
    /// <returns></returns>
    IEnumerator FadeGameOver()
    {
        gameOver.SetActive(true);
        
        StartCoroutine(FadeOut(1, 2));
        Cursor.visible = false;

        Color gameOverColor = gameOverText.color;
        float alphaValue;

        while (gameOverText.color.a < 1)
        {
            alphaValue = gameOverColor.a + (1 * Time.deltaTime);
            gameOverColor = new Color(gameOverColor.r, gameOverColor.g, gameOverColor.b, alphaValue);
            gameOverText.color = new Color(gameOverColor.r, gameOverColor.g, gameOverColor.b, alphaValue);
            yield return null;
        }

        yield return new WaitForSeconds(4);
        gameOverPanel.SetActive(true);
        ChangeCursor(false);
        Cursor.visible = true;

        if (finalTrigger != null)
        {
            if (!finalTrigger.activeSelf)
            {
                GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
                for (int i = 0; i < enemies.Length; i++)
                {
                    Destroy(enemies[i]);
                }
            }
        }

        Time.timeScale = 0;
    }

    /// <summary>
    /// Coroutine that allows us to continue the game after a Game Over.
    /// </summary>
    /// <returns></returns>
    IEnumerator ContinueGame()
    {
        Time.timeScale = 1;
        ChangeCursor(true);
        GameObject[] bullets = GameObject.FindGameObjectsWithTag("BulletEnemy");

        for (int i = 0; i < bullets.Length; i++)
        {
            bullets[i].SetActive(false);
        }

        gameOverPanel.SetActive(false);
        Color gameOverColor = gameOverText.color;
        float alphaValue;

        while (gameOverText.color.a > 0)
        {
            alphaValue = gameOverColor.a - (3 * Time.deltaTime);
            gameOverColor = new Color(gameOverColor.r, gameOverColor.g, gameOverColor.b, alphaValue);
            gameOverText.color = new Color(gameOverColor.r, gameOverColor.g, gameOverColor.b, alphaValue);
            yield return null;
        }

        gameOver.SetActive(false);
        StartCoroutine(FadeIn(2, 0));
        player.transform.position = respawnPosition;
        Camera.main.GetComponent<CameraMovement>().enabled = true;
        yield return new WaitForSeconds(2);
        player.GetComponent<PlayerHealth>().RestorePlayer();

        if (finalTrigger != null)
        {
            finalTrigger.SetActive(true);
        }
    }

    /// <summary>
    /// Coroutine of the initial transition.
    /// </summary>
    /// <param name="speed">Speed at which the transition occurs.</param>
    /// <param name="waitTime">Waiting time that we put at the beginning.</param>
    /// <returns></returns>
    public IEnumerator FadeIn(float speed, float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        Color imageColor = fadeImage.color;
        float alphaValue;

        while (fadeImage.color.a > 0)
        {
            alphaValue = imageColor.a - (speed * Time.deltaTime);
            imageColor = new Color(imageColor.r, imageColor.g, imageColor.b, alphaValue);
            fadeImage.color = new Color(imageColor.r, imageColor.g, imageColor.b, alphaValue);
            yield return null;
        }

        fadePanel.SetActive(false);
    }

    /// <summary>
    /// Coroutine of the final transition.
    /// </summary>
    /// <param name="speed">Speed at which the transition occurs.</param>
    /// <param name="waitTime">Waiting time that we put at the beginning.</param>
    /// <returns></returns>
    public IEnumerator FadeOut(float speed, float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        fadePanel.SetActive(true);
        
        Color imageColor = fadeImage.color;
        float alphaValue;

        while (fadeImage.color.a < 1)
        {
            alphaValue = imageColor.a + (speed * Time.deltaTime);
            imageColor = new Color(imageColor.r, imageColor.g, imageColor.b, alphaValue);
            fadeImage.color = new Color(imageColor.r, imageColor.g, imageColor.b, alphaValue);
            yield return null;
        }
    }

    /// <summary>
    /// Coroutine that is in charge of activating and deactivating the text with the name of the level.
    /// </summary>
    /// <returns></returns>
    IEnumerator TextFade()
    {
        levelTextPanel.SetActive(true);

        Color levelColor = levelText.color;
        float alphaValue;

        while (levelText.color.a < 1)
        {
            alphaValue = levelColor.a + (0.5f * Time.deltaTime);
            levelColor = new Color(levelColor.r, levelColor.g, levelColor.b, alphaValue);
            levelText.color = new Color(levelColor.r, levelColor.g, levelColor.b, alphaValue);
            yield return null;
        }

        yield return new WaitForSeconds(3);

        while (levelText.color.a > 0)
        {
            alphaValue = levelColor.a - (0.5f * Time.deltaTime);
            levelColor = new Color(levelColor.r, levelColor.g, levelColor.b, alphaValue);
            levelText.color = new Color(levelColor.r, levelColor.g, levelColor.b, alphaValue);
            yield return null;
        }

        levelTextPanel.SetActive(false);
    }

    /// <summary>
    /// Coroutine that deactivates the music at the end of the level.
    /// </summary>
    /// <returns></returns>
    IEnumerator FadeOutMusic()
    {
        while (ambientMusic.volume > 0.01f)
        {
            ambientMusic.volume -= Time.deltaTime / 2;
            yield return null;
        }
    }

    /// <summary>
    /// Coroutine that shows the text where it is indicated that we can skip the narrative.
    /// </summary>
    /// <returns></returns>
    IEnumerator SkipNarrative()
    {
        canSkip = true;

        Color Color = skipText.color;
        float alphaValue;

        while (skipText.color.a < 1)
        {
            alphaValue = Color.a + (1f * Time.deltaTime);
            Color = new Color(Color.r, Color.g, Color.b, alphaValue);
            skipText.color = new Color(Color.r, Color.g, Color.b, alphaValue);
            yield return null;
        }

        yield return new WaitForSeconds(1);

        while (skipText.color.a > 0)
        {
            alphaValue = Color.a - (1f * Time.deltaTime);
            Color = new Color(Color.r, Color.g, Color.b, alphaValue);
            skipText.color = new Color(Color.r, Color.g, Color.b, alphaValue);
            yield return null;
        }
    }

    /// <summary>
    /// Function called to scale the screen to a 1480:720 format.
    /// </summary>
    void ScaleScreen()
    {
        float targetAspect = 1480.0f / 720.0f;
        float windowAspect = (float)Screen.width / (float)Screen.height;
        float scaleheight = windowAspect / targetAspect;
        Camera camera = Camera.main;

        if (scaleheight < 1.0f)
        {
            Rect rect = camera.rect;

            rect.width = 1.0f;
            rect.height = scaleheight;
            rect.x = 0;
            rect.y = (1.0f - scaleheight) / 2.0f;

            camera.rect = rect;
        }

        else
        {
            float scalewidth = 1.0f / scaleheight;

            Rect rect = camera.rect;

            rect.width = scalewidth;
            rect.height = 1.0f;
            rect.x = (1.0f - scalewidth) / 2.0f;
            rect.y = 0;

            camera.rect = rect;
        }
    }
}