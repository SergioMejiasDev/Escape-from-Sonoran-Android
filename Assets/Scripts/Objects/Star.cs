using System.Collections;
using UnityEngine;

/// <summary>
/// Class of the star that launches the final boss.
/// </summary>
public class Star : MonoBehaviour
{
    float rotationSpeed;
    float attackSpeed;
    int maxShoots;
    [SerializeField] GameObject[] cannons = null;

    void Start()
    {
        StartCoroutine(Move());
    }

    void Update()
    {
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("BulletPlayer"))
        {
            collision.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Coroutine where the star moves to the center of the screen.
    /// </summary>
    /// <returns></returns>
    IEnumerator Move()
    {
        while (transform.position.x > -12)
        {
            transform.Translate(Vector2.right * 5 * Time.deltaTime);
            yield return null;
        }

        StartCoroutine(Shoot());
    }

    /// <summary>
    /// Coroutine where the star begins to shoot.
    /// </summary>
    /// <returns></returns>
    IEnumerator Shoot()
    {
        yield return new WaitForSeconds(3);

        int currentShoots = 0;

        attackSpeed = 1.0f;

        maxShoots = 5;

        rotationSpeed = 25;

        while (currentShoots < maxShoots)
        {
            foreach (GameObject shootPoint in cannons)
            {
                GameObject bullet = ObjectPooler.SharedInstance.GetPooledObject("Bullet4");

                if (bullet != null)
                {
                    bullet.transform.position = shootPoint.transform.position;
                    bullet.transform.rotation = shootPoint.transform.rotation;
                    bullet.SetActive(true);
                }
            }

            currentShoots += 1;

            yield return new WaitForSeconds(attackSpeed);
        }

        currentShoots = 0; 

        attackSpeed = 0.5f;

        maxShoots = 15;

        rotationSpeed = -35;

        while (currentShoots < maxShoots)
        {
            foreach (GameObject shootPoint in cannons)
            {
                GameObject bullet = ObjectPooler.SharedInstance.GetPooledObject("Bullet4");

                if (bullet != null)
                {
                    bullet.transform.position = shootPoint.transform.position;
                    bullet.transform.rotation = shootPoint.transform.rotation;
                    bullet.SetActive(true);
                }
            }

            currentShoots += 1;

            yield return new WaitForSeconds(attackSpeed);
        }

        currentShoots = 0;

        attackSpeed = 0.25f;

        maxShoots = 60;

        rotationSpeed = 45;

        while (currentShoots < maxShoots)
        {
            foreach (GameObject shootPoint in cannons)
            {
                GameObject bullet = ObjectPooler.SharedInstance.GetPooledObject("Bullet4");

                if (bullet != null)
                {
                    bullet.transform.position = shootPoint.transform.position;
                    bullet.transform.rotation = shootPoint.transform.rotation;
                    bullet.SetActive(true);
                }
            }

            currentShoots += 1;

            yield return new WaitForSeconds(attackSpeed);
        }

        yield return new WaitForSeconds(2);
        GameObject boss = GameObject.FindGameObjectWithTag("Boss");
        boss.GetComponent<FinalBoss>().SelectAttack();
        
        GameObject explosion = ObjectPooler.SharedInstance.GetPooledObject("Explosion");
        if (explosion != null)
        {
            explosion.SetActive(true);
            explosion.transform.position = transform.position;
        }

        Destroy(gameObject);
    }
}