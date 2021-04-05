using System.Collections;
using UnityEngine;

/// <summary>
/// Class of the explosions that are generated when the player or the enemies die.
/// </summary>
public class Explosion : MonoBehaviour
{
    [SerializeField] AudioSource audioSource = null;

    void OnEnable()
    {
        audioSource.Play();
        StartCoroutine(DestroyExplosion());
    }

    /// <summary>
    /// Coroutine where the explosion is destroyed.
    /// </summary>
    /// <returns></returns>
    IEnumerator DestroyExplosion()
    {
        yield return new WaitForSeconds(0.7f);
        gameObject.SetActive(false);
    }
}