using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class GameManager : MonoBehaviour
{
    public static GameManager _instance;

    [Header("Flags")]
    [SerializeField] private bool pauseIsTrigger;
    [SerializeField] private bool lvUpIsTrigger;
    [SerializeField] private SaveData saveData;

    [SerializeField] public int enemyDefeated;
    [SerializeField] public int asteroidDefeated;
    [SerializeField] private int waveCleared;
    [SerializeField] public float timer;

    public bool test;

    public SaveData SaveData { get => saveData; set => saveData = value; }

    public void Awake()
    {
        //Codigo para controlar la destruccion del GameObject
        if(_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        SaveData.loadFromJson();
    }
    private void Update()
    {
        if (test)
        {
            SaveData.loadFromJson();
            test = false;
        }
    }
    public void GameOver()
    {
        triggerTimeStop(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void triggerTimeStop(bool state)
    {
        if (!state)
        {
            Time.timeScale = 1;
        }
        if (state)
        {
            Time.timeScale = 0;
        }
    }
    public void IniciarPartida()
    {
        SceneManager.LoadScene("Game");
    }

    //-----------------------JSON Function----------------------

    public void addEnemyDefeated()
    {
        enemyDefeated++;
    }
    public void addAsteroidDefeated()
    {
        asteroidDefeated++;
    }
    public void waveCleareds()
    {
        waveCleared++;
    }
    public void timePlayed(float time)
    {
        timer = time;
    }
    public void updatePlayerAchievement()
    {
        int puntuacion = (int)InGameMenuManager._instance.Points;
        if (SaveData.achievement.record < puntuacion)
        {
            SaveData.achievement.record = puntuacion;
        }
        SaveData.achievement.puntuation += puntuacion;
        SaveData.achievement.enemiesDefeated += enemyDefeated;
        SaveData.achievement.asteroidsDestroyed += asteroidDefeated;
        SaveData.achievement.wavesCleared += waveCleared;
        SaveData.achievement.timesPlayed++;
        SaveData.achievement.timePlayed += timer;

        SaveData.saveToJson();
    }
    public void updatePlayerPrefAchievement()
    {
        int puntuacion = (int)InGameMenuManager._instance.Points;
        if (PlayerPrefs.GetFloat("record") < puntuacion)
        {
            PlayerPrefs.SetFloat("record", puntuacion);
        }
        PlayerPrefs.SetFloat("puntuacion", PlayerPrefs.GetFloat("puntuacion")+puntuacion);
        PlayerPrefs.SetFloat("enemyDefeated", PlayerPrefs.GetFloat("enemyDefeated") + enemyDefeated);
        PlayerPrefs.SetFloat("asteroidDefeated", PlayerPrefs.GetFloat("asteroidDefeated") + asteroidDefeated);
        PlayerPrefs.SetFloat("waveCleared", PlayerPrefs.GetFloat("waveCleared") + waveCleared);
        PlayerPrefs.SetFloat("timesPlayed", PlayerPrefs.GetFloat("timesPlayed") + 1);
        PlayerPrefs.SetFloat("timePlayed", PlayerPrefs.GetFloat("timePlayed") + timer);

    }
}
