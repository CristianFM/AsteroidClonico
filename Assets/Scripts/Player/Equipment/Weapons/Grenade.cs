using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : Equipment
{
    [Header("Attribute")]
    [SerializeField] private int lv;
    [SerializeField] private int lvMax;
    [SerializeField] private float power;
    [SerializeField] private int dispersion;
    [SerializeField] private float cooldown;
    private float shootForce = 5f;

    //variables de texto
    private string nextLvText;
    private string titleWeapon;

    [Header("Variables de control")]
    [SerializeField] private bool isActive;

    [Header("Objetos referencias")]
    [SerializeField] private GameObject grenadePrefab;



    #region Encapsulamiento
    public int Lv { get => lv; set => lv = value; }
    public float Power { get => power; set => power = value; }
    public int Range { get => dispersion; set => dispersion = value; }
    public int LvMax { get => lvMax; set => lvMax = value; }
    public string NextLvText { get => NextLvText1; set => NextLvText1 = value; }
    public string NextLvText1 { get => nextLvText; set => nextLvText = value; }
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
        else if (variable == "dispersion")
        {
            dispersion += (int)value;
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
        else if (variable == "range")
        {
            return Range;
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
        variables[1] = Power;
        variables[2] = Range;
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
            return NextLvText;
        }
        else
        {
            throw new InvalidOperationException("Condition was false. Cannot return a value.");
        }
    }
    public override void lvUp()
    {
        if(lv == 0)
        {
            isActive = true;
            InGameMenuManager._instance.triggerMissileImage();
            lv++;
            NextLvText = "Aumento de numero de balas tras la explosion";
        }else if(lv == 1 || lv == 3)
        {
            lv++;
            dispersion += 2;
            NextLvText = "Aumento de la potencia de la explosion";
        }
        else if (lv== 2 || lv == 4)
        {
            lv++;
            power = 1;
            NextLvText = "Aumento de numero de balas tras la explosion";
        }
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
       initializeVariables();
    }

    // Update is called once per frame
    void Update()
    {
        handleShooting();
    }

    //-----------Inicializacion de las variables-----------
    private void initializeVariables()
    {
        //numeric variables
        lv = 0;
        lvMax = 5;
        power = 1;
        dispersion = 6;
        cooldown = 5f;

        //Control Variables
        isActive = false;

        //text variables
        NextLvText = "Desbloqueo de la granada.";
        TitleWeapon = "Misil Granada";
    }

    //----------Funciones para manejar el disparao de la granada y el seteo de variabels del misil---------
    
    //con este scrip controlamos el cooldown para el disparo de la granada/misil
    private void handleShooting()
    {
        cooldown += Time.deltaTime;
        if (isActive && cooldown >= 5)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                Shoot();
                cooldown = 0;
            }
            InGameMenuManager._instance.handleMissileImage(true);
        }
        else { InGameMenuManager._instance.handleMissileImage(false); }
    }

    //con este script instanciamos el misil y lo preparamos poniendole valor a las variables
    private void Shoot()
    {
        GameObject grenade = Instantiate(grenadePrefab, transform.position, Quaternion.identity);

        Rigidbody2D rb = grenade.GetComponent<Rigidbody2D>();

        rb.AddForce(transform.right * shootForce, ForceMode2D.Impulse);
        grenade.GetComponent<Missile>().BulletCount = dispersion;
        grenade.GetComponent<Missile>().Power = power;
    }
}
