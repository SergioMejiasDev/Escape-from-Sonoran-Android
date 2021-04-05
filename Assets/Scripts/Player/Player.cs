using UnityEngine;

/// <summary>
/// Class with the movement and attack functions assigned to the player when he goes on foot (chapters 1 and 2).
/// </summary>
public class Player : MonoBehaviour
{
    #region Variables
    [Header("Movement")]
    [SerializeField] float speed = 4;
    [SerializeField] float jump = 8.0f;
    [SerializeField] LayerMask groundLayerMask = 0;
    [SerializeField] JoyStickController rightStick = null;
    Quaternion initialRotation;
    public bool inPlatform = false;
    float h;

    [Header("Shoot")]
    float timeLastShoot;
    [SerializeField] float cadency = 0;
    public GameObject arm;
    [SerializeField] Transform shootPoint = null;

    [Header("Components")]
    [SerializeField] Rigidbody2D rb = null;
    [SerializeField] Animator anim = null;
    #endregion

    private void OnEnable()
    {
        h = 0;
    }

    void Start()
    {
        arm.SetActive(true);
        initialRotation = arm.transform.rotation;
    }

    void Update()
    {
        if (Time.timeScale != 0)
        {
            Movement();

            Animation();

            Turn();

            if (Input.GetButtonDown("Cancel"))
            {
                GameManager.gameManager.PauseGame();
            }
        }

        else
        {
            h = 0;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("MovingPlatform"))
        {
            transform.parent = collision.transform;
            inPlatform = true;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("MovingPlatform"))
        {
            if ((collision.gameObject.GetComponent<MovingPlatform>().isVertical) && (collision.gameObject.GetComponent<MovingPlatform>().direction == -1))
            {
                rb.gravityScale = 0;
            }

            else
            {
                rb.gravityScale = 1;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("MovingPlatform"))
        {
            inPlatform = false;
            transform.SetParent(null);
            rb.gravityScale = 1;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "SpawnTrigger1")
        {
            Level1Manager.level1Manager.SpawnZone();
            collision.gameObject.SetActive(false);
        }

        else if (collision.gameObject.name == "SpawnTrigger2")
        {
            Level2Manager.level2Manager.SpawnZone();
            collision.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Boolean that indicates from a raycast if the player is touching the ground.
    /// </summary>
    /// <returns>True if the raycast touches an object with the specified mask, false if it doesn't.</returns>
    private bool IsGrounded()
    {
        RaycastHit2D hit1 = Physics2D.Raycast(new Vector2(transform.position.x - 0.8f, transform.position.y - 1.4f), Vector2.down, 0.2f, groundLayerMask);
        RaycastHit2D hit2 = Physics2D.Raycast(new Vector2(transform.position.x + 0.8f, transform.position.y - 1.4f), Vector2.down, 0.2f, groundLayerMask);
        
        return hit1 || hit2;
    }

    /// <summary>
    /// Function to activate movement through on-screen controls.
    /// </summary>
    /// <param name="input">2 right, 4 left, 5 enable. </param>
    public void EnableMovement(int input)
    {
        switch (input)
        {
            case 2:
                h = 1;
                break;
            case 4:
                h = -1;
                break;
            case 5:
                h = 0;
                break;
        }
    }

    /// <summary>
    /// Function that makes the player move.
    /// </summary>
    void Movement()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime * h);
    }

    /// <summary>
    /// Function that manages the movement of the arm and the rotation of the player.
    /// </summary>
    void Turn()
    {
        float rh = rightStick.horizontal;
        float rv = rightStick.vertical;

        if (rh != 0 && rv != 0)
        {
            if (rh > 0.1f)
            {
                if (!inPlatform)
                {
                    transform.localScale = new Vector3(0.5f, 0.5f, 1f);
                    arm.transform.localScale = new Vector3(1f, 1f, 1f);
                }

                else
                {
                    transform.localScale = new Vector3(0.33f, 0.33f, 1f);
                    arm.transform.localScale = new Vector3(1f, 1f, 1f);
                }
            }

            else if (rh < -0.1f)
            {
                if (!inPlatform)
                {
                    transform.localScale = new Vector3(-0.5f, 0.5f, 1f);
                    arm.transform.localScale = new Vector3(-1f, -1f, 1f);
                }

                else
                {
                    transform.localScale = new Vector3(-0.33f, 0.33f, 1f);
                    arm.transform.localScale = new Vector3(-1f, -1f, 1f);
                }
            }

            float angle = Mathf.Atan2(rv, rh) * Mathf.Rad2Deg;
            arm.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            if (Time.time - timeLastShoot > cadency)
            {
                Shoot();
            }
        }

        else
        {
            arm.transform.rotation = initialRotation;

            if (!inPlatform)
            {
                transform.localScale = new Vector3(0.5f, 0.5f, 1f);
                arm.transform.localScale = new Vector3(1f, 1f, 1f);
            }

            else
            {
                transform.localScale = new Vector3(0.33f, 0.33f, 1f);
                arm.transform.localScale = new Vector3(1f, 1f, 1f);
            }
        }
    }

    /// <summary>
    /// Function that makes animations run.
    /// </summary>
    void Animation()
    {
        anim.SetBool("Walking", h != 0 && IsGrounded());
        anim.SetBool("Jumping", !IsGrounded());
    }

    /// <summary>
    /// Function that makes the jumps run.
    /// </summary>
    public void Jump()
    {
        if (IsGrounded())
        {
            rb.AddForce(Vector2.up * jump, ForceMode2D.Impulse);
        }
    }

    /// <summary>
    /// Function that makes the player shoot.
    /// </summary>
    void Shoot()
    {
        GameObject bullet = ObjectPooler.SharedInstance.GetPooledObject("BulletPlayer");
        
        timeLastShoot = Time.time;
        
        if (bullet != null)
        {
            bullet.transform.position = shootPoint.position;
            bullet.transform.rotation = shootPoint.rotation;
            bullet.SetActive(true);
        }
    }
}