using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float bulletLifetime = 1f;
    [SerializeField] private float damage;
    [SerializeField] private float life;

    #region Encapsulamiento
    public float Damage { get => damage; set => damage = value; }
    public float Life { get => life; set => life = value; }
    #endregion

    private void Awake()
    {
        Destroy(gameObject, bulletLifetime);
        Life = 0;
    }
    private void getDamage(float dmg)
    {
        Life -= dmg;
        if (Life <= 0) 
        { 
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Enemy") || collision.CompareTag("Asteroid"))
        {
            getDamage(1);
        }
    }
}
