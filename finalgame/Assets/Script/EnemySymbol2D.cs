using UnityEngine;

public class EnemySymbol2D : MonoBehaviour
{
    [Header("심볼 고유 ID (맵에서 삭제할 때 사용)")]
    public string symbolID; // ⚠️ 인스펙터에서 적마다 다르게 적어주세요 (ex: Mob_01, Mob_02)

    [Header("전투 데이터 ID (전투 씬에서 소환할 적 종류)")]
    public int enemyID;     // ⚠️ 도감 번호 (ex: 1번은 슬라임, 2번은 고블린)

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
            if (player != null)
            {
                player.SyncStatsToManager(); // 현재 플레이어 스탯 백업
            }

            //  두 가지 ID를 모두 매니저에게 던져줍니다!
            GameManager.Instance.EnterBattle(symbolID, enemyID);
        }
    }
}
