using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Booster : Equipment
{
    [Header("Attribute")]
    [SerializeField] private int lv;
    [SerializeField] private int lvMax;
    [SerializeField] private float velocity;

    [SerializeField] private string nextLvText;
    [SerializeField] private string titleWeapon;

    #region Encapsulamiento
    public int Lv { get => lv; set => lv = value; }
    public float Velocity { get => velocity; set => velocity = value; }
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
        else if (variable == "velocity")
        {
            Velocity += value;
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
            return LvMax ;
        }
        else if (variable == "velocity")
        {
            return Velocity;
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
        variables[1] = velocity;
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
        if(lv <= 5)
        {
            velocity += 2f;
            lv++;
        }
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        InitializeVariable();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void InitializeVariable()
    {
        //variables numericas
        lv = 1;
        lvMax = 5;
        velocity = 4f;

        //variables text
        NextLvText = "Aumento de la velocidad de movimiento";
        TitleWeapon = "Aceleradores";
    }
}
