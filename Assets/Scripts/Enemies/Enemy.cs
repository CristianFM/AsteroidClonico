using System.Drawing;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private ParticleSystem destroyedParticles;
    [SerializeField] private InGameMenuManager inGameInterface;
    [SerializeField] private GameObject bulletContainer;
    [SerializeField] private GameObject particleContainer;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform enemyBulletSpawn;

    public EnemyManager enemyManager;
    private Rigidbody2D enemyRigibody;

    [SerializeField] public float life;
    [SerializeField] private float bulletSpeed = 8f;
    [SerializeField] private float fireRate = 1f;

    public int size = 3;
    private float nextFireTime;

    private void Awake()
    {
        inGameInterface = GameObject.FindGameObjectWithTag("InGameManager").GetComponent<InGameMenuManager>();
        bulletContainer = GameObject.FindGameObjectWithTag("BulletContainer");
        particleContainer = GameObject.FindGameObjectWithTag("ParticleContainer");
    }
    void Start()
    {
        //Movimiento del Enemigo
        enemyRigibody = GetComponent<Rigidbody2D>();
        transform.localScale = 0.5f * size * Vector3.one;

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        Vector2 direction = new Vector2(Random.value, Random.value).normalized;
        float spawnSpeed = Random.Range(4f - size, 5f - size);
        rb.AddForce(direction * spawnSpeed, ForceMode2D.Impulse);
    }

    void Update()
    {
        //Cooldown del disparo del enemigo
        if (Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + 1f / fireRate;
        }
    }
    private void getDmg(float dmg)
    {
        life -= dmg;
        if (life <= 0)
        {
            GameManager._instance.enemyDefeated++;
            inGameInterface.addPoints(50);
            Instantiate(destroyedParticles, transform.position, Quaternion.identity,particleContainer.transform);
            Destroy(gameObject);
        }
    }

    void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, enemyBulletSpawn.position, enemyBulletSpawn.rotation, bulletContainer.transform);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.velocity = enemyBulletSpawn.right * bulletSpeed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet") || collision.CompareTag("Orbital"))
        {
            if (collision.tag == "Bullet")
            {
                getDmg(collision.GetComponent<Bullet>().Damage);
            }
            if (collision.tag == "Orbital")
            {
                getDmg(collision.GetComponent<Orbital>().Power);
            }
        }
    }
}
