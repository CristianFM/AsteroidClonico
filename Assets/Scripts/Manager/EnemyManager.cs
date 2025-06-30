using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] public GameObject asteroidPrefab;
    [SerializeField] private Enemy enemyPrefab;
    [SerializeField] private float spawnDelay = 5f;

    public int asteroidCount = 0;

    private int level = 0;

    private float nextSpawnTime;

    [SerializeField]private bool firstRoundFlag;
    [SerializeField]private bool test;

    void Start()
    {
        // Esto retrasa el primer spawn
        nextSpawnTime = Time.time + spawnDelay;
        firstRoundFlag = true;
    }

    // Update is called once per frame
    void Update()
    {
        manageAsteroidWave();
        // Verifica si es momento de intentar un nuevo spawn
        enemySpawn();
    }

    #region ManageSpawns
    private void manageAsteroidWave()
    {
        if (asteroidCount == 0)
        {
            level++;

            if (firstRoundFlag)
            {
                firstRoundFlag = false;
            }
            else
            {
                InGameMenuManager._instance.triggerLvUp();
                GameManager._instance.waveCleareds();
            }

            int numAsteroids = 2 * level;
            for (int i = 0; i < numAsteroids; i++)
            {
                addNewAsteroid();
                SpawnAsteroid();
            }
        }
    }

    private float enemyLifeModifier()
    {
        return level / 5;
    }
    #endregion

    #region SpawnScrips
    private void enemySpawn()
    {
        if (Time.time >= nextSpawnTime)
        {
            // Verifica si NO hay enemigos activos
            if (GameObject.FindGameObjectsWithTag("Enemy").Length == 0)
            {
                SpawnEnemy();
                nextSpawnTime = Time.time + spawnDelay; // reinicia el temporizador
            }
        }
    }
    public void addNewAsteroid()
    {
        asteroidCount++;
    }
    private void SpawnAsteroid()
    {
        GameObject asteroid = Instantiate(asteroidPrefab, getSpawn(), Quaternion.identity);
        asteroid.GetComponent<Asteroid>().enemyManager = this;
        asteroid.GetComponent<Asteroid>().life += (int)enemyLifeModifier();
    }

    void SpawnEnemy()
    {
        Enemy enemyShip = Instantiate(enemyPrefab, getSpawn(), Quaternion.identity);
        enemyShip.GetComponent<Enemy>().enemyManager = this;
        enemyShip.life += (int)enemyLifeModifier();
    }

    private Vector2 getSpawn()
    {
        float offset = Random.Range(0f, 1f);
        Vector2 viewportSpawnPosition = Vector2.zero;

        int edge = Random.Range(0, 4);
        if (edge == 0)
        {
            viewportSpawnPosition = new Vector2(offset, 0);
        }
        else if (edge == 1)
        {
            viewportSpawnPosition = new Vector2(offset, 1);
        }
        else if (edge == 2)
        {
            viewportSpawnPosition = new Vector2(0, offset);
        }
        else if (edge == 3)
        {
            viewportSpawnPosition = new Vector2(1, offset);
        }

        Vector2 worldSpawnPosition = Camera.main.ViewportToWorldPoint(viewportSpawnPosition);

        return worldSpawnPosition;
    }
    #endregion
}
