using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class that takes care of the credits scene.
/// </summary>
public class Credits : MonoBehaviour
{
    float speed = 50;
    [SerializeField] RectTransform creditsTransform = null;
    [SerializeField] Text thanksText = null;
    [SerializeField] GameObject cancelPanel = null;
    float finalPosition = 1500;

    void Start()
    {
        GameManager.gameManager.ChangeCursor(false);
        GameManager.gameManager.InitialFade();
        GameManager.gameManager.ActivateMusic();

        StartCoroutine(WaitForCancel());

        StartCoroutine(CreditsMovement());
    }

    public void CancelButton()
    {
        cancelPanel.SetActive(false);
        StartCoroutine(CloseCredits());
    }

    /// <summary>
    /// Coroutine that manages the movement of credits.
    /// </summary>
    /// <returns></returns>
    IEnumerator CreditsMovement()
    {
        while (creditsTransform.anchoredPosition.y < finalPosition)
        {
            creditsTransform.anchoredPosition += new Vector2(0, speed * Time.deltaTime);
            yield return null;
        }

        Color levelColor = thanksText.color;
        float alphaValue;

        while (thanksText.color.a < 1)
        {
            alphaValue = levelColor.a + (0.5f * Time.deltaTime);
            levelColor = new Color(levelColor.r, levelColor.g, levelColor.b, alphaValue);
            thanksText.color = new Color(levelColor.r, levelColor.g, levelColor.b, alphaValue);
            yield return null;
        }

        yield return new WaitForSeconds(3);

        while (thanksText.color.a > 0)
        {
            alphaValue = levelColor.a - (0.5f * Time.deltaTime);
            levelColor = new Color(levelColor.r, levelColor.g, levelColor.b, alphaValue);
            thanksText.color = new Color(levelColor.r, levelColor.g, levelColor.b, alphaValue);
            yield return null;
        }

        yield return new WaitForSeconds(2);

        StartCoroutine(CloseCredits());
    }

    /// <summary>
    /// Corroutine that closes the credits and returns to the menu.
    /// </summary>
    /// <returns></returns>
    IEnumerator CloseCredits()
    {
        GameManager.gameManager.FinalFade();
        yield return new WaitForSeconds(2);
        GameManager.gameManager.DeactivateMusic();
        yield return new WaitForSeconds(1);
        Cursor.visible = true;
        GameManager.gameManager.LoadScene(0);
    }

    IEnumerator WaitForCancel()
    {
        yield return new WaitForSeconds(4);

        cancelPanel.SetActive(true);
    }
}