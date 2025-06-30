using UnityEngine;

public class EnemyAiming : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Transform enemyBulletSpawn;

    void Start()
    {
        GameObject playerObj = GameObject.FindWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
        else
        {
            Debug.LogWarning("No se encontro al jugador con el tag 'Player'.");
        }
    }

    void Update()
    {
        if (player != null)
        {
            //El sistema de apuntado mide la longitud del vector en una radiante entre el BulletSpawnEnemy y
            //el jugador y luego combirtiendolo en un grado para que unity lo entienda en Quaternio,
            //por ultimo cambiando su angulo segun el valor
            Vector2 direction = player.position - enemyBulletSpawn.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            enemyBulletSpawn.rotation = Quaternion.Euler(0, 0, angle);
        }
    }
}
