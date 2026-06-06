using UnityEngine;

public class EnemySymbol2D : MonoBehaviour
{
    [Header("연결할 몬스터 데이터 에셋")]
    public EnemyDataSO enemyData; // 👈 여기에 슬라임 데이터나 고블린 데이터 SO를 드래그앤드롭!

    [Header("심볼 고유 ID (맵 복귀 시 삭제용 주민번호)")]
    public string symbolID;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // 이미 잡은 적이라면 맵에서 삭제
        if (GameManager.Instance != null && GameManager.Instance.currentData.defeatedEnemyIDs.Contains(symbolID))
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerController player = collision.GetComponent<PlayerController>();
            if (player != null) player.SyncStatsToManager();

            // SO가 가진 도감 번호를 매니저에게 전달하면서 전투 진입
            if (enemyData != null)
            {
                GameManager.Instance.EnterBattle(symbolID, enemyData.enemyID);
            }
            else
            {
                Debug.LogError($"{gameObject.name}에 EnemyDataSO가 등록되지 않았습니다!");
            }
        }
    }
}
