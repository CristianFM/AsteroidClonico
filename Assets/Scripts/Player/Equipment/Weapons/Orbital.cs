using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Orbital : Equipment
{
    [Header("Attribute")]
    [SerializeField] private int lv;
    [SerializeField] private int lvMax;
    [SerializeField] private float power;
    [SerializeField] private float speed;
    [SerializeField] private Quaternion lastParentRotaion;

    [SerializeField] private string nextLvText;
    [SerializeField] private string titleWeapon;

    [Header("CallingObjects")]
    [SerializeField] private List<GameObject> orbitalList;
    [SerializeField] private Transform orbitalContainer;

    [Header("Flags")]
    [SerializeField] private bool levelUpFlag;
    [SerializeField] private bool test;

    #region Encapsulamiento
    public int Lv { get => lv; set => lv = value; }
    public float Power { get => power; set => power = value; }
    public float Speed { get => speed; set => speed = value; }
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
        else if (variable == "speed")
        {
            Speed += value;
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
        else if (variable == "speed")
        {
            return Speed;
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
        variables[2] = Speed;
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
        if (lv == 0)
        {
            lv++;
            activateOrbitals();
            NextLvText = "Aumento de la velocidad de rotacion de los orbitales ";
        }
        else if (lv == 1 || lv == 3 || lv == 5 || lv == 7)
        {
            speed += 5;
            NextLvText = "Aumento del ataque de los orbitales y añade un dron más";
            lv++;
        }else if (lv == 2 || lv ==4 || lv== 6)
        {
            power += 1;
            NextLvText = "Aumento de la velocidad de rotacion de los orbitales";
            lv++;
            activateOrbitals();
        }
        #endregion
    }
    private void Awake()
    {
        orbitalContainer = GameObject.FindGameObjectWithTag("OrbitalContainer").transform;
    }
    // Start is called before the first frame update
    void Start()
    {
        lv = 0;
        lvMax = 8;
        power = 1;
        speed = 10;

        //recogemos todos los orbitales alojados dentro del Orbital Container para interactuar con ellos
        getOrbitalChilds();

        //establecemos los valores de texto del arma
        NextLvText = "Desbloquear Orbitales.\n Esferas que te seguiran y atacaran a los asteroides y enemigos";
        TitleWeapon = "Drones Orbitales";
    }

    // Update is called once per frame
    void Update()
    {
       
        if (test)
        {
            test = false;
            activateOrbitals();
        }
        orbitalContainer.position = transform.position;
        orbitalContainer.Rotate(0, 0, 10 * Time.deltaTime * speed);
    }
    
    private void activateOrbitals()
    {
        Debug.Log("ActivarOrbe");
        if (lv == 1)
        {
            orbitalList[0].SetActive(true);
        }
        if (lv == 3)
        {
            orbitalList[1].SetActive(true);
        }
        if (lv == 5)
        {
            orbitalList[2].SetActive(true);
        }
        if (lv == 7)
        {
            orbitalList[3].SetActive(true);
        }
    }
    //Recogemos los orbitales establecemos sus valores de combate
    private void getOrbitalChilds()
    {
        int maxIndex = orbitalContainer.childCount - 1;
        for (int i = 0; i <= maxIndex; i++)
        {
            orbitalList.Add(orbitalContainer.transform.GetChild(i).gameObject);
            orbitalList[i].gameObject.SetActive(false);
            orbitalList[i].GetComponent<OrbitalOrb>().Damage = power;
            orbitalList[i].GetComponent<OrbitalOrb>().Orbital = this;
        }
    }
    //esta icurrutina sirve para que una vez el orbe golpee un enemigo/asteroide desaparezca temporalmente
    //de esta manera no se convierte en un escudo infranqueable, y requiere repararse
    //luego de golpear a un asteroide
    public void orbReparation(GameObject obj)
    {
        StartCoroutine(reactivateOrb(obj));
    }
    private IEnumerator reactivateOrb(GameObject orb)
    {
        yield return new WaitForSeconds(1f);

        orb.gameObject.SetActive(true);

        yield return null;
    }

}
