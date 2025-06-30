using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using TMPro;

public class UpgradeManager : MonoBehaviour
{
    //Static Variable
    public static UpgradeManager _upgradeInstance;

    [Header("Variables de control")]
    [SerializeField] private bool lvUpMenuFlag;
    [SerializeField] private bool lvUpFlag;

    [Header("Calling Objects")]
    [SerializeField] private GameObject parentWeapon;
    [SerializeField] private GameObject LvUpMenu;
    [SerializeField] private List<GameObject> LvUpCards;


    [Header("ListEquipment")]
    [SerializeField] private List<GameObject> equipmentList;
    [SerializeField] private List<GameObject> randomSelection;
    [SerializeField] private List<GameObject> filteredList;

    public bool LvUpMenuFlag { get => lvUpMenuFlag; set => lvUpMenuFlag = value; }

    private void Awake()
    {
        // Static Behavior
        if (_upgradeInstance == null)
        {
            _upgradeInstance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        #region Montar lista de equipamientos
        parentWeapon = GameObject.FindGameObjectWithTag("Equipment");

        foreach (Transform child in parentWeapon.transform)
        {

            equipmentList.Add(child.gameObject);

        }
        #endregion
    }
    private void Start()
    {
        LvUpMenu = GameObject.FindGameObjectWithTag("LvUpMenu");
        LvUpMenuFlag = true;

        getCanvasPanelLv();
        triggerLvUpMenu();
        emptyTextLvUp();
    }
    private void Update()
    {
        //Booleano de control para pruebas
        if (lvUpFlag)
        {
            lvUp();
            lvUpFlag = false;
        }
    }

    //Tenemos esta funcion para poder recoger cualquier equipamiento del jugador desde cualquier clase
    public GameObject getEquipment(string name)
    {
        for (int i = 0; i <= equipmentList.Count - 1; i++)
        {
            if (equipmentList[i].name == name)
            {
                return equipmentList[i];
            }
        }
        return null;
    }


    //-----------------------LvBehavior------------------------

    #region LvUpLogic
    // funcion principal de la clase que gestiona la mejora de equipamiento al acabar la fase.
    public void lvUp()
    {
        GameManager._instance.triggerTimeStop(true);
        triggerLvUpMenu(); // aparecion del menu de mejoras.
        selectRandomWeapon(); // filtrado de mejoras.
        setTextLvUp(); // carga de las tarjetas de lv up.
    }

    //la locura de seleccion de equipamiento 100% mas optimizable, pero bueno me salio asi de primeras.
    private void selectRandomWeapon()
    {
        // en primer lugar vaciamos las listas para que no hayan conflictos
        filteredList.Clear();
        randomSelection.Clear();

        //seguidamente recorremos la lista buscando que equipamientos estan a maximo nivel para descartarlos de la nueva lista
        for (int i = 0; i <= equipmentList.Count - 1; i++)
        {
            float lvMax = equipmentList[i].GetComponent<Equipment>().getAttributesValues("lvMax");
            float lv = equipmentList[i].GetComponent<Equipment>().getAttributesValues("lv");
            if(lv < lvMax)
            {
                filteredList.Add(equipmentList[i]);
            }
        }

        //a partir de aqui montamos la lista con las tres mejoras que vamos a poder elegir asegurandonos de que no se repitan
        //entre ellas.

        //Primero revisamos si quedan pocas mejoras disponibles para poner texto en las tarjetas correspondientes.
        //En caso negativo desactivamos el menu de mejoras de nivel y activamos uno que dice que ya no te quedan mejoras dispoble,
        // para que continues el juego.
        if (filteredList.Count == 0)
        {
            Debug.Log("No quedan mejoras");
            LvUpMenu.SetActive(false);
            InGameMenuManager._instance.triggerEmptyLvUp();
        }
        else if (filteredList.Count <= 2) // Sin embargo si quedan 2 o menos si podemos seguir cargando informacion en lar tarjetas.
        {
            for (int x = 0; x <= filteredList.Count - 1;) // esta vez cargaremos lo que haya en la lista filtrada
            {
                int index = Random.Range(0, filteredList.Count);
                if (!randomSelection.Contains(filteredList[index]))
                {
                    randomSelection.Add(filteredList[index]);
                    filteredList.RemoveAt(index);
                    x++;
                }
            }
        }else
        {
            //este se ejecutara en caso de que se hayan podido cargar tres equipamientos y el procedimiento seguira normalmente
            for (int x = 0; x < 3;)
            {
                int index = Random.Range(0, filteredList.Count);
                if (!randomSelection.Contains(filteredList[index]))
                {
                    randomSelection.Add(filteredList[index]); // y finalmente aqui tenemos la lista con las mejoras disponibles para cargar.
                    filteredList.RemoveAt(index);
                    x++;
                }
            }
        }
    }

    #endregion

    //------------------------LvUpMenu----------------------------
    #region LvUpMenu

    //Esta funcion cogera las cartas de mejoras vacias y las pondra en una lista para su manipulacion posterior
    private void getCanvasPanelLv()
    {
        foreach (Transform child in LvUpMenu.transform)
        {

            LvUpCards.Add(child.gameObject);

        }
    }

    //esta funcion hara aparecer y desaparecer el menu de mejoras de nivel
    public void triggerLvUpMenu()
    {
        if (LvUpMenuFlag == true)
        {
            emptyTextLvUp();
            LvUpMenu.SetActive(false);
            LvUpMenuFlag = false;
            GameManager._instance.triggerTimeStop(false);
        }
        else if (LvUpMenuFlag == false)
        {
            LvUpMenu.SetActive(true);
            LvUpMenuFlag = true;
        }
    }

    //esta funcion cargara las tarjetas con la informacion obtenida de las funciones de la seccion LVBehavior
    private void setTextLvUp()
    {
        for (int i = 0; i <= randomSelection.Count - 1; i++)
        {
            LvUpCards[i].GetComponentsInChildren<TextMeshProUGUI>()[0].text = randomSelection[i].GetComponent<Equipment>().getLvText("title");
            LvUpCards[i].GetComponentsInChildren<TextMeshProUGUI>()[1].text = randomSelection[i].GetComponent<Equipment>().getLvText("body");
        }
    }
    
    //Esta funcion vaciara la informacion de las tarjetas una vez hayas elegido una 
    private void emptyTextLvUp()
    {
        for (int i = 0; i <= randomSelection.Count - 1; i++)
        {
            LvUpCards[i].GetComponentsInChildren<TextMeshProUGUI>()[0].text = "";
            LvUpCards[i].GetComponentsInChildren<TextMeshProUGUI>()[1].text = "";
        }
    }

    //estas funciones nos ayudaran a obtener la mejora en base a la ventana que eleguigamos en el menu
    // y tambien a controlar que si quedan pocas mejoras no rellene las tarjetas, y desactiven el subir de nivel
    //de la tarjeta vacia.
    public void selectUpgrade1()
    {
        if (randomSelection.Count >= 1)
        {
            randomSelection[0].GetComponent<Equipment>().lvUp();
            triggerLvUpMenu();
        }
    }
    public void selectUpgrade2()
    {
        if (randomSelection.Count >= 2)
        {
            randomSelection[1].GetComponent<Equipment>().lvUp();
            triggerLvUpMenu();
        }
    }
    public void selectUpgrade3()
    {
        if (randomSelection.Count == 3)
        {
            randomSelection[2].GetComponent<Equipment>().lvUp();
            triggerLvUpMenu();
        }
    }
    #endregion

}
