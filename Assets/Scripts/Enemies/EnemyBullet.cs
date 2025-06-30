using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    [SerializeField] private float bulletLifetime = 1f;
    [SerializeField] public float damage;
    [SerializeField] public float life;

    private void Awake()
    {
        Destroy(gameObject, bulletLifetime);
        life = 0;
    }
}
