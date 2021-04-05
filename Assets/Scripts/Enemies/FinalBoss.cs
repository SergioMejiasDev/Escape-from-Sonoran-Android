using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class that controls the main functions of the third and last boss.
/// </summary>
public class FinalBoss : MonoBehaviour
{
    #region Variables
    [Header("Health")]
    [SerializeField] float maxHealth = 750f;
    float health;
    [SerializeField] Image fullBattery = null;

    [Header("Attack")]
    GameObject player;
    GameObject bullet;
    [SerializeField] GameObject cannon = null;
    [SerializeField] GameObject arm = null;
    [SerializeField] Transform shootPointCannon = null;
    [SerializeField] Transform shootPointArm = null;
    bool moveCannon = true;
    bool moveArm;
    Quaternion cannonStartingRotation;
    Quaternion armStartingRotation;
    float timeBetweenAttacks = 8;
    bool mode2 = false;

    [Header("Attack Shoot")]
    float timeLastShoot;
    [SerializeField] float cadency = 1.0f;
    bool shoot0 = true;

    [Header("Attack Laser")]
    float cadency1;
    float attackSpeed1 = 0.2f;
    int maxShoots1 = 10;
    int maxRepetitions1;

    [Header("Attack Missile")]
    float cadency2 = 0.75f;
    float maxShoots2;
    [SerializeField] Transform spawnZoneRight2 = null;

    [Header("Attack Energy Ball")]
    [SerializeField] GameObject energyBall = null;
    [SerializeField] GameObject whiteBall = null;

    [Header("Attack Meteorite")]
    float cadency4 = 0.5f;
    int maxShoots4;
    [SerializeField] Transform spawnZoneUp4 = null;

    [Header("Attack Star")]
    [SerializeField] Transform centerRefence = null;
    [SerializeField] GameObject star = null;
    [SerializeField] GameObject redBall = null;

