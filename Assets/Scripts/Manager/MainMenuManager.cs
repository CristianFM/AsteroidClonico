using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MainMenuManager : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject controlMenu;
    public GameObject recordMenu;

    public Achievement achievement;

    public List<TextMeshProUGUI> textList;

    // Start is called before the first frame update
    private void Awake()
    {
        controlMenu.SetActive(false);
        recordMenu.SetActive(false);
    }
    void Start()
    {
        achievement = GameManager._instance.SaveData.GetComponent<SaveData>().achievement;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void startNewGame()
    {
        GameManager._instance.IniciarPartida();
    }
    public void exitGame()
    {
        Debug.Log("exit");
        Application.Quit();
    }

    public void openControlInterface()
    {
        mainMenu.SetActive(false);
        controlMenu.SetActive(true);
        recordMenu.SetActive(false);
    }
    public void openMainMenu()
    {
        mainMenu.SetActive(true);
        controlMenu.SetActive(false);
        recordMenu.SetActive(false);
    }
    public void openRecordMenu()
    {
        recordText();
        recordMenu.SetActive(true);
        controlMenu.SetActive(false);
        mainMenu.SetActive(false);
    }
    private string cronometro(float time)
    {
        float minutes = Mathf.FloorToInt(time / 60);
        float seconds = Mathf.FloorToInt(time % 60);

        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }
    private void recordText()
    {
        textList[0].text = PlayerPrefs.GetFloat("record").ToString();
        textList[1].text = PlayerPrefs.GetFloat("puntuacion").ToString();

        Debug.Log(PlayerPrefs.GetFloat("asteroidDestroyed").ToString());
        textList[2].text = PlayerPrefs.GetFloat("asteroidDefeated").ToString();

        Debug.Log(PlayerPrefs.GetFloat("enemiesDestroyed").ToString());
        textList[3].text = PlayerPrefs.GetFloat("enemyDefeated").ToString();
        textList[4].text = PlayerPrefs.GetFloat("waveCleared").ToString();
        textList[5].text = cronometro(PlayerPrefs.GetFloat("timePlayed"));
        textList[6].text = PlayerPrefs.GetFloat("timesPlayed").ToString();

    }
}
