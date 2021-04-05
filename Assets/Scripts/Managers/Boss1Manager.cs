using System.Collections;
using UnityEngine;

/// <summary>
/// Class that controls the specific functions of the first boss scene.
/// </summary>
public class Boss1Manager : MonoBehaviour
{
    #region Variables
    public static Boss1Manager boss1Manager;
    
    [Header("Elevator")]
    [SerializeField] GameObject redButton = null;
    [SerializeField] GameObject greenButton = null;
    [SerializeField] Animator elevatorAnim;
    [SerializeField] AudioSource elevatorAudio;
    [SerializeField] AudioClip ring = null;
    [SerializeField] AudioClip doors = null;
    
    [Header("Scene Transition")]
    public GameObject gunPanel;

    [Header("Batteries Spawner")]
    [SerializeField] GameObject battery = null;
    [SerializeField] Transform spawnPoint1 = null;
    [SerializeField] Transform spawnPoint2 = null;

    [Header("Characters")]
    [SerializeField] GameObject player = null;
    [SerializeField] Player playerClass = null;
    [SerializeField] GameObject enemy = null;
    [SerializeField] EnemyBig enemyClass = null;
    #endregion

    void Start()
    {
        boss1Manager = this;
        GameManager.gameManager.InitialFade();
        GameManager.gameManager.StartDialogue(0);
    }

    /// <summary>
    /// We call this function to start the boss battle.
    /// </summary>
    public void StartBattle()
    {
        GameManager.gameManager.CloseDialogue();
        GameManager.gameManager.ActivateMusic();
        playerClass.enabled = true;
        enemyClass.enabled = true;
        StartCoroutine(SpawnBattery());
    }

    /// <summary>
    /// We call this function when the enemy dies.
    /// </summary>
    public void BossDeath()
    {
        StartCoroutine(OpenElevator());
    }

    /// <summary>
    /// Coroutine that activates the elevator animation and starts the transition of scenes.
    /// </summary>
    /// <returns></returns>
    IEnumerator OpenElevator()
    {
        GameManager.gameManager.DeactivateMusic();
        yield return new WaitForSeconds(2);
        GameManager.gameManager.ChangeCursor(false);
        elevatorAudio.clip = ring;
        elevatorAudio.Play();
        redButton.SetActive(false);
        greenButton.SetActive(true);
        yield return new WaitForSeconds(1);
        elevatorAudio.clip = doors;
        elevatorAudio.Play();
        yield return new WaitForSeconds(1);
        elevatorAnim.SetTrigger("BossDie");
        yield return new WaitForSeconds(2);
        GameManager.gameManager.FinalFade();
        yield return new WaitForSeconds(2);
        player.SetActive(false);
        yield return new WaitForSeconds(3);
        GameManager.gameManager.ChangeCursor(false);
        gunPanel.SetActive(true);
    }

    /// <summary>
    /// Coroutine that randomly generates batteries.
    /// </summary>
    /// <returns></returns>
    IEnumerator SpawnBattery()
    {
        while (true)
        {
            yield return new WaitForSeconds(25);

            float randomNumber = Random.value;
            Transform batterySpawnPoint;

            if (randomNumber < 0.5)
            {
                batterySpawnPoint = spawnPoint1;
            }
            else
            {
                batterySpawnPoint = spawnPoint2;
            }

            if (enemy != null)
            {
                Instantiate(battery, batterySpawnPoint.position, batterySpawnPoint.rotation);
            }
            else
            {
                yield break;
            }
        }
    }
}