using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.IO;


// ==========================================
// 1. 저장할 데이터 구조 정의 (데이터 바구니)
// ==========================================
// ⚠️ 주의: 이 클래스는 MonoBehaviour를 상속받지 않습니다! (뒤에 : MonoBehaviour가 없음)
[System.Serializable]
public class GameData
{
    // 플레이어 데이터
    public Vector2 playerPosition;
    public int playerHP;
    public int playerMaxHP;
    public int playerGold;
    public int playerGoldMax;
    public int playerAttackPower;
    public int playerDefensePower;

    // 로그라이크 진행 데이터
    public int currentFloor;
    public List<string> defeatedEnemyIDs = new List<string>(); // 처치된 적들의 ID 목록
}


// ==========================================
// 2. 게임의 전체 상태와 세이브를 관리하는 매니저
// ==========================================
public class GameManager : MonoBehaviour
{
    // 어디서나 접근 가능한 싱글톤 인스턴스
    public static GameManager Instance { get; private set; }

    [Header("현재 게임 데이터")]
    // ⭐ 위에 정의한 GameData 클래스를 실시간으로 들고 있는 변수입니다.
    public GameData currentData = new GameData();

    [HideInInspector]
    public string currentBattleEnemyID; // 현재 전투 중인 적의 ID 백업용

    private string saveFilePath; // 세이브 파일이 저장될 경로

    private void Awake()
    {
        // 싱글톤 세팅 (씬이 바뀌어도 파괴되지 않게 함)
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // 기기별로 안전한 저장 경로 설정 (C드라이브 사용자 폴더나 모바일 내부 저장소 등)
            saveFilePath = Path.Combine(Application.persistentDataPath, "savefile.json");
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // --- 💾 세이브 로직 ---
    public void SaveGame()
    {
        // 저장하기 직전에 필드에 플레이어가 있다면 최신 정보를 currentData에 동기화
        PlayerController player = FindObjectOfType<PlayerController>();
        if (player != null)
        {
            player.SyncStatsToManager();
        }

        // currentData 내부의 모든 정보를 텍스트(JSON)로 변환
        string json = JsonUtility.ToJson(currentData, true);

        // 파일로 저장
        File.WriteAllText(saveFilePath, json);
        Debug.Log("게임 저장 완료! 경로: " + saveFilePath);
    }

    // --- 📂 로드 로직 ---
    public bool LoadGame()
    {
        // 세이브 파일이 존재하는지 확인
        if (File.Exists(saveFilePath))
        {
            // 파일 읽어오기
            string json = File.ReadAllText(saveFilePath);

            // 텍스트 데이터를 다시 유니티 변수(currentData)로 복구
            currentData = JsonUtility.FromJson<GameData>(json);
            Debug.Log("게임 불러오기 완료.");
            return true;
        }

        Debug.LogWarning("저장된 세이브 파일이 없습니다.");
        return false;
    }

    // --- ⚔️ 전투 진입 및 복귀 ---
    public void EnterBattle(string enemyID)
    {
        currentBattleEnemyID = enemyID;
        SceneManager.LoadScene("BattleScene"); // 인카운터 시 전투 씬으로 전환
    }

    public void ReturnToField()
    {
        // 전투에서 승리해 필드로 돌아올 때, 방금 싸운 적을 처치 목록에 추가
        if (!string.IsNullOrEmpty(currentBattleEnemyID))
        {
            currentData.defeatedEnemyIDs.Add(currentBattleEnemyID);
        }

        SceneManager.LoadScene("FieldScene"); // 다시 필드 씬으로 복귀
    }
}