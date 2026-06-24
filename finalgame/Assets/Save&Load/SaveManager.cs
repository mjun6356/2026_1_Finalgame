using UnityEngine;
using System.IO;


public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance;

    string savePath;

    private void Awake()
    {
        Instance = this;

        savePath =
            Application.persistentDataPath +
            "/save.json";
    }

    public void SaveGame(
        int hp,
        int attack,
        int defense,
        int score,
        float time)
    {
        SaveData data = new SaveData();

        data.playerHP = hp;
        data.attackPower = attack;
        data.defense = defense;
        data.score = score;
        data.playTime = time;

        string json =
            JsonUtility.ToJson(data, true);

        File.WriteAllText(savePath, json);

        Debug.Log("저장 완료");
    }

    public SaveData LoadGame()
    {
        if (File.Exists(savePath))
        {
            string json =
                File.ReadAllText(savePath);

            SaveData data =
                JsonUtility.FromJson<SaveData>(json);

            Debug.Log("로드 완료");

            return data;
        }

        Debug.Log("저장 파일 없음");
        return null;
    }
}
