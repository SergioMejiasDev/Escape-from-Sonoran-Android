using System.Collections;
using UnityEngine;

/// <summary>
/// Class that controls the specific functions of the second level.
/// </summary>
public class Level2Manager : MonoBehaviour
{
    #region Variables
    public static Level2Manager level2Manager;

    [Header("Spawn Zone")]
    [SerializeField] GameObject enemy1 = null;
    [SerializeField] GameObject enemy2 = null;
    [SerializeField] GameObject enemy3 = null;
    int remainingEnemies;
    [SerializeField] GameObject battery = null;
    [SerializeField] Transform batterySpawnZone = null;
    [SerializeField] Transform enemiesSpawnZone = null;
    [SerializeField] GameObject warning = null;

    [Header("Elevator")]
    [SerializeField] GameObject redButton = null;
    [SerializeField] GameObject greenButton = null;
    [SerializeField] Animator elevatorAnim = null;
    [SerializeField] AudioSource elevatorAudio = null;
    [SerializeField] AudioClip ring = null;
    [SerializeField] AudioClip doors = null;
    #endregion

    void Start()
    {
        level2Manager = this;

        GameManager.gameManager.ChangeCursor(true);
        GameManager.gameManager.InitialFade();
        GameManager.gameManager.TextFading();
    }

    /// <summary>
    /// Function that launches the coroutine where enemies are spawned at the end of the level.
    /// </summary>
    public void SpawnZone()
    {
        StartCoroutine(SpawnEnemies());
    }

    /// <summary>
    /// Coroutine that causes multiple enemies to appear at the end of the level.
    /// </summary>
    /// <returns></returns>
    IEnumerator SpawnEnemies()
    {
        Camera.main.GetComponent<CameraMovement>().enabled = false;
        GameObject[] aliveEnemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        for (int i = 0; i < aliveEnemies.Length; i++)
        {
            Destroy(aliveEnemies[i]);
        }

        enemy1.GetComponent<EnemyClass1>().direction = 1;
        enemy2.GetComponent<EnemyClass2>().direction = 1;
        enemy3.GetComponent<EnemyClass3>().direction = 1;
        remainingEnemies = 6;

        yield return new WaitForSeconds(1);
        Instantiate(battery, batterySpawnZone.position, batterySpawnZone.rotation);
        warning.SetActive(true);

        yield return new WaitForSeconds(1);
        Instantiate(enemy2, enemiesSpawnZone.position, enemiesSpawnZone.rotation);
        Instantiate(enemy3, enemiesSpawnZone.position, enemiesSpawnZone.rotation);
        remainingEnemies -= 2;

        yield return new WaitForSeconds(3);
        
        if (!player.activeSelf)
        {
            yield break;
        }

        Instantiate(enemy2, enemiesSpawnZone.position, enemiesSpawnZone.rotation);
        Instantiate(enemy3, enemiesSpawnZone.position, enemiesSpawnZone.rotation);
        remainingEnemies -= 2;
        warning.SetActive(false);

        yield return new WaitForSeconds(3);

        if (!player.activeSelf)
        {
            yield break;
        }

        Instantiate(enemy1, enemiesSpawnZone.position, enemiesSpawnZone.rotation);
        Instantiate(enemy3, enemiesSpawnZone.position, enemiesSpawnZone.rotation);
        remainingEnemies -= 2;

        StartCoroutine(VerifyEnemies());
    }

    /// <summary>
    /// Coroutine that checks if there are enemies left on the scene.
    /// </summary>
    /// <returns></returns>
    IEnumerator VerifyEnemies()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        while (true)
        {
            if (!player.activeSelf)
            {
                GameObject[] batteries = GameObject.FindGameObjectsWithTag("Battery");

                if (batteries != null)
                {
                    for (int i = 0; i < batteries.Length; i++)
                    {
                        Destroy(batteries[i]);
                    }
                }

                yield break;
            }

            GameObject[] aliveEnemies = GameObject.FindGameObjectsWithTag("Enemy");

            if (aliveEnemies.Length == 0 && remainingEnemies == 0)
            {
                StartCoroutine(OpenElevator(player));

                yield break;
            }

            yield return new WaitForSeconds(2);
        }
    }

    /// <summary>
    /// Coroutine that opens the elevator door and begins the transition to the next scene.
    /// </summary>
    /// <param name="player">The player.</param>
    /// <returns></returns>
    IEnumerator OpenElevator(GameObject player)
    {
        GameManager.gameManager.DeactivateMusic();
        yield return new WaitForSeconds(1);
        GameManager.gameManager.ChangeCursor(false);
        elevatorAudio.clip = ring;
        elevatorAudio.Play();
        redButton.SetActive(false);
        greenButton.SetActive(true);
        yield return new WaitForSeconds(1);
        elevatorAudio.clip = doors;
        elevatorAudio.Play();
        yield return new WaitForSeconds(1);

        if (!player.activeSelf)
        {
            yield break;
        }

        elevatorAnim.SetTrigger("BossDie");
        GameManager.gameManager.FinalFade();
        yield return new WaitForSeconds(2);
        player.SetActive(false);
        yield return new WaitForSeconds(4);
        GameManager.gameManager.LoadScene(4);
    }
}