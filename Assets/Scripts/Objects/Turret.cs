using UnityEngine;

/// <summary>
/// Automatic turrets class.
/// </summary>
public class Turret : MonoBehaviour
{
    GameObject bullet;
    [SerializeField] Transform shootPoint = null;
    float timeLastShoot;
    public float cadency;

    void Update()
    {
        if (Time.time - timeLastShoot > cadency)
        {
            timeLastShoot = Time.time;

            bullet = ObjectPooler.SharedInstance.GetPooledObject("BulletEnemy");

            if (bullet != null)
            {
                bullet.transform.position = shootPoint.position;
                bullet.transform.rotation = shootPoint.rotation;
                bullet.SetActive(true);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Water"))
        {
            AudioSource audioSource = collision.gameObject.GetComponent<AudioSource>();
            audioSource.Play();
            Destroy(gameObject);
        }
    }
}