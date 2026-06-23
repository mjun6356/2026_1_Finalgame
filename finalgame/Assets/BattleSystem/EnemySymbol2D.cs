using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public class EnemySymbol2D : MonoBehaviour
{
    public EnemyDataSO enemySO;
    public string symbolID;

    private void Start()
    {
        if (GameManager.Instance != null && GameManager.Instance.currentBattleSymbolID == this.symbolID)
        {
            Destroy(gameObject);
            GameManager.Instance.currentBattleSymbolID = string.Empty;
            GameManager.Instance.currentBattleEnemySO = null;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) EnterBattle();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) EnterBattle();
    }

    private void EnterBattle()
    {
        //  1. 씬 넘어가기 전에 플레이어의 최신 스탯과 위치를 GameManager에 백업!
        PlayerController player = FindFirstObjectByType<PlayerController>();
        if (player != null)
        {
            player.SyncStatsToManager();
        }

        //  2. 적 정보 백업!
        if (GameManager.Instance != null)
        {
            GameManager.Instance.currentBattleSymbolID = this.symbolID;
            GameManager.Instance.currentBattleEnemySO = this.enemySO;
        }

        SceneManager.LoadScene("BattleScene");
    }
}
