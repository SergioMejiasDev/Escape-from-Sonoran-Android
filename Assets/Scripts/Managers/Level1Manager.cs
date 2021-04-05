using System.Collections;
using UnityEngine;

/// <summary>
/// Class that controls the specific functions of the first level.
/// </summary>
public class Level1Manager : MonoBehaviour
{
    #region Variables
    public static Level1Manager level1Manager;

    [Header("Spawn Zone")]
    [SerializeField] GameObject enemy1 = null;
    [SerializeField] GameObject enemy2 = null;
    [SerializeField] GameObject enemy3 = null;
    int remainingEnemies;
    [SerializeField] GameObject battery = null;
    [SerializeField] Transform batterySpawnZone = null;
    [SerializeField] Transform enemiesSpawnZone = null;
    [SerializeField] GameObject warning = null;

    [Header("Scene Transition")]
    [SerializeField] GameObject closedDoor = null;
    [SerializeField] GameObject openedDoor = null;
    [SerializeField] AudioSource doorSource = null;
    #endregion

    void Start()
    {
        level1Manager = this;

        GameManager.gameManager.StartNarrative();
    }

    /// <summary>
    /// Function we call when the player enters the final spawn zone.
    /// </summary>
    public void SpawnZone()
    {
        StartCoroutine(SpawnEnemies());
    }

    /// <summary>
    /// Coroutine that spawns multiple enemies before opening the final door.
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
        remainingEnemies = 5;

        yield return new WaitForSeconds(1);
        Instantiate(battery, batterySpawnZone.position, batterySpawnZone.rotation);
        warning.SetActive(true);

        yield return new WaitForSeconds(1);
        Instantiate(enemy2, enemiesSpawnZone.position, enemiesSpawnZone.rotation);
        remainingEnemies -= 1;

        yield return new WaitForSeconds(2);

        if (!player.activeSelf)
        {
            yield break;
        }

        Instantiate(enemy2, enemiesSpawnZone.position, enemiesSpawnZone.rotation);
        warning.SetActive(false);
        remainingEnemies -= 1;

        yield return new WaitForSeconds(2);
        Instantiate(battery, batterySpawnZone.position, batterySpawnZone.rotation);
        yield return new WaitForSeconds(2);

        if (!player.activeSelf)
        {
            yield break;
        }

        Instantiate(enemy1, enemiesSpawnZone.position, enemiesSpawnZone.rotation);
        remainingEnemies -= 1;
        yield return new WaitForSeconds(4);

        if (!player.activeSelf)
        {
            yield break;
        }

        Instantiate(enemy1, enemiesSpawnZone.position, enemiesSpawnZone.rotation);
        remainingEnemies -= 1;

        yield return new WaitForSeconds(1);
        Instantiate(battery, batterySpawnZone.position, batterySpawnZone.rotation);

        yield return new WaitForSeconds(6);

        if (!player.activeSelf)
        {
            yield break;
        }

        Instantiate(enemy3, enemiesSpawnZone.position, enemiesSpawnZone.rotation);
        remainingEnemies -= 1;

        StartCoroutine(VerifyEnemies());
    }

    /// <summary>
    /// Coroutine that checks if there is an enemy on the scene.
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
                StartCoroutine(OpenDoor(player));

                yield break;
            }

            yield return new WaitForSeconds(2);
        }
    }

    /// <summary>
    /// Coroutine that opens the final door and initiates the scene transition.
    /// </summary>
    /// <param name="player">The player.</param>
    /// <returns></returns>
    IEnumerator OpenDoor(GameObject player)
    {
        GameManager.gameManager.DeactivateMusic();

        yield return new WaitForSeconds(1);

        GameManager.gameManager.ChangeCursor(false);
        
        if (!player.activeSelf)
        {
            yield break;
        }

        closedDoor.SetActive(false);
        openedDoor.SetActive(true);
        doorSource.Play();
        
        GameManager.gameManager.FinalFade();

        yield return new WaitForSeconds(4);

        GameManager.gameManager.LoadScene(2);
    }
}