using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitalOrb : MonoBehaviour
{

    // Start is called before the first frame update
    private float damage;
    private Orbital orbital;

    public Orbital Orbital { get => orbital; set => orbital = value; }
    public float Damage { get => damage; set => damage = value; }

    void Start()
    {
        Damage = 1;
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Asteroid")|| collision.CompareTag("Enemy"))
        {
            Orbital.orbReparation(this.gameObject);
            this.gameObject.SetActive(false);
        }
    }

}
