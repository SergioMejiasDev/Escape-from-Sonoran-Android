using System.Collections;
using UnityEngine;

/// <summary>
/// Class of the switches that open doors.
/// </summary>
public class Switch : MonoBehaviour
{
    [SerializeField] GameObject offSwitch = null;
    [SerializeField] GameObject door = null;
    [SerializeField] bool restart = false;
    [SerializeField] AudioSource audioSource = null;
    AudioSource doorAudioSource;
    [SerializeField] BoxCollider2D switchCollider = null;

    void Start()
    {
        doorAudioSource = door.GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("BulletPlayer"))
        {
            collision.gameObject.SetActive(false);
            StartCoroutine(SwitchOn());
        }
    }

    /// <summary>
    /// Coroutine where the switch is activated and opens the door.
    /// </summary>
    /// <returns></returns>
    IEnumerator SwitchOn()
    {
        offSwitch.SetActive(false);
        switchCollider.enabled = false;
        audioSource.Play();
        yield return new WaitForSeconds(1);
        doorAudioSource.Play();
        yield return new WaitForSeconds(1);
        door.SetActive(false);
        
        if (restart)
        {
            StartCoroutine(SwitchRestart());
        }
    }

    /// <summary>
    /// Coroutine where the switch and door are reset.
    /// </summary>
    /// <returns></returns>
    IEnumerator SwitchRestart()
    {
        yield return new WaitForSeconds(2);
        door.SetActive(true);
        doorAudioSource.Play();
        yield return new WaitForSeconds(2);
        offSwitch.SetActive(true);
        audioSource.Play();
        switchCollider.enabled = true;
    }
}