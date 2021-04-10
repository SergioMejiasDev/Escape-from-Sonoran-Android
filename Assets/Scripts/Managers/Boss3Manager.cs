using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class that controls the specific functions of the third boss scene.
/// </summary>
public class Boss3Manager : MonoBehaviour
{
    #region Variables
    public static Boss3Manager boss3Manager;

    [Header("Explosion")]
    [SerializeField] GameObject whitePanel = null;
    [SerializeField] Image whiteImage = null;
    float fadeSpeed = 0.6f;
    [SerializeField] AudioSource audioSource = null;

    [Header("Sky Fading")]
    [SerializeField] GameObject sun = null;
    float sunSpeed = 0.25f;
    [SerializeField] SpriteRenderer bgsr = null;
    float skyFadeSpeed = 0.01f;

    [Header("Battery Spawner")]
    [SerializeField] GameObject battery = null;

    [Header("Characters")]
    [SerializeField] GameObject player = null;
    [SerializeField] FlyingPlayer playerClass = null;
    [SerializeField] GameObject enemy = null;
    [SerializeField] FinalBoss enemyClass = null;

    [Header("Narrative")]
    [SerializeField] Text[] narrativeTexts = null;
    [SerializeField] float[] narrativeTimes = null;
    #endregion

    void Start()
    {
        boss3Manager = this;
        GameManager.gameManager.InitialFade();
        GameManager.gameManager.StartDialogue(0);
    }

    /// <summary>
    /// Function we call to start the battle.
    /// </summary>
    public void StartBattle()
    {
        GameManager.gameManager.CloseDialogue();
        GameManager.gameManager.ActivateMusic();
        StartCoroutine(Sun());
        StartCoroutine(SkyFade());
        playerClass.enabled = true;
        enemyClass.enabled = true;
        StartCoroutine(SpawnBattery());
    }

    /// <summary>
    /// Function called when an explosion occurs.
    /// </summary>
    /// <param name="killPlayer">It will be true if the player dies in the explosion, false if the enemy dies.</param>
    public void StartExplosion(bool killPlayer)
    {
        GameManager.gameManager.ChangeCursor(false);
        StartCoroutine(Explosion(killPlayer));
    }

    /// <summary>
    /// Coroutine that initiates the white explosion.
    /// </summary>
    /// <param name="killPlayer">It will be true if the player dies in the explosion, false if the enemy dies.</param>
    /// <returns></returns>
    IEnumerator Explosion(bool killPlayer)
    {
        if (killPlayer)
        {
            player.SetActive(false);
        }

        whitePanel.SetActive(true);
        Color imageColor = whiteImage.color;
        float alphaValue;

        while (whiteImage.color.a < 1)
        {
            alphaValue = imageColor.a + (fadeSpeed * Time.deltaTime);
            imageColor = new Color(imageColor.r, imageColor.g, imageColor.b, alphaValue);
            whiteImage.color = new Color(imageColor.r, imageColor.g, imageColor.b, alphaValue);
            yield return null;
        }
        
        if (!killPlayer)
        {
            Destroy(enemy);
            audioSource.Play();
        }

        yield return new WaitForSeconds(4);

        while (whiteImage.color.a > 0)
        {
            alphaValue = imageColor.a - (fadeSpeed * Time.deltaTime);
            imageColor = new Color(imageColor.r, imageColor.g, imageColor.b, alphaValue);
            whiteImage.color = new Color(imageColor.r, imageColor.g, imageColor.b, alphaValue);
            yield return null;
        }

        if (killPlayer)
        {
            GameManager.gameManager.GameOver();
        }

        else
        {
            StartCoroutine(EndGame());
        }
    }

    /// <summary>
    /// Coroutine that is responsible for the movement of the sun.
    /// </summary>
    /// <returns></returns>
    IEnumerator Sun()
    {
        while (sun.transform.position.y > -12)
        {
            sun.transform.Translate(Vector2.down * sunSpeed * Time.deltaTime);
            yield return null;
        }

        Destroy(sun);
    }

    /// <summary>
    /// Coroutine that starts the transition between backgrounds.
    /// </summary>
    /// <returns></returns>
    IEnumerator SkyFade()
    {
        yield return new WaitForSeconds(20);

        Color imageColor = bgsr.color;
        float alphaValue;

        while (bgsr.color.a > 0)
        {
            alphaValue = imageColor.a - (skyFadeSpeed * Time.deltaTime);
            imageColor = new Color(imageColor.r, imageColor.g, imageColor.b, alphaValue);
            bgsr.color = new Color(imageColor.r, imageColor.g, imageColor.b, alphaValue);
            yield return null; 
        }
    }

    /// <summary>
    /// Coroutine that begins the last narrative and begins the scene transition.
    /// </summary>
    /// <returns></returns>
    IEnumerator EndGame()
    {
        GameManager.gameManager.DeactivateMusic();
        yield return new WaitForSeconds(3);
        GameManager.gameManager.ChangeCursor(false);
        GameManager.gameManager.FinalFade();
        yield return new WaitForSeconds(2);
        GameManager.gameManager.ChangeCursor(false);
        Cursor.visible = false;
        player.SetActive(false);
        yield return new WaitForSeconds(4);
        
        for (int i = 0; i < narrativeTexts.Length; i++)
        {
            Color imageColor = narrativeTexts[i].color;
            float alphaValue;

            while (narrativeTexts[i].color.a < 1)
            {
                alphaValue = imageColor.a + (0.5f * Time.deltaTime);
                imageColor = new Color(imageColor.r, imageColor.g, imageColor.b, alphaValue);
                narrativeTexts[i].color = new Color(imageColor.r, imageColor.g, imageColor.b, alphaValue);
                yield return null;
            }

            yield return new WaitForSeconds(narrativeTimes[i]);

            while (narrativeTexts[i].color.a > 0)
            {
                alphaValue = imageColor.a - (0.5f * Time.deltaTime);
                imageColor = new Color(imageColor.r, imageColor.g, imageColor.b, alphaValue);
                narrativeTexts[i].color = new Color(imageColor.r, imageColor.g, imageColor.b, alphaValue);
                yield return null;
            }
        }

        GameManager.gameManager.LoadScene(7);
    }

    /// <summary>
    /// Coroutine that generates batteries randomly.
    /// </summary>
    /// <returns></returns>
    IEnumerator SpawnBattery()
    {
        while (true)
        {
            float waitForBatteries = Random.Range(40, 50);
            
            yield return new WaitForSeconds(waitForBatteries);

            Vector2 batterySpawnPoint = new Vector2(Random.Range(-17, 2), Random.Range(-9, 9));

            if (enemy != null)
            {
                Instantiate(battery, batterySpawnPoint, Quaternion.identity) ;
            }

            else
            {
                yield break;
            }
        }
    }
}