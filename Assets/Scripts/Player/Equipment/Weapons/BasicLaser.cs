using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BasicLaser : Equipment
{
    [Header("Atributte")]
    [SerializeField] private int lv;
    [SerializeField] private int lvMax;
    [SerializeField] private float power;
    [SerializeField] private float penetrationPower;
    private float bulletSpeed;
    private float shootCooldown;

    [Header("Informacion del arma")]
    [SerializeField] private string nextLvText;
    [SerializeField] private string titleWeapon;

    [Header("Object references")]
    [SerializeField] private GameObject bulletContainer;
    [SerializeField] private AudioManager audioManager;
    [SerializeField] private Rigidbody2D bulletPrefab;
    [SerializeField] private Rigidbody2D shipRigidbody;

    //Variables de control
    private bool isAlive = true;


    #region encapsulamiento
    public int Lv { get => lv; set => lv = value; }
    public float Power { get => power; set => power = value; }
    public float PenetrationPower { get => penetrationPower; set => penetrationPower = value; }
    public int LvMax { get => lvMax; set => lvMax = value; }
    public string NextLvText { get => nextLvText; set => nextLvText = value; }
    public string TitleWeapon { get => titleWeapon; set => titleWeapon = value; }
    #endregion

    #region OverrideFunction  (Lv Related)
    // Utilizamos este metodo del padre para actualizar los valores de este equipamiento
    public override void setAttributesValues(string variable, float value)
    {
        if (variable == "lv")
        {
            Lv += (int)value;
        }
        else if (variable == "power")
        {
            Power += value;
        }
        else if (variable == "penetration")
        {
            PenetrationPower += value;
        }
        else
        {
            Debug.LogError("No se encontro la variable");
        }
    }

    public override float getAttributesValues(string variable)
    {
        if (variable == "lv")
        {
            return Lv;
        }
        else if (variable == "lvMax")
        {
            return LvMax;
        }
        else if (variable == "power")
        {
            return Power;
        }
        else if (variable == "penetration")
        {
            return PenetrationPower;
        }
        else
        {
            Debug.LogError("No se encontro la variable");
        }
        return -1000000f;
    }
    public override float[] getAllAttributesValues()
    {
        float[] variables = new float[2];
        variables[0] = Lv;
        variables[1] = power;
        variables[2] = penetrationPower;
        return variables;
    }
    public override string getLvText(string text)
    {
        if (text == "title")
        {
            return TitleWeapon;
        }
        else if (text == "body")
        {
            return nextLvText;
        }
        else
        {
            throw new InvalidOperationException("Condition was false. Cannot return a value.");
        }
    }

    public override void lvUp()
    {
        if (lv == 1 || lv == 4)
        {
            lv++;
            penetrationPower++;
            NextLvText = "Aumento de la potencia de ataque del cañon principal";
        }else if (lv == 2 || lv == 3)
        {
            lv++;
            power++;
            NextLvText = "Aumento de la potencia de ataque del cañon principal";
        }
    }
    #endregion
    // Start is called before the first frame update
    private void Awake()
    { 
        bulletContainer = GameObject.FindGameObjectWithTag("BulletContainer");
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }
    void Start()
    {
        initializeVariables();
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale == 1)
        {
            if (isAlive)
            {
                HandleShooting();
            }
        }
    }

    //----------Iniciacion de las Variables---------
    private void initializeVariables()
    {
        Lv = 1;
        LvMax = 6;
        Power = 1f;
        PenetrationPower = 1;
        bulletSpeed = 8f;
        shootCooldown = 0;

        TitleWeapon = "Rifle Laser";
        NextLvText = "Las balas ahora atraviesan un enemigo mas";
    }

    //----------Funcion de disparo basico----------------
    private void HandleShooting()
    {
       /* if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }*/
        if (Input.GetMouseButton(0))
        {
            shootCooldown += Time.deltaTime;
            if(shootCooldown >= 0.25f)
            {
                Shoot();
                shootCooldown = 0;
            }
        }
    }
    private void Shoot()
    {
        audioManager.PlaySFX(audioManager.Shoot);

        //Creamos la bala y le damos las propiedades para interactuar con el juego.
        Rigidbody2D bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity, bulletContainer.transform);

        bullet.GetComponent<Bullet>().Damage = Power;
        bullet.GetComponent<Bullet>().Life = PenetrationPower;

        bullet.AddForce(bulletSpeed * transform.right, ForceMode2D.Impulse);
    }
}
