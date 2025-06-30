using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    //variables numericas
    private int bulletCount = 0;
    private float bulletSpeed = 5f;
    private float power = 1;
    private float timer = 0;

    //variables Llamadas de Objetos
    private AudioManager audioManager;
    [SerializeField]private GameObject bulletPrefab;

    public float Power { get => power; set => power = value; }
    public int BulletCount { get => bulletCount; set => bulletCount = value; }

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        lookAtCursor();
    }
    private void Start()
    {
        Power = 1;
    }
    private void Update()
    {
        weaponBehavior();
    }

    //Lanzo esta funcion al comienzo de la vida del gameobject para que mire a la direccion que el jugador apunta
        private void lookAtCursor()
    {

        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 rotation = mousePosition - transform.position;

        float rotZ = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0, 0, rotZ);
    }


    //------------Weapon Behavior----------------
        private void weaponBehavior()
        {
            timer += Time.deltaTime;
            if (timer >= 0.5f)
            {
                Explode();
                Destroy(gameObject);
            }
        }

        //funcion de gestion de la subexplosion y la dispersion de balas
        void Explode()
    {
        float angleStep = 360f / BulletCount;

        audioManager.PlaySFX(audioManager.Explosion);
        for (int i = 0; i < BulletCount; i++)
        {
            float angle = i * angleStep * Mathf.Deg2Rad;
            Vector2 direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)).normalized;

            // Spawn con pequeño desplazamiento
            Vector2 spawnPos = (Vector2)transform.position + direction * 0.2f;
            GameObject bullet = Instantiate(bulletPrefab, spawnPos, Quaternion.identity);

            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

            bullet.GetComponent<Bullet>().Damage = Power;
            bullet.GetComponent<Bullet>().Life = 0;
            rb.velocity = direction * bulletSpeed;
        }
    }

    //-----------------triggerBehavior-------------
        void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy") || other.CompareTag("Asteroid"))
        {
            Explode();
            Destroy(gameObject);
        }
    }
}
