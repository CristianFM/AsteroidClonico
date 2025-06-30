using System.Collections;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    public int life;
    private int size = 3;
    private bool isHit;
    private float timer;

    [SerializeField] private ParticleSystem destroyedParticles;
    [SerializeField] public EnemyManager enemyManager;
    [SerializeField] private InGameMenuManager inGameInterface;
    [SerializeField] private GameObject bulletContainer;
    [SerializeField] private GameObject enemyContainer;
    [SerializeField] private GameObject particleContainer;

    private void Awake()
    {
        enemyManager = GameObject.FindGameObjectWithTag("EnemyManager").GetComponent<EnemyManager>();
        inGameInterface = GameObject.FindGameObjectWithTag("InGameManager").GetComponent<InGameMenuManager>();
        bulletContainer = GameObject.FindGameObjectWithTag("BulletContainer");
        enemyContainer = GameObject.FindGameObjectWithTag("EnemyContainer");
        particleContainer = GameObject.FindGameObjectWithTag("ParticleContainer");
    }
    void Start()
    {
        //Velocidad y movimiento del Asteroide

        transform.localScale = 0.5f * size * Vector3.one;
        setMovementVariable();

        //asteroidCount++;
        //Sumar al contador de asteroides para saber si hay que cambiar de lvl
    }
    private void Update()
    {
        isInvulnerable();
    }
    private void setMovementVariable()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        Vector2 direction = new Vector2(Random.value, Random.value).normalized;
        float spawnSpeed = Random.Range(4f - size, 5f - size);
        rb.AddForce(direction * spawnSpeed, ForceMode2D.Impulse);
    }
    private void getDmg(int dmg)
    {
        life -= dmg;
        if (life <= 0)
        {
            inGameInterface.addPoints(10);
            enemyManager.asteroidCount--;
            GameManager._instance.asteroidDefeated++;
            generateSmallerAsteroids();
            Instantiate(destroyedParticles, transform.position, Quaternion.identity, particleContainer.transform);
        }
    }
    private void isInvulnerable()
    {

        if(isHit == true)
        {
            timer += Time.deltaTime;
            if(timer > 0.1f)
            {
                isHit = false;
            }
        }
    }
    //esta funcion generara nuevos asteroides cuando este sea destruido pero mas pequeños
    private void generateSmallerAsteroids()
    {
        if (size > 1)
        {
            for (int i = 0; i < 2; i++)
            {
                enemyManager.addNewAsteroid();
                GameObject newAsteroid = Instantiate(enemyManager.asteroidPrefab, transform.position, Quaternion.identity, enemyContainer.transform );
                newAsteroid.GetComponent<Asteroid>().size = size - 1;
                //newAsteroid.enemyManager = enemyManager;
            }
        }
        Destroy(gameObject);

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isHit == false) 
        {
            isHit = true;
            if (collision.CompareTag("Bullet") || collision.CompareTag("Orbital"))
            {
                if (collision.tag == "Bullet")
                {
                    getDmg((int)collision.GetComponent<Bullet>().Damage);
                }
                if (collision.tag == "Orbital")
                {
                    getDmg((int)collision.GetComponent<OrbitalOrb>().Damage);
                }
                if (collision.tag == "Missile")
                {
                    getDmg((int)collision.GetComponent<Missile>().Power);
                }
            }
        }       
    }
}
