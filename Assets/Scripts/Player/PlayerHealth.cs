using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Player health class. It is common both when you are on foot and when you are flying.
/// </summary>
public class PlayerHealth : MonoBehaviour
{
    #region Variables
    [Header("Health")]
    [SerializeField] float maxHealth = 10;
    float health;
    [SerializeField] Image fullBattery = null;
    [SerializeField] Image hurtImage = null;
    [SerializeField] AudioClip hurtSound = null;
    [SerializeField] AudioClip batterySound = null;
    bool isHurt;
    [SerializeField] bool isType2 = false;
    GameObject explosion;

    [Header("Components")]
    [SerializeField] AudioSource audioSource = null;
    [SerializeField] Animator anim = null;
    #endregion

    void Start()
    {
        health = maxHealth;
    }

    void Update()
    {
        if (isHurt)
        {
            hurtImage.color = new Color(1.0f, 0.0f, 0.0f, 0.2f);
            audioSource.clip = hurtSound;
            audioSource.Play();
            isHurt = false;
        }

        else
        {
            hurtImage.color = Color.Lerp(hurtImage.color, Color.clear, 10.0f * Time.deltaTime);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Hurt(1);
        }

        else if (collision.gameObject.CompareTag("Boss"))
        {
            Hurt(1);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Sword"))
        {
            Hurt(1);
        }

        else if (collision.gameObject.CompareTag("Battery"))
        {
            RestoreHealth(5);
            audioSource.clip = batterySound;
            audioSource.Play();
            Destroy(collision.gameObject);
        }

        else if (collision.gameObject.CompareTag("CheckPoint"))
        {
            Destroy(collision.gameObject);
            GameManager.gameManager.CheckPoint(collision.gameObject.transform.position);
        }
    }

    /// <summary>
    /// Function that is called when the player takes some damage.
    /// </summary>
    /// <param name="damage">Amount of damage received.</param>
    public void Hurt(int damage)
    {
        if (health > 0)
        {
            health -= damage;
            fullBattery.fillAmount -= (damage / maxHealth);
            isHurt = true;

            if (health <= 0)
            {
                health = 0;

                if (GetComponent<Player>() != null)
                {
                    StartCoroutine(Die());
                }
                else
                {
                    StartCoroutine(DieFlying());
                }
            }
        }
    }

    /// <summary>
    /// Function we call when the player restores health.
    /// </summary>
    /// <param name="restoredHealth">Amount of health that is restored.</param>
    void RestoreHealth(float restoredHealth)
    {
        health += restoredHealth;

        fullBattery.fillAmount = (health / maxHealth);
        {
            if (health > maxHealth)
            {
                health = maxHealth;
                fullBattery.fillAmount = 1;
            }
        }
    }

    /// <summary>
    /// Function that is responsible for restoring the player after continuing after a Game Over.
    /// </summary>
    public void RestorePlayer()
    {
        gameObject.SetActive(true);

        if (GetComponent<Player>() != null)
        {
            GetComponent<Player>().enabled = true;
            GetComponent<Player>().arm.SetActive(true);
            GetComponent<Player>().inPlatform = false;
        }

        else
        {
            GetComponent<FlyingPlayer>().enabled = true;
            GetComponent<FlyingPlayer>().arm.SetActive(true);
        }

        RestoreHealth(maxHealth);
    }

    /// <summary>
    /// Corroutine where the player on foot dies.
    /// </summary>
    /// <returns></returns>
    IEnumerator Die()
    {
        if (!isType2)
        {
            anim.SetTrigger("Dying1");
        }

        else if (isType2)
        {
            anim.SetTrigger("Dying2");
        }

        GetComponent<Player>().arm.SetActive(false);
        GetComponent<Player>().enabled = false;

        yield return new WaitForSeconds(2);

        explosion = ObjectPooler.SharedInstance.GetPooledObject("Explosion");
        
        if (explosion != null)
        {
            explosion.SetActive(true);
            explosion.transform.position = transform.position;
            explosion.transform.rotation = transform.rotation;
        }

        GameManager.gameManager.GameOver();
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Corroutine where the flying player dies.
    /// </summary>
    /// <returns></returns>
    IEnumerator DieFlying()
    {
        anim.SetTrigger("Dying3");
        GetComponent<FlyingPlayer>().arm.SetActive(false);
        GetComponent<FlyingPlayer>().enabled = false;
        yield return new WaitForSeconds(2);
        explosion = ObjectPooler.SharedInstance.GetPooledObject("Explosion");
        if (explosion != null)
        {
            explosion.SetActive(true);
            explosion.transform.position = transform.position;
            explosion.transform.rotation = transform.rotation;
        }
        GameManager.gameManager.GameOver();
        gameObject.SetActive(false);
    }
}