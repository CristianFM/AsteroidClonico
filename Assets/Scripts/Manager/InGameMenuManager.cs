using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InGameMenuManager: MonoBehaviour
{
    //Static Variable.
    public static InGameMenuManager _instance;

    //Ventanas de interfaz.
    [Header("Interface Windows")]
    [SerializeField] private GameObject menuPausa;
    [SerializeField] private GameObject menuGameOver;
    [SerializeField] private GameObject emptyUpgrades;

    //Elementos de interfaz.
    [Header("Interface Elements")]
    [SerializeField] private Slider healthSlider;
    [SerializeField] private TextMeshProUGUI puntuation;
    [SerializeField] private TextMeshProUGUI time;
    [SerializeField] private GameObject missileImage;

    //Variables de control.
    private bool isPaused;

    //Variables de valores.
    private float timer, seconds, minutes;
    private float points;

    #region Encapsulamiento de las variables
    public GameObject MenuPausa { get => menuPausa; set => menuPausa = value; }
    public GameObject EmptyUpgrades { get => emptyUpgrades; set => emptyUpgrades = value; }
    public GameObject MissileImage { get => missileImage; set => missileImage = value; }
    public GameObject MenuGameOver { get => menuGameOver; set => menuGameOver = value; }
    public Slider HealthSlider { get => healthSlider; set => healthSlider = value; }
    public TextMeshProUGUI Puntuation { get => puntuation; set => puntuation = value; }
    public TextMeshProUGUI Time { get => time; set => time = value; }
    public float Points { get => points; set => points = value; }
    #endregion

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        initializeWindowsInterface();
    }

    void Update()
    {
        handlePausemenu();
        Cronometro();
    }


    #region Interface In Game

    //-----------------Funciones referentes a la interfaz in game (HUD)------------------
    //Estas Funciones sirven para gestionar las variables en la HUD Superiors
    public void updateLifeBar(float life)
    {
        HealthSlider.value = life;
    }
    public void UpdateMaxLifeBar(float life)
    {
        HealthSlider.maxValue = life;
    }
    public void addPoints(float value)
    {
        Points += value;
        Puntuation.text = Points.ToString().PadLeft(4, '0');
    }

    //--------------------------Cronometro------------------------
    //Funcion para actualizar el cronometro
    private void Cronometro()
    {
        timer += UnityEngine.Time.deltaTime;
        minutes = Mathf.FloorToInt(timer / 60);
        seconds = Mathf.FloorToInt(timer % 60);

        Time.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    //--------------------------Misil/Granada------------------------
    //Con estas funciones gestionamos la interfaz de la granada para que se visualice
    //cuando puedes y cuando no lanzarla.
    public void triggerMissileImage()
    {
        MissileImage.SetActive(true);
    }

    public void handleMissileImage(bool value)
    {
        if (value)
        {
            MissileImage.GetComponent<Image>().color = Color.white;
        }
        else 
        {
            MissileImage.GetComponent<Image>().color = Color.gray;
        }
    }
    #endregion

    //---------------------------Movimiento de ventana general-------------------
    //Seteamos todas las ventanas en false de base para poder jugar, y las activaremos
    // cuando nos convenga
    private void initializeWindowsInterface()
    {
        MenuPausa.SetActive(false);
        MenuGameOver.SetActive(false);
        EmptyUpgrades.SetActive(false);
        MissileImage.SetActive(false);
    }
    private void handlePausemenu()
    {
        //gestion del pause
        if (Input.GetKeyDown(KeyCode.Escape)) //Si pulsan la P pasaran dos cosas
        {
            if (isPaused) //si estÅEpausado y se presiona P, el juego continua
            {
                ReanudarJuego();
            }
            else //De lo contrario, pausa el juego 
            {
                PausarJuego();
            }
        }
    }
    public void PausarJuego()
    {
        MenuPausa.SetActive(true);
        GameManager._instance.triggerTimeStop(true);
        isPaused = true;
    }
    public void ReanudarJuego()
    {
        MenuPausa.SetActive(false);
        if (UpgradeManager._upgradeInstance.LvUpMenuFlag == false)
        {
            UnityEngine.Time.timeScale = 1f;
        }
        isPaused = false;
    }
    public void restart()
    {
        //GameManager._instance.updatePlayerAchievement();
        GameManager._instance.updatePlayerPrefAchievement();
        GameManager._instance.GameOver();
    }
    public void openGameOverMenu()
    {
        GameManager._instance.timePlayed(timer);
        StartCoroutine(triggerGameOVer());
    }
    public void VolverAlMenu()
    {
        GameManager._instance.updatePlayerPrefAchievement();
        UnityEngine.Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    //------------------LvUpMenu------------------

    //Funcion que lanza el LVup, o en este caso la adquisicion de mejoras que se ejecuta
    //cuando acaba una oleada
    public void triggerLvUp()
    {
        UpgradeManager._upgradeInstance.lvUp();
    }

    #region Funciones Interfaz/Menu
    public void triggerEmptyLvUp()
    {
        EmptyUpgrades.SetActive(true);
    }
    public void exitEmptyLvUp()
    {
        EmptyUpgrades.SetActive(false);
        GameManager._instance.triggerTimeStop(false);
    }

    //-----------------------Icorrutinas -----------------------
    //Icorrutinas usadas para el manejo de interfaz
    private IEnumerator triggerGameOVer()
    {

        yield return new WaitForSeconds(2f);

        GameManager._instance.triggerTimeStop(true);
        MenuGameOver.SetActive(true);

        yield return null;
    }
    #endregion
}
