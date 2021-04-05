using System.Collections;
using UnityEngine;

/// <summary>
/// Meteorites released by the final boss.
/// </summary>
public class Meteorite : MonoBehaviour
{
    float speed = 10;

    void OnEnable()
    {
        StartCoroutine(DestroyMeteorite());
    }

    void Update()
    {
        transform.Translate(Vector2.down * speed * Time.deltaTime);      
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerHealth>().Hurt(1);

            GameObject explosion = ObjectPooler.SharedInstance.GetPooledObject("Explosion");

            if (explosion != null)
            {
                explosion.SetActive(true);
                explosion.transform.position = transform.position;
                explosion.transform.rotation = transform.rotation;
            }

            gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Coroutine where the meteorite is destroyed after a few seconds.
    /// </summary>
    /// <returns></returns>
    IEnumerator DestroyMeteorite()
    {
        yield return new WaitForSeconds(10);
        gameObject.SetActive(false);
    }
}