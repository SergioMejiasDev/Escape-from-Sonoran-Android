using System.Collections;
using UnityEngine;

/// <summary>
/// Class that is assigned to the Empty Objects in the water to produce an effect on the player (damage and respawn).
/// </summary>
public class Water : MonoBehaviour
{
    [SerializeField] Transform respawn = null;
    [SerializeField] AudioSource audioSource = null;
    [SerializeField] GameObject splatter = null;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            audioSource.Play();
            StartCoroutine(Respawn(collision.gameObject));
            Destroy(Instantiate(splatter, collision.gameObject.transform.position, collision.gameObject.transform.rotation), 0.5f);
        }
    }

    /// <summary>
    /// Coroutine where the player respawns.
    /// </summary>
    /// <param name="player">The player.</param>
    /// <returns></returns>
    IEnumerator Respawn(GameObject player)
    {
        player.SetActive(false);
        yield return new WaitForSeconds(2);
        if (player != null)
        {
            player.SetActive(true);
            player.transform.position = respawn.position;
            player.transform.rotation = respawn.rotation;
        }

        player.GetComponent<PlayerHealth>().Hurt(2);
    }
}
