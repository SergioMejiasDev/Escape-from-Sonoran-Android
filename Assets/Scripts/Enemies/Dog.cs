using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class that controls the main functions of the second boss.
/// </summary>
public class Dog : MonoBehaviour
{
    #region Variables
    [Header("Movement")]
    [SerializeField] float speed = 2;
    int direction = 1;
    Vector2 startingPosition;
    
    [Header("Attack")]
    GameObject player;
    GameObject bullet;
    [SerializeField] Transform cannon = null;
    float timeLastShoot;
    [SerializeField] float cadency = 1.5f;

    [Header("Health")]
    [SerializeField] float maxHealth = 4; 
    float health;
    [SerializeField] Image fullBattery = null;
    [SerializeField] GameObject explosion = null;

    [Header("Components")]
    [SerializeField] Animator anim = null;
    #endregion

    void Start()
    {
        health = maxHealth;
        player = GameObject.FindGameObjectWithTag("Player");
        startingPosition = transform.position;
    }

    void Update()
    {
        if (player.activeSelf)
        {
            Movement();

            ChangeDirection();

            Point();

            Animation();
        }

        else
        {
            anim.SetBool("IsIddle", true);
            direction = 0;
        }
    }

    /// <summary>
    /// Function that makes the enemy move from one side of the screen to the other.
    /// </summary>
    void Movement()
    {
        transform.Translate(Vector2.right * speed * direction * Time.deltaTime);
    }

    /// <summary>
    /// Feature that makes the enemy change direction when he reaches the edge of the screen.
    /// </summary>
    void ChangeDirection()
    {
        if ((transform.position.x) > startingPosition.x + 13)
        {
            direction = -1;
        }

        else if ((transform.position.x) < startingPosition.x - 13)
        {
            direction = 1;
        }

        if ((player.transform.position.x) > (transform.position.x))
        {
            transform.localScale = new Vector3(-0.9f, 0.9f, 1f);
        }

        else
        {
            transform.localScale = new Vector3(0.9f, 0.9f, 1f);
        }
    }

    /// <summary>
    /// Function that makes the enemy constantly point towards the player.
    /// </summary>
    void Point()
    {
        Vector3 dir = player.transform.position - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        cannon.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        if (Time.time - timeLastShoot > cadency)
        {
            Shoot();
        }
    }

    /// <summary>
    /// Feature that makes the enemy constantly shoot the player.
    /// </summary>
    void Shoot()
    {
        bullet = ObjectPooler.SharedInstance.GetPooledObject("BulletEnemy");
        
        if (bullet != null)
        {
            bullet.transform.position = cannon.position;
            bullet.transform.rotation = cannon.rotation;
            bullet.SetActive(true);
        }

        timeLastShoot = Time.time;
    }

    /// <summary>
    /// Function that activates the animation of the enemy.
    /// </summary>
    void Animation()
    {
        anim.SetBool("IsWalking", true);
    }

    /// <summary>
    /// Function that is activated when the enemy suffers some damage.
    /// </summary>
    /// <param name="damage">Amount of damage the enemy takes.</param>
    public void Hurt(int damage)
    {
        health -= damage;
        fullBattery.fillAmount -= (damage / maxHealth);

        if (health <= 0)
        {
            StartCoroutine(Die());
        }
    }

    /// <summary>
    /// Coroutine where the enemy dies and we call the transition between scenes.
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

        GetComponent<Dog>().enabled = false;
        yield return new WaitForSeconds(2);
        Instantiate(explosion, transform.position, transform.rotation);
        Boss2Manager.boss2Manager.BossDie();
        Destroy(gameObject);
    }
}