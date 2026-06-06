using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public class BattleManager : MonoBehaviour
{
    public static BattleManager Instance { get; private set; }

    public enum BattleState { PlayerMenu, PlayerAction, EnemyTurn, Win, Lose }
    [Header("현재 전투 상태")]
    public BattleState currentState;

    [Header("몬스터 데이터베이스")]
    // 에디터에서 우리가 만든 슬라임SO, 고블린SO 목록을 여기에 드래그해서 넣어둡니다.
    public List<EnemyDataSO> enemyDatabase;
    private EnemyDataSO activeEnemySO; // 이번 판에 매칭된 몬스터 SO

    [Header("실시간 전투 변수")]
    private int currentEnemyHP;
    private bool isSpareable = false;

    [Header("연결할 UI 스크립트/오브젝트")]
    public AttackBar attackBar;
    public GameObject enemyTurnBox;
    public GameObject menuButtonsParent;
    public Text battleLogText;

    private void Awake()
    {
        Instance = this;
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // 1. GameManager에 저장된 도감 번호로 데이터베이스에서 몬스터 찾기
        int targetID = GameManager.Instance.currentBattleEnemyID;
        activeEnemySO = enemyDatabase.Find(enemy => enemy.enemyID == targetID);

        // 만약 못 찾았다면 기본 첫 번째 몬스터로 예외 처리
        if (activeEnemySO == null && enemyDatabase.Count > 0) activeEnemySO = enemyDatabase[0];

        // 2. 실시간 변수 초기화 (SO 원본을 건드리지 않고 값만 복사)
        currentEnemyHP = activeEnemySO.maxHP;
        isSpareable = false;

        // 3. 첫 턴 시작
        ChangeState(BattleState.PlayerMenu);
    }

    public void ChangeState(BattleState newState)
    {
        currentState = newState;

        switch (currentState)
        {
            case BattleState.PlayerMenu:
                menuButtonsParent.SetActive(true);
                enemyTurnBox.SetActive(false);
                // SO에 적어둔 조우 대사 출력
                battleLogText.text = activeEnemySO.encounterText;
                break;

            case BattleState.PlayerAction:
                menuButtonsParent.SetActive(false);
                break;

            case BattleState.EnemyTurn:
                menuButtonsParent.SetActive(false);
                StartCoroutine(EnemyTurnRoutine());
                break;

            case BattleState.Win:
                GameManager.Instance.currentData.playerGold += 15;
                GameManager.Instance.ReturnToField();
                break;

            case BattleState.Lose:
                Debug.Log("게임 오버");
                break;
        }
    }

    // ==========================================
    // 🔘 4대 버튼 UI 이벤트
    // ==========================================

    // 1. FIGHT (공격)
    public void OnFightButtonClicked()
    {
        if (currentState != BattleState.PlayerMenu) return;
        ChangeState(BattleState.PlayerAction);
        battleLogText.text = "";
        attackBar.StartAttack();
    }

    public void ProcessPlayerAttack(float damageMultiplier)
    {
        int baseAtk = 15;
        int finalDamage = Mathf.RoundToInt(baseAtk * damageMultiplier);

        currentEnemyHP -= finalDamage;
        battleLogText.text = $"{activeEnemySO.enemyName}에게 {finalDamage}의 피해를 입혔다!";

        // 체력이 30% 이하로 떨어지면 노란색 이름(자비 가능) 상태로 변경
        if (currentEnemyHP <= activeEnemySO.maxHP * 0.3f && currentEnemyHP > 0)
        {
            isSpareable = true;
        }

        StartCoroutine(WaitAndSwitchTurn(2f, currentEnemyHP <= 0 ? BattleState.Win : BattleState.EnemyTurn));
    }

    // 2. ACT (행동)
    public void OnActButtonClicked()
    {
        if (currentState != BattleState.PlayerMenu) return;
        ChangeState(BattleState.PlayerAction);

        // SO에 지정된 행동 관찰 대사 출력
        battleLogText.text = activeEnemySO.actActionText;

        // 행동을 했으므로 자비가 가능하게 만듦
        isSpareable = true;

        StartCoroutine(WaitAndSwitchTurn(2.5f, BattleState.EnemyTurn));
    }

    // 3. ITEM (아이템)
    public void OnItemButtonClicked()
    {
        if (currentState != BattleState.PlayerMenu) return;

        if (GameManager.Instance.currentData.playerGold >= 5)
        {
            ChangeState(BattleState.PlayerAction);
            GameManager.Instance.currentData.playerGold -= 5;

            GameManager.Instance.currentData.playerHP += 20;
            if (GameManager.Instance.currentData.playerHP > GameManager.Instance.currentData.playerMaxHP)
                GameManager.Instance.currentData.playerHP = GameManager.Instance.currentData.playerMaxHP;

            battleLogText.text = "맛있는 사탕을 먹었다! 체력이 20 회복되었습니다.";
            StartCoroutine(WaitAndSwitchTurn(2f, BattleState.EnemyTurn));
        }
        else
        {
            battleLogText.text = "골드가 부족합니다!";
        }
    }

    // 4. MERCY (자비)
    public void OnMercyButtonClicked()
    {
        if (currentState != BattleState.PlayerMenu) return;
        ChangeState(BattleState.PlayerAction);

        if (isSpareable)
        {
            battleLogText.text = $"{activeEnemySO.enemyName}(을)를 놔주었다. (자비 성공)";
            StartCoroutine(WaitAndSwitchTurn(2.5f, BattleState.Win));
        }
        else
        {
            battleLogText.text = $"{activeEnemySO.enemyName}(은)는 아직 가고 싶지 않아 보인다. (자비 실패)";
            StartCoroutine(WaitAndSwitchTurn(2.5f, BattleState.EnemyTurn));
        }
    }

    // 👾 적 턴 루틴
    private IEnumerator EnemyTurnRoutine()
    {
        battleLogText.text = "";
        enemyTurnBox.SetActive(true);

        yield return new WaitForSeconds(5f); // 5초 버티기

        enemyTurnBox.SetActive(false);

        if (GameManager.Instance.currentData.playerHP <= 0)
            ChangeState(BattleState.Lose);
        else
            ChangeState(BattleState.PlayerMenu);
    }

    private IEnumerator WaitAndSwitchTurn(float delay, BattleState nextState)
    {
        yield return new WaitForSeconds(delay);
        ChangeState(nextState);
    }

    // 피격 처리 (SO의 공격력 데이터 활용)
    public void PlayerTakeDamage()
    {
        GameManager.Instance.currentData.playerHP -= activeEnemySO.attackPower;
        if (GameManager.Instance.currentData.playerHP < 0) GameManager.Instance.currentData.playerHP = 0;
    }
   
}
