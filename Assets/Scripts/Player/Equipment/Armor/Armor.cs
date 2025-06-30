using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Armor : Equipment
{

    [Header("Attribute")]
    [SerializeField] private int lv;
    [SerializeField] private int lvMax;
    [SerializeField] private float life;
    [SerializeField] private float maxLife;
    [SerializeField] private float lifeGeneration;
    [SerializeField] private float timer;


    [SerializeField] private string nextLvText;
    [SerializeField] private string titleWeapon;

    #region Encapsulamiento
    public int Lv { get => lv; set => lv = value; }
    public float Life { get => life; set => life = value; }
    public float LifeGeneration { get => lifeGeneration; set => lifeGeneration = value; }
    public float MaxLife { get => maxLife; set => maxLife = value; }
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
        else if (variable == "life")
        {
            Life += value;
        }
        else if (variable == "maxLife")
        {
            MaxLife += value;
        }
        else if (variable == "lifeGeneration")
        {
            lifeGeneration += value;
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
        else if (variable == "life")
        {
            return Life;
        }
        else if (variable == "maxLife")
        {
            return MaxLife;
        }
        else if (variable == "lifeGeneration")
        {
            return LifeGeneration;
        }
        else
        {
            Debug.LogError("No se encontro la variable");
        }
        return -1000000f;
    }
    public override float[] getAllAttributesValues()
    {
        float[] variables = new float[3];
        variables[0] = Lv;
        variables[1] = life;
        variables[2] = MaxLife;
        variables[3] = LifeGeneration;
        return variables;
    }

    public override void lvUp()
    {
        if (lv == 1|| lv == 3 || lv == 5)
        {

            lv++;
            maxLife++;
            life++;
            updateLife();
        }
        else if (lv == 2 || lv == 4 || lv == 6)
        {
            lifeGeneration -= 2f;
            lv++;
        }

    }

    public override string getLvText(string text)
    {
        if(text == "title")
        {
            return TitleWeapon;
        }else if (text == "body")
        {
            return nextLvText;
        }
        else
        {
            throw new InvalidOperationException("Condition was false. Cannot return a value.");
        }
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        initializeVariable();

        //llamamos a esta funcion al comenzar para que la barra de vida se actualice con los valores estandar.
        updateLife();
    }

    // Update is called once per frame
    void Update()
    {
        //Control de la regeneracion de vida

        regenLife();
    }
    //---------------Inicializacion de Variables
    private void initializeVariable()
    {
        //numeric variables
        Lv = 1;
        LvMax = 7;
        Life = 2;
        MaxLife = 2;
        LifeGeneration = 15f;

        //text variables
        NextLvText = "Aumento de la vida maxima de la nave";
        TitleWeapon = "Blindaje";
    }

    //---------------Funciones de modificacion de la vida-----------------
    private void regenLife()
    {
        if (life < maxLife)
        {
            timer += Time.deltaTime;
            if (timer >= lifeGeneration)
            {
                gainHealth(1);
                timer = 0;
            }
        }
    }
    public void gainHealth(float amount)
    {
        life += amount;
        if (life > maxLife)
        {
            life = maxLife;
        }
        InGameMenuManager._instance.updateLifeBar(life);
    }
    public void loseHealth(float amount)
    {
        life -= amount;
        InGameMenuManager._instance.updateLifeBar(life);
    }

    //----------------Funciones para modificar la HUD---------------
    private void updateLife()
    {
        InGameMenuManager._instance.UpdateMaxLifeBar(MaxLife);
        InGameMenuManager._instance.updateLifeBar(Life);
    }
}
