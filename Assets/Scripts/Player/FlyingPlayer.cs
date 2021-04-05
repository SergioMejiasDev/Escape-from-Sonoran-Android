using UnityEngine;

/// <summary>
/// Class with the movement and attack functions assigned to the player when flying (chapter 3).
/// </summary>
public class FlyingPlayer : MonoBehaviour
{
    #region Variables
    [Header("Movement")]
    [SerializeField] float speed = 4;
    [SerializeField] JoyStickController leftStick = null;
    [SerializeField] JoyStickController rightStick = null;
    Quaternion initialRotation;

    [Header("Shoot")]
    float timeLastShoot;
    [SerializeField] float cadency = 0.25f;
    public GameObject arm;
    [SerializeField] Transform shootPoint = null;
    #endregion

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

            Turn();

            if (Input.GetButtonDown("Cancel"))
            {
                GameManager.gameManager.PauseGame();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "SpawnTrigger3")
        {
            Level3Manager.level3Manager.StartFade();
            Destroy(collision.gameObject);
        }
    }

    /// <summary>
    /// Function that manages the movement of the arm and the rotation of the player.
    /// </summary>
    void Turn()
    {
        float h = rightStick.horizontal;
        float v = rightStick.vertical;

        if (h != 0 && v != 0)
        {
            if (h > 0.1f)
            {
                transform.localScale = new Vector3(0.5f, 0.5f, 1f);
                arm.transform.localScale = new Vector3(1f, 1f, 1f);
            }

            else if (h < -0.1f)
            {
                transform.localScale = new Vector3(-0.5f, 0.5f, 1f);
                arm.transform.localScale = new Vector3(-1f, -1f, 1f);
            }

            float angle = Mathf.Atan2(v, h) * Mathf.Rad2Deg;
            arm.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            if (Time.time - timeLastShoot > cadency)
            {
                Shoot();
            }
        }

        else
        {
            arm.transform.rotation = initialRotation;
            transform.localScale = new Vector3(0.5f, 0.5f, 1f);
            arm.transform.localScale = new Vector3(1f, 1f, 1f);
        }
    }


    /// <summary>
    /// Function that makes the player move.
    /// </summary>
    void Movement()
    {
        float h = leftStick.horizontal;
        float v = leftStick.vertical;

        transform.Translate(Vector2.right * speed * Time.deltaTime * h);
        transform.Translate(Vector2.up * speed * Time.deltaTime * v);
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