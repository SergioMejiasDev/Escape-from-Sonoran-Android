using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class that controls the main functions of the first boss.
/// </summary>
public class EnemyBig : MonoBehaviour
{
    #region Variables
    [Header("Movement")]
    [SerializeField] float speed = 1;
    int direction = 0;

    [Header("Attack")]
    GameObject player;
    GameObject bullet;
    [SerializeField] GameObject cannon = null;
    [SerializeField] Transform shootPoint = null;
    float timeLastShoot;
    [SerializeField] float cadency = 1.5f;

    [Header("Health")]
    [SerializeField] float maxHealth = 25;
    float health;
    [SerializeField] Image fullBattery = null;
    [SerializeField] GameObject explosion = null;

    [Header("Components")]
    [SerializeField] Animator anim = null;
    EnemyBig enemyBigScript;
    #endregion

    void Start()
    {
        health = maxHealth;
        player = GameObject.FindGameObjectWithTag("Player");
        enemyBigScript = this;
    }

    void Update()
    {
        if (player.activeSelf)
        {
            if (player.transform.position.x < transform.position.x)
            {
                direction = -1;
            }

            else
            {
                direction = 1;
            }

            Movement();

            Point();

            Animation();
        }

        else
        {
            direction = 0;
            anim.SetBool("IsWalking", false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if ((other.gameObject.CompareTag("BulletPlayer")))
        {
            other.gameObject.SetActive(false);
            health -= 1;
            fullBattery.fillAmount -= (1 / maxHealth);

            if (health <= 0)
            {
                StartCoroutine(Die());
            }
        }
    }

    /// <summary>
    /// Function that makes the enemy move constantly.
    /// </summary>
    void Movement()
    {
        transform.Translate(Vector2.right * speed * direction * Time.deltaTime);
    }

    /// <summary>
    /// Feature that makes the enemy constantly target the player.
    /// </summary>
    void Point()
    {
        if (direction == -1)
        {
            transform.localScale = new Vector3(0.75f, 0.75f, 1f);
            cannon.transform.localScale = new Vector3(1f, 1f, 1f);
        }

        else
        {
            transform.localScale = new Vector3(-0.75f, 0.75f, 1f);
            cannon.transform.localScale = new Vector3(-1f, -1f, 1f);
        }

        Vector3 dir = transform.position - player.transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        cannon.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        if (Time.time - timeLastShoot > cadency)
        {
            Shoot();
        }
    }

    /// <summary>
    /// Function that makes the enemy shoot constantly.
    /// </summary>
    void Shoot()
    {
        bullet = ObjectPooler.SharedInstance.GetPooledObject("BulletEnemy");

        if (bullet != null)
        {
            bullet.transform.position = shootPoint.position;
            bullet.transform.rotation = shootPoint.rotation;
            bullet.SetActive(true);
        }
        
        timeLastShoot = Time.time;
    }

    /// <summary>
    /// Function that activates the animation of the enemy.
    /// </summary>
    void Animation()
    {
        anim.SetBool("IsWalking", direction != 0);
    }

    /// <summary>
    /// Coroutine where the enemy dies and the transition between scenes is activated.
    /// </summary>
    /// <returns></returns>
    IEnumerator Die()
    {
        anim.SetTrigger("Dying");
        GameObject[] bullets = GameObject.FindGameObjectsWithTag("BulletEnemy");
        for (int i = 0; i < bullets.Length; i++)
        {
            bullets[i].SetActive(false);
        }
        cannon.SetActive(false);
        enemyBigScript.enabled = false;
        yield return new WaitForSeconds(2);
        Instantiate(explosion, transform.position, transform.rotation);
        Boss1Manager.boss1Manager.BossDeath();
        Destroy(gameObject);
    }
}
