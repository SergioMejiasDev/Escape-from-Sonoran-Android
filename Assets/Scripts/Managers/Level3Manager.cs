using System.Collections;
using UnityEngine;

/// <summary>
/// Class that controls the specific functions of the third level.
/// </summary>
public class Level3Manager : MonoBehaviour
{
    public static Level3Manager level3Manager;

    [Header("Scene Transition")]
    [SerializeField] GameObject fadeBackgroundPanel = null;
    [SerializeField] SpriteRenderer fadeBackgroundImage;
    float fadeBGSpeed = 0.5f;

    void Start()
    {
        level3Manager = this;
        GameManager.gameManager.ChangeCursor(true);
        GameManager.gameManager.InitialFade();
        GameManager.gameManager.TextFading();
    }

    /// <summary>
    /// Function that is called when the level end trigger is reached.
    /// </summary>
    public void StartFade()
    {
        StartCoroutine(BackgroundFade());
    }

    /// <summary>
    /// Coroutine that makes the background appear and starts the scene transition.
    /// </summary>
    /// <returns></returns>
    IEnumerator BackgroundFade()
    {
        yield return new WaitForSeconds(2);
        GameManager.gameManager.DeactivateMusic();

        Color imageBGColor = fadeBackgroundImage.color;
        float alphaBGValue;

        while (fadeBackgroundImage.color.a > 0)
        {
            alphaBGValue = imageBGColor.a - (fadeBGSpeed * Time.deltaTime);
            imageBGColor = new Color(imageBGColor.r, imageBGColor.g, imageBGColor.b, alphaBGValue);
            fadeBackgroundImage.color = new Color(imageBGColor.r, imageBGColor.g, imageBGColor.b, alphaBGValue);
            yield return null;
        }

        fadeBackgroundPanel.SetActive(false);

        GameManager.gameManager.FinalFade();
        GameManager.gameManager.ChangeCursor(false);
        yield return new WaitForSeconds(2);
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        player.SetActive(false);
        yield return new WaitForSeconds(2);
        GameManager.gameManager.LoadScene(6);
    }
}