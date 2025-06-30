using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class SaveData : MonoBehaviour
{
    private static readonly string SAVE_FOLDER = Application.dataPath + "/Saves/savedata.Json";
    public Achievement achievement = new Achievement();
    public bool testSave;
    public bool testLoad;

    public string player;
    public List<Achievement> playerList = new List<Achievement>();

    private void Update()
    {
        if (testSave)
        {
            saveToJson();
            testSave = false;
        }
        if (testLoad)
        {
            loadFromJson();
            testLoad = false;
        }
    }

    public void saveToJson()
    {
        string inventoryData = JsonUtility.ToJson(achievement);
        //System.IO.File.WriteAllText(SAVE_FOLDER, inventoryData);
    }

    public void loadFromJson()
    {
        Debug.Log("inload");
        string inventoryData = System.IO.File.ReadAllText(SAVE_FOLDER);
        Debug.Log(inventoryData);
        achievement = JsonUtility.FromJson<Achievement>(inventoryData);
        Debug.Log(achievement);
        Debug.Log("data loaded");
    }
}

[System.Serializable]
public class Achievement
{
    public int record;
    public int puntuation;
    public int enemiesDefeated;
    public int asteroidsDestroyed;
    public int wavesCleared;
    public int timesPlayed;
    public float timePlayed;
}
