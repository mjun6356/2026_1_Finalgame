using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Overlays;
using UnityEngine;
using UnityEngine.SceneManagement;

[Serializable]
public class PlayerData
{
    public List<string> collectedItems = new List<string>();

    public int stage = 1;

}

public class GameDataManager : MonoBehaviour
{
    public static GameDataManager instance;

    public PlayerData playerData;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }


    public void SaveData(PlayerData playerData)
    {
        string fllePath = Application.persistentDataPath + "/Player_data.json";
        string json = JsonUtility.ToJson(playerData, true);
        System.IO.File.WriteAllText(fllePath, json);
        Debug.Log("게임 데이터 저장됨: " + json);

    }

    public PlayerData LoadData()
    {
        string fllePath = Application.persistentDataPath + "/Player_data.json";
        if (System.IO.File.Exists(fllePath))
        {
            string json =System.IO.File.ReadAllText(fllePath);
            PlayerData playerData = JsonUtility.FromJson<PlayerData>(json);
            Debug.Log("게임 데이터 로드됨: " + json);
            return playerData;

        }
        else
        {
            Debug.LogWarning("저장된 게임 데이터가 없습니다.");
            return new PlayerData();
        }
    }
    public void GameStart()
    {
        PlayerData playerData = LoadData();
        if(playerData == null)
        {
            playerData = new PlayerData();
            SceneManager.LoadScene("wouid");
        }
        else
        {
            SceneManager.LoadScene("would_" + playerData.stage);
        }
    }


    public void PlayerDead()
    {
        PlayerData playerData = LoadData();
        if (playerData != null)
        {

            playerData.stage = 1;

            foreach (string item in playerData.collectedItems.ToList())
            {
                if (UnityEngine.Random.Range(0,2) == 0)
                {
                    playerData.collectedItems.Remove(item);
                }
            }

            SaveData(playerData);

        }
        SceneManager.LoadScene("YOUDIED");
    }

    
}
