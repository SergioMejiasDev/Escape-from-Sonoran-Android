using System.Collections;
using UnityEngine;

/// <summary>
/// Class of the bouncing boxes on level 2.
/// </summary>
public class BoxTrap : MonoBehaviour
{
    [SerializeField] Transform respawn = null;
    GameObject explosion;
    [SerializeField] SpriteRenderer sr = null;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            sr.enabled = false;

            explosion = ObjectPooler.SharedInstance.GetPooledObject("Explosion");
            
            if (explosion != null)
            {
                explosion.SetActive(true);
                explosion.transform.position = collision.gameObject.transform.position;
                explosion.transform.rotation = collision.gameObject.transform.rotation;
            }

            StartCoroutine(Respawn(collision.gameObject));
            collision.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Coroutine where the player respawns after the collision.
    /// </summary>
    /// <param name="player">The player.</param>
    /// <returns></returns>
    IEnumerator Respawn(GameObject player)
    {
        yield return new WaitForSeconds(2);

        if (player != null)
        {
            player.SetActive(true);
            player.transform.position = respawn.position;
            player.transform.rotation = respawn.rotation;
            player.GetComponent<PlayerHealth>().Hurt(2);
            Destroy(gameObject);
        }
    }
}
