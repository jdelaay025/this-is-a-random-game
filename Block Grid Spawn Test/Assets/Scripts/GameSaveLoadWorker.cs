using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSaveLoadWorker : MonoBehaviour
{
    #region Global Variable Declaration

    public static GameSaveLoadWorker Instance;

    public string playerGameSaveFilename = string.Empty;
    public string playerStreamerGameSaveFilename = string.Empty;

    public float timeToStartSaving = 2f;

    float saveTimer = 0f;
    public float saveTimeCap = 3f;

    #endregion

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }

        playerGameSaveFilename = Application.persistentDataPath + "//playerInfo.dat";
    }
    void Start()
    {

    }
    void Update()
    {
        if(saveTimer > 0f)
        {
            saveTimer -= Time.deltaTime;
        }
    }


    void OnApplicationFocus(bool focus)
    {
        if (TapTapScript.Instance.readyToPlay &&
            Time.time > timeToStartSaving && saveTimer <= 0f)
        {
            if (focus)
            {
                PauseManager.Instance.ClosePauseMenu();

                LoadGame();

                TapTapScript.Instance.SetUpReturnMessage();

                saveTimer = saveTimeCap;

                //Debug.Log("returned");
            }
            else
            {
                SaveGame();

                //Debug.Log("left");
            }
        }
    }

    public void RetrieveFloatKey(string name)
    {
        PlayerPrefs.GetFloat(name);
    }
    public void RetrieveIntKey(string name)
    {
        PlayerPrefs.GetInt(name);
    }
    public void RetrieveStringKey(string name)
    {
        PlayerPrefs.GetString(name);
    }

    public void UpdateKey(string name, float num)
    {
        PlayerPrefs.SetFloat(name, num);
    }
    public void UpdateKey(string name, int num)
    {
        PlayerPrefs.SetInt(name, num);
    }
    public void UpdateKey(string name, string item)
    {
        PlayerPrefs.SetString(name, item);
    }

    public void SaveGame()
    {
        if (TapTapScript.Instance.dontSave == false)
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Create(playerGameSaveFilename);

            PlayerData data = new PlayerData();
            data.lastGameEnded = DateTime.Now;
            data.totalMoney = TapTapScript.Instance.totalMoney;
            data.currentCostNumber = TapTapScript.Instance.CurrentLevelCost;
            data.currentValueNumber = TapTapScript.Instance.currentValueIncrease;
            data.currentLevel = TapTapScript.Instance.currentLevel;
            data.currentMoneyIncrease = TapTapScript.Instance.currentMoneyIncrease;
            data.whichAxe = TapTapScript.Instance.whichAxe;
            data.whichStep = TapTapScript.Instance.whichStep;
            data.whichStepNum = TapTapScript.Instance.whichStepNum;
            data.hasManager = TapTapScript.Instance.hasManager;
            data.totalClicks = TapTapScript.Instance.totalClicks;
            data.currentNumOfStrikes = TapTapScript.Instance.currentNumOfStrikes;
            data.currentStreamerGroup = TapTapScript.Instance.currentStreamerGroup;
            data.currentAxePrice = TapTapScript.Instance.currentAxePrice;
            data.upgradeIncomePrice = TapTapScript.Instance.upgradeIncomePrice;
            data.mineSpeedMultiplier = TapTapScript.Instance.mineSpeedMultiplier;
            data.firstTime = TapTapScript.Instance.firstTime;
            data.gameStartingPoint = TapTapScript.Instance.gameStartingTime;

            bf.Serialize(file, data);
            file.Close();

            //Debug.Log("Game Saved!!!");
        }
        else
        {
            Debug.Log("Can't save because you told us not to!!");
        }
    }
    public void LoadGame()
    {
        if (File.Exists(playerGameSaveFilename))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(playerGameSaveFilename, FileMode.Open);

            PlayerData data = (PlayerData)bf.Deserialize(file);
            file.Close();

            TapTapScript.Instance.currentStreamerGroup = data.currentStreamerGroup;
            TapTapScript.Instance.lastEndingTime = data.lastGameEnded;
            TapTapScript.Instance.totalMoney = data.totalMoney;
            TapTapScript.Instance.CurrentLevelCost = data.currentCostNumber;
            TapTapScript.Instance.currentValueIncrease = data.currentValueNumber;
            TapTapScript.Instance.currentLevel = data.currentLevel;
            TapTapScript.Instance.currentMoneyIncrease = data.currentMoneyIncrease;
            TapTapScript.Instance.whichAxe = data.whichAxe;
            TapTapScript.Instance.whichStep = data.whichStep;
            TapTapScript.Instance.whichStepNum = data.whichStepNum;
            TapTapScript.Instance.hasManager = data.hasManager;
            TapTapScript.Instance.totalClicks = data.totalClicks;
            TapTapScript.Instance.currentNumOfStrikes = data.currentNumOfStrikes;
            TapTapScript.Instance.currentAxePrice = data.currentAxePrice;
            TapTapScript.Instance.upgradeIncomePrice = data.upgradeIncomePrice;
            TapTapScript.Instance.mineSpeedMultiplier = data.mineSpeedMultiplier;
            TapTapScript.Instance.firstTime = data.firstTime;
            TapTapScript.Instance.gameStartingTime = data.gameStartingPoint;

            TapTapScript.Instance.SetAxeType();
            TapTapScript.Instance.CalculatePrice();
        }
        else
        {
            Debug.Log("Sorry, nothing to Load bro. We'll start it now");
        }
    }
    public void DeleteGameSave()
    {
        if (File.Exists(playerGameSaveFilename))
        {
            File.Delete(playerGameSaveFilename);
        }

        TapTapScript.Instance.dontSave = true;
    }

}

[Serializable]
public class PlayerData
{
    public string currentStreamerGroup;
    public DateTime gameStartingPoint;
    public DateTime lastGameEnded;
    public double totalMoney;
    public int currentCostNumber;
    public int currentValueNumber;
    public int currentLevel;
    public double currentMoneyIncrease;
    public string whichAxe;
    public int currentAxePrice = 0;
    public string whichStep;
    public int whichStepNum;
    public bool hasManager;
    public int totalClicks;
    public int currentNumOfStrikes;
    public double upgradeIncomePrice;
    public float mineSpeedMultiplier;
    public bool firstTime;
}