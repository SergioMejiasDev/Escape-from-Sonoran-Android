using System.Collections;
using UnityEngine;

/// <summary>
/// Class associated with the type 2 enemy (green color, walks and melee attacks).
/// </summary>
public class EnemyClass2 : MonoBehaviour
{
    #region Variables
    [Header("Movement")]
    [SerializeField] float speed = 3;
    public int direction = 1;
    Vector3 startingPosition;
    [SerializeField] float movementDistance = 100;

    [Header("Attack")]
    float timeLastAttack;
    [SerializeField] float cadency = 0.85f;
    GameObject player;
    [SerializeField] GameObject attackPoint = null;

    [Header("Health")]
    [SerializeField] int health = 5;
    GameObject explosion;

    [Header("Components")]
    [SerializeField] Animator anim = null;
    [SerializeField] AudioSource audioSource = null;
    #endregion

    void Start()
    {
        startingPosition = transform.position;
        player = GameObject.FindGameObjectWithTag("Player");
        attackPoint.SetActive(false);
    }

    void Update()
    {
        if (direction == 1)
        {
            transform.localScale = new Vector3(0.5f, 0.5f, 1f);
        }

        else
        {
            transform.localScale = new Vector3(-0.5f, 0.5f, 1f);
        }

        if ((player.activeSelf) && (Vector3.Distance(transform.position, player.transform.position) < 3))
        {
            Attack();
        }

        else
        {
            Movement();
        }

        Animation();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if ((other.gameObject.CompareTag("BulletPlayer")))
        {
            other.gameObject.SetActive(false);
            health -= 1;

            if (health <= 0)
            {
                explosion = ObjectPooler.SharedInstance.GetPooledObject("Explosion");
                if (explosion != null)
                {
                    explosion.SetActive(true);
                    explosion.transform.position = transform.position;
                    explosion.transform.rotation = transform.rotation;
                }

                Destroy(gameObject);
            }
        }
    }

    /// <summary>
    /// Function that makes the enemy move constantly.
    /// </summary>
    void Movement()
    {
        transform.Translate(Vector2.right * speed * direction * Time.deltaTime);

        ChangeDirection();
    }

    /// <summary>
    /// Function that makes the enemy attack constantly.
    /// </summary>
    void Attack()
    {
        if (player.transform.position.x < transform.position.x)
        {
            direction = -1;
        }

        else if (player.transform.position.x > transform.position.x)
        {
            direction = 1;
        }

        if (Time.time - timeLastAttack > cadency)
        {
            timeLastAttack = Time.time;
            attackPoint.SetActive(true);
            audioSource.Play();
            StartCoroutine(DestroyAttack());
        }
    }

    /// <summary>
    /// A feature that constantly activates the enemy's animation.
    /// </summary>
    void Animation()
    {
        anim.SetBool("Attack", ((player.activeSelf) && (Vector3.Distance(transform.position, player.transform.position) < 3)));
    }

    /// <summary>
    /// Function that makes the enemy change direction.
    /// </summary>
    void ChangeDirection()
    {
        if ((transform.position.x) > startingPosition.x + movementDistance)
        {
            direction = -1;
        }

        else if ((transform.position.x) < startingPosition.x - movementDistance)
        {
            direction = 1;
        }
    }

    /// <summary>
    /// Corroutine that causes the GameObject that damages the enemy to be destroyed.
    /// </summary>
    /// <returns></returns>
    IEnumerator DestroyAttack()
    {
        yield return new WaitForSeconds(0.1f);
        attackPoint.SetActive(false);
    }
}
