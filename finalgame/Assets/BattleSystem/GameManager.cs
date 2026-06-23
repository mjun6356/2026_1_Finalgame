using UnityEngine;
using System.IO;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header(" 현재 게임 데이터 (구조체 연동)")]
    public GameData currentData;

    [Header(" 현재 전투 정보 (SO 직접 연동!)")]
    public string currentBattleSymbolID;     // 필드 몹 고유 주민번호 (전전단계 피드백 반영)
    public EnemyDataSO currentBattleEnemySO; //  이제 int ID 대신 SO를 직접 기억합니다!

    private string saveFilePath;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            saveFilePath = Path.Combine(Application.persistentDataPath, "savefile.json");
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // ---  세이브 로직 ---
    public void SaveGame()
    {
        //  유니티 6 최적화 표준 함수 적용 완료! (경고 제거)
        PlayerController player = FindFirstObjectByType<PlayerController>();

        if (player != null)
        {
            player.SyncStatsToManager();
        }

        // 데이터 저장
        string json = JsonUtility.ToJson(currentData, true);
        File.WriteAllText(saveFilePath, json);
        Debug.Log("게임 저장 완료! 경로: " + saveFilePath);
    }
}

