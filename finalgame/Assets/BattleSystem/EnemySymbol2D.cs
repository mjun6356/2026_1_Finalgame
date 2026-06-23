using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public class EnemySymbol2D : MonoBehaviour
{

    [Header(" 이 몹이 가질 스크립터블 오브젝트(데이터)")]
    public EnemyDataSO enemySO;

    [Header(" 고유 주민번호 (맵에 있는 몹마다 다르게 적으세요 ex: Mob_01, Mob_02)")]
    public string symbolID;

    private void Start()
    {
        // ==========================================
        //  [2. 전투 복귀 및 몹 삭제 로직]
        // ==========================================
        // 배틀이 끝나고 필드 씬이 새로 로드되었을 때 실행됩니다.
        if (GameManager.Instance != null)
        {
            // GameManager에 저장된 '방금 싸운 몹 주민번호'가 내 번호와 일치한다면?
            if (GameManager.Instance.currentBattleSymbolID == this.symbolID)
            {
                // 이미 처리된 몹이므로 필드에서 흔적도 없이 삭제!
                Destroy(gameObject);

                //  다음 전투를 위해 GameManager의 임시 배틀 정보는 깔끔하게 비워줍니다.
                GameManager.Instance.currentBattleSymbolID = string.Empty;
                GameManager.Instance.currentBattleEnemySO = null;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // ==========================================
        //  [1. 전투 입장 로직]
        // ==========================================
        // 플레이어 오브젝트와 부딪혔을 때만 작동 (플레이어 태그가 "Player"여야 합니다)
        if (collision.gameObject.CompareTag("Player"))
        {
            if (GameManager.Instance != null)
            {
                // 1. 내 주민번호와 스크립터블 오브젝트를 GameManager에 백업!
                GameManager.Instance.currentBattleSymbolID = this.symbolID;
                GameManager.Instance.currentBattleEnemySO = this.enemySO;

                // (선택사항) 부딪힌 시점의 플레이어 좌표를 기억하고 싶다면 세이브 데이터에 저장 가능
                // GameManager.Instance.currentData.lastFieldPositionX = collision.transform.position.x;
                // GameManager.Instance.currentData.lastFieldPositionY = collision.transform.position.y;
            }

            // 2. 배틀 씬으로 전환! (본인의 실제 배틀 씬 이름으로 적으세요)
            SceneManager.LoadScene("BattleScene");
        }
    }


}