    [Header("Components")]
    [SerializeField] Animator anim = null;
    FinalBoss finalBoss;
    #endregion

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        cannonStartingRotation = cannon.transform.rotation;
        armStartingRotation = arm.transform.rotation;
        health = maxHealth;
        finalBoss = this;
        SelectAttack();
    }

    void Update()
    {
        if (moveCannon && player.activeSelf)
        {
            Vector3 dir = cannon.transform.position - player.transform.position;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            cannon.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            
            if ((Time.time - timeLastShoot > cadency) && shoot0)
            {
                Shoot(shootPointCannon);
            }
        }

        else
        {
            cannon.transform.rotation = cannonStartingRotation;
        }

        if (moveArm && player.activeSelf)
        {
            Vector3 dir = arm.transform.position - player.transform.position;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            arm.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("BulletPlayer"))
        {
            Hurt();

            collision.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Function where the enemy randomly chooses the attack to carry out.
    /// </summary>
    public void SelectAttack()
    {
        shoot0 = true;
        moveCannon = true;

        if (health <= (0.2f * maxHealth))
        {
            mode2 = true;
            timeBetweenAttacks = 4;
        }

        if (player.activeSelf)
        {
            float randomValue = Random.value;

            if (randomValue <= 0.1)
            {
                StartCoroutine(AttackEnergyBall());
            }
            else if (randomValue <= 0.2)
            {
                StartCoroutine(AttackStar());
            }
            else if (randomValue <= 0.4)
            {
                StartCoroutine(AttackMeteorite());
            }
            else if (randomValue <= 0.7)
            {
                StartCoroutine(AttackMissile());
            }
            else if (randomValue <= 1.0)
            {
                StartCoroutine(AttackLaser());
            }
        }
    }

    /// <summary>
    /// Function that is activated when the enemy suffers some damage.
    /// </summary>
    void Hurt()
    {
        health -= 1;

        fullBattery.fillAmount -= (1 / maxHealth);
        
        if (health <= 0)
        {
            StartCoroutine(Die());
        }
    }

    /// <summary>
    /// Function that we will call when we want the enemy to point at a target with his hand.
    /// </summary>
    /// <param name="target">The object the enemy will target.</param>
    void PointHand(Transform target)
    {
        Vector3 dir = arm.transform.position - target.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        arm.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    /// <summary>
    /// Function that we will call when we want the enemy to shoot.
    /// </summary>
    /// <param name="shootPoint">Site where the bullet is instantiated.</param>
    void Shoot(Transform shootPoint)
    {
        bullet = ObjectPooler.SharedInstance.GetPooledObject("BulletEnemy");

        if (bullet != null)
        {
            bullet.transform.position = shootPoint.transform.position;
            bullet.transform.rotation = shootPoint.transform.rotation;
            bullet.SetActive(true);
        }

        timeLastShoot = Time.time;
    }

    /// <summary>
    /// Function that we will call when the enemy dies and we want the attacks on the screen to disappear.
    /// </summary>
    void StopAttacks()
    {
        GameObject[] bullets = GameObject.FindGameObjectsWithTag("BulletEnemy");

        for (int i = 0; i < bullets.Length; i++)
        {
            bullets[i].SetActive(false);
        }

        GameObject activeStar = GameObject.FindGameObjectWithTag("Star");

        if (activeStar != null)
        {
            Destroy(activeStar);
        }

        GameObject[] activeMissiles = GameObject.FindGameObjectsWithTag("Missile");

        if (activeMissiles != null)
        {
            for (int i = 0; i < activeMissiles.Length; i++)
            {
                activeMissiles[i].SetActive(false);
            }
        }

        GameObject[] activeMeteorites = GameObject.FindGameObjectsWithTag("Meteorite");

        if (activeMeteorites != null)
        {
            for (int i = 0; i < activeMeteorites.Length; i++)
            {
                activeMeteorites[i].SetActive(false);
            }
        }

        GameObject[] activeLasers = GameObject.FindGameObjectsWithTag("Bullet4");

        if (activeLasers != null)
        {
            for (int i = 0; i < activeLasers.Length; i++)
            {
                activeLasers[i].SetActive(false);
            }
        }
    }

    /// <summary>
    /// Coroutine that launches the attack of lasers.
    /// </summary>
    /// <returns></returns>
    IEnumerator AttackLaser()
    {
        yield return new WaitForSeconds(timeBetweenAttacks);

        PointHand(player.transform);
        moveArm = true;
        int repetitions = 0;

        if (!mode2)
        {
            cadency1 = 1;
            maxRepetitions1 = 3;
        }
        else
        {
            cadency1 = 0.5f;
            maxRepetitions1 = 6;
        }

        while (repetitions < maxRepetitions1)
        {
            int currentShoots = 0;

            while (currentShoots < maxShoots1)
            {
                bullet = ObjectPooler.SharedInstance.GetPooledObject("Bullet4");

                if (health <= 0)
                {
                    yield break;
                }

                if (bullet != null)
                {
                    bullet.transform.position = shootPointArm.position;
                    bullet.transform.rotation = shootPointArm.rotation;
                    bullet.SetActive(true);
                }

                currentShoots += 1;

                yield return new WaitForSeconds(attackSpeed1);

            }

            repetitions += 1;

            yield return new WaitForSeconds(cadency1);
        }

        moveArm = false;

        arm.transform.rotation = armStartingRotation;

        SelectAttack();
    }

    /// <summary>
    /// Coroutine that launches the missile attack.
    /// </summary>
    /// <returns></returns>
    IEnumerator AttackMissile()
    {
        yield return new WaitForSeconds(timeBetweenAttacks);

        int shoots = 0;

        if (!mode2)
        {
            maxShoots2 = 10;
        }

        else
        {
            maxShoots2 = 20;
        }

        while (shoots < maxShoots2)
        {
            Vector2 spawnZone = new Vector2(spawnZoneRight2.position.x, spawnZoneRight2.position.y + Random.Range(-7f, 7f));

            GameObject missile = ObjectPooler.SharedInstance.GetPooledObject("Missile");

            if (health <= 0)
            {
                yield break;
            }

            if (missile != null)
            {
                missile.transform.position = spawnZone;
                missile.SetActive(true);
                shoots += 1;
                yield return new WaitForSeconds(cadency2);
            }
        }

        SelectAttack();
    }

    /// <summary>
    /// Coroutine that launches the attack of the energy ball.
    /// </summary>
    /// <returns></returns>
    IEnumerator AttackEnergyBall()
    {
        yield return new WaitForSeconds(timeBetweenAttacks);

        if (!mode2)
        {
            shoot0 = false;
            moveCannon = false;
        }

        moveArm = true;
        whiteBall.SetActive(true);
        yield return new WaitForSeconds(4);
        whiteBall.SetActive(false);

        if (health <= 0)
        {
            yield break;
        }

        Instantiate(energyBall, shootPointArm.position, shootPointArm.rotation);
        yield return new WaitForSeconds(7);
        moveArm = false;
        arm.transform.rotation = armStartingRotation;
        moveCannon = true;
        shoot0 = true;
        SelectAttack();
    }

    /// <summary>
    /// Coroutine that launches the meteor attack.
    /// </summary>
    /// <returns></returns>
    IEnumerator AttackMeteorite()
    {
        yield return new WaitForSeconds(timeBetweenAttacks);

        int shoots = 0;

        if (!mode2)
        {
            maxShoots4 = 30;
        }

        else
        {
            maxShoots4 = 60;
        }

        shoot0 = false;
        moveCannon = false;

        PointHand(spawnZoneUp4);

        whiteBall.SetActive(true);

        yield return new WaitForSeconds(2);

        whiteBall.SetActive(false);

        Instantiate(energyBall, shootPointArm.position, shootPointArm.rotation);

        yield return new WaitForSeconds(4);

        arm.transform.rotation = armStartingRotation;

        while (shoots < maxShoots4)
        {
            Vector2 spawnZone = new Vector2(spawnZoneUp4.position.x + Random.Range(-23.0f, 13.0f), spawnZoneUp4.position.y);

            GameObject meteorite = ObjectPooler.SharedInstance.GetPooledObject("Meteorite");

            if (health <= 0)
            {
                yield break;
            }

            if (meteorite != null)
            {
                meteorite.transform.position = spawnZone;
                meteorite.transform.rotation = spawnZoneUp4.rotation;
                meteorite.SetActive(true);
                shoots += 1;
                yield return new WaitForSeconds(cadency4);
            }
        }

        yield return new WaitForSeconds(3);

        moveCannon = true;
        shoot0 = true;

        SelectAttack();
    }

    /// <summary>
    /// Coroutine where the star attack is launched.
    /// </summary>
    /// <returns></returns>
    IEnumerator AttackStar()
    {
        yield return new WaitForSeconds(timeBetweenAttacks);

        shoot0 = false;
        moveCannon = false;

        PointHand(centerRefence);

        redBall.SetActive(true);

        yield return new WaitForSeconds(2);

        redBall.SetActive(false);

        if (health <= 0)
        {
            yield break;
        }

        Instantiate(star, redBall.transform.position, shootPointArm.rotation);

        yield return new WaitForSeconds(6);

        arm.transform.rotation = armStartingRotation;
    }

    /// <summary>
    /// Corroutine that is launched after the enemy dies.
    /// </summary>
    /// <returns></returns>
    IEnumerator Die()
    {
        anim.SetTrigger("Dying");
        StopAttacks();
        arm.SetActive(false);
        cannon.SetActive(false);
        finalBoss.enabled = false;
        yield return new WaitForSeconds(2);
        Boss3Manager.boss3Manager.StartExplosion(false);
    }
}