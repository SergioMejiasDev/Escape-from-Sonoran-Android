using UnityEngine;

/// <summary>
/// Class of the places where the player can bounce.
/// </summary>
public class BouncyBlock : MonoBehaviour
{
    [SerializeField] AudioSource audioSource = null;
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            audioSource.Play();
        }
    }
}