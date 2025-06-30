using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.Rendering.DebugUI;

public class Shield : Equipment
{
    [Header("Attribute")]
    [SerializeField] private int lv;
    [SerializeField] private int lvMax;
    [SerializeField] private float cooldown;
    [SerializeField] private bool invulnerable;
    [SerializeField] private bool lvUpFlag;

    private string nextLvText;
    private string titleWeapon;

    [Header("variable de control")]
    [SerializeField] private float timer;

    [Header("Llamada de objetos")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private ParticleSystem shieldOut;


    #region Encapsulamiento de las variables
    public int Lv { get => lv; set => lv = value; }
    public float Cooldown { get => cooldown; set => cooldown = value; }
    public bool Invulnerable { get => invulnerable; set => invulnerable = value; }
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
        else if (variable == "cooldown")
        {
            Cooldown += value;
        }else if(variable == "lvMax")
        {
            lvMax += (int)value;
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
        else if (variable == "cooldown")
        {
            return Cooldown;
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
        variables[1] = Cooldown;
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
    #region lvUpBehavior
    public override void lvUp()
    {
        if (lv == 0)
        {
            lv++;
            activateShield();
            NextLvText = "Reduccion del tiempo de espera \n de reaparecion del escudo en 1 segundo";
        }
        else if (lv>0 && lv <=4)
        {
            lv++;
            cooldown--;
        }
    }
    #endregion
    #endregion

    void Start()
    {
        initializeVariables();
    }

    void Update()
    {
        shieldBehavior();
    }

    private void initializeVariables()
    {
        //variables numericas
        Lv = 0;
        lvMax = 5;
        Cooldown = 10f;

        //variables de control
        Invulnerable = false;

        //llamada a objetos
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = false;

        shieldOut = transform.GetChild(0).GetComponent<ParticleSystem>();

        //variables text
        NextLvText = "Desbloquear escudo.\n El escudo te otorgara proteccion frente a un golpe,\n" +
                " luego tendra que recargarse.";
        TitleWeapon = "Escudo de Energia";
    }

    #region EquipmentBehavior

    //controlador de los estado de los escudos
    private void shieldBehavior()
    {
        if (lv >= 1)
        {
            handleInvulnerability();
        }
        if (lvUpFlag)
        {
            lvUp();
            lvUpFlag = false;
        }
    }

    // funcion para activar el escudo
    private void activateShield()
    {
        invulnerable = true;
        spriteRenderer.enabled = true;
    }
    
    //cuando la invulnerabilidad se desactiva esta funcion con un contador espera el tiempo establecido
    //por el quipamiento para recargarlo y reactivarlo.
    private void handleInvulnerability()
    {
        if (invulnerable == false)
        {
            timer += Time.deltaTime;
            if (timer >= cooldown)
            {
                activateShield();
                timer = 0;
            }
        }
    }

    //--------------Icorrutinas-----------------
    //esta icorrutina la usamos para retrasar la destruccion del escudo y dar medio segundo al jugador para reaccionar
    //y volverlo vulnerable a los ataques.
    //Es llamada desde el escrip de Player que es el que gestiona las collisiones
    public IEnumerator delayInvulnerability()
    {
        yield return new WaitForSeconds(0.5f);

        invulnerable = false;
        spriteRenderer.enabled = false;
        shieldOut.Play();
        yield return null;
    }
    #endregion


}
