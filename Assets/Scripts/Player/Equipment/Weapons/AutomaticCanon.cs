using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutomaticCanon : Equipment
{
    [Header("Attribute")]
    [SerializeField] private int lv;
    [SerializeField] private int lvMax;
    [SerializeField] private float power;
    [SerializeField] private float cooldown;
    [SerializeField] private float penetrationPower;
    private float bulletSpeed;
    
    //Text Variables
    private string nextLvText;
    private string titleWeapon;
    
    //Control Variables
    private float timer;

    [Header("CallingObjects")]
    [SerializeField] private CircleCollider2D rangeWeapon;
    [SerializeField] private GameObject target;
    [SerializeField] private Transform aim;
    [SerializeField] private Collider2D[] collider2Ds;
    [SerializeField] private List<GameObject> listaEnemies;
    [SerializeField] private Rigidbody2D bulletPrefab;
    [SerializeField] private GameObject bulletContainer;


    #region Encapsulamiento de variables
    public int Lv { get => lv; set => lv = value; }
    public float PenetrationPower { get => penetrationPower; set => penetrationPower = value; }
    public float Cooldown { get => cooldown; set => cooldown = value; }
    public float Power { get => power; set => power = value; }
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
        else if (variable == "cooldown")
        {
            Cooldown += value;
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
        else if (variable == "cooldown")
        {
            return Cooldown;
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
        float[] variables = new float[1];
        variables[0] = Lv;
        variables[1] = Power;
        variables[2] = Cooldown;
        variables[3] = PenetrationPower;
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
        if (lv == 0)
        {
            NextLvText = "Aumento de la cadencia de disparo";
            lv++;
        }
        else if (lv == 1 || lv == 3 || lv == 5 || lv == 7 || lv == 9)
        {
            NextLvText = "Aumento de la potencia del disparo";
            lv++;
            cooldown -= 0.2f;
            if(lv == 3)
            {
                NextLvText = "Las balas atraviesan un enemigo mas";
            } 
        }
        else if (lv == 2 || lv == 6 || lv == 8)
        {
            NextLvText = "Aumento de la cadencia de disparo";
            lv++;
            power += 1f;
        }else if (lv == 4)
        {
            NextLvText = "Aumento de la cadencia de disparo";
            lv++;
            penetrationPower++;
        }
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        initializeVariables();
        initializeReferences();

    }

    // Update is called once per frame
    void Update()
    {
        attackBehavior();
    }

    //--------------Funciones de Inicializacion de las variables-------------
    private void initializeVariables()
    {
        Lv = 0;
        LvMax = 10;
        Cooldown = 2;
        Power = 1;
        PenetrationPower = 1;
        bulletSpeed = 8f;

        NextLvText = "Desbloqueo del cañon automatico que disparara a los asteoides cercanos";
        TitleWeapon = "Cañon Automatico";
    }
    private void initializeReferences()
    {
        bulletContainer = GameObject.FindGameObjectWithTag("BulletContainer");
        rangeWeapon = this.GetComponent<CircleCollider2D>();
        listaEnemies = new List<GameObject>();
        aim = transform.GetChild(0);
    }

    //------------------Funciones del comportamiento del arma----------------------
    private void attackBehavior()
    {
        if (lv >= 1)
        {
            timer += Time.deltaTime;
            if (timer >= Cooldown)
            {
                shoot();
                timer = 0;
            }
            detectEnemies();
        }
    }

    //-------------------------Funciones de disparo-------------------------
    private void shoot()
    {
        HandleShooting();
    }
    private void HandleShooting()
    {
        Rigidbody2D bullet = Instantiate(bulletPrefab, transform.position, aim.rotation, bulletContainer.transform);
        bullet.GetComponent<Bullet>().Damage = power;
        bullet.GetComponent<Bullet>().Life = penetrationPower;
        bullet.velocity = aim.right * bulletSpeed;
    }

    //---------------------Funciones de apuntado y deteccion de enemigos--------------

    //Esta funcion detectara a todos los collider dentro del area del caños automatico y los filtrarara
    // por enemigos targeteables.
    private void detectEnemies()
    {

        listaEnemies.Clear();
        float distanceEnemy = 0f;

        collider2Ds = Physics2D.OverlapCircleAll(rangeWeapon.bounds.center, rangeWeapon.radius);

        for (int i = 0; i < collider2Ds.Length; i++)
        {
            if (collider2Ds[i].tag == "Enemy" || collider2Ds[i].tag == "Asteroid")
            {
                listaEnemies.Add(collider2Ds[i].gameObject);
            }
        }

        for (int x = 0; x <= listaEnemies.Count - 1; x++)
        {
            if (distanceEnemy == 0 || distanceEnemy > Vector3.Distance(this.transform.position, listaEnemies[x].transform.position))
            {
                distanceEnemy = Vector3.Distance(this.transform.position, listaEnemies[x].transform.position);
                target = listaEnemies[x];
            }
        }
        setAiming();
    }

    //Esta funcion escogera al enemigo mas cercano para disparar
    private void setAiming()
    {
        if (target != null)
        {
            Vector2 direction = target.transform.position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            aim.rotation = Quaternion.Euler(0, 0, angle);
        }
        else { Debug.Log("No hay enemigo a la vista"); }
    }

}
