using System.Collections;
using UnityEngine;

/// <summary>
/// Class that controls the specific functions of the second boss scene.
/// </summary>
public class Boss2Manager : MonoBehaviour
{
    #region Variables
    public static Boss2Manager boss2Manager;

    [Header ("Battle Spawners")]
    [SerializeField] GameObject tnt = null;
    [SerializeField] GameObject battery = null;
    [SerializeField] Transform spawnPointLeft = null;
    [SerializeField] Transform spawnPointRight = null;
    [SerializeField] Transform batterySpawnPoint = null;

    [Header("Characters")]
    [SerializeField] GameObject player = null;
    [SerializeField] Player playerClass = null;
    [SerializeField] GameObject enemy = null;
    [SerializeField] Dog enemyClass = null;
    #endregion

    void Start()
    {
        boss2Manager = this;
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
        playerClass.enabled = true;
        enemyClass.enabled = true;
        RespawnTNT();
        StartCoroutine(SpawnBattery());
    }

    /// <summary>
    /// Function we call when the boss dies.
    /// </summary>
    public void BossDie()
    {
        StartCoroutine(EndBattle());
    }

    /// <summary>
    /// Function called when the TNT box explodes, causing another to appear.
    /// </summary>
    public void RespawnTNT()
    {
        if (Random.value < 0.5f)
        {
            Instantiate(tnt, spawnPointLeft.position, Quaternion.identity);
        }

        else
        {
            Instantiate(tnt, spawnPointRight.position, Quaternion.identity);
        }
    }

    /// <summary>
    /// Coroutine that causes a battery to appear in the center of the map from time to time.
    /// </summary>
    /// <returns></returns>
    IEnumerator SpawnBattery()
    {
        while (true)
        {
            yield return new WaitForSeconds(50);

            if (enemy != null)
            {
                Instantiate(battery, batterySpawnPoint.position, Quaternion.identity);
            }

            else
            {
                yield break;
            }
        }
    }

    /// <summary>
    /// Coroutine that starts when the boss dies and is responsible for making the transition to the next level.
    /// </summary>
    /// <returns></returns>
    public IEnumerator EndBattle()
    {
        GameManager.gameManager.DeactivateMusic();
        yield return new WaitForSeconds(3);
        GameManager.gameManager.ChangeCursor(false);
        GameManager.gameManager.FinalFade();
        yield return new WaitForSeconds(2);
        player.SetActive(false);
        yield return new WaitForSeconds(4);
        GameManager.gameManager.LoadScene(5);
    }
}
