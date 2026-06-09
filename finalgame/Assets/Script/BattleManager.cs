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
    public Sprite enemyUI;

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
                enemyTurnBox.SetActive(false);

                // 1. 대사가 나오는 동안 하단 메뉴 버튼들을 잠시 숨깁니다.
                menuButtonsParent.SetActive(false);

                // 2. 내가 만든 다이얼로그 매니저를 호출해서 몬스터의 조우 다이얼로그를 재생합니다.
                // (※ 아래 코드는 본인의 다이얼로그 실행 함수 이름으로 바꾸셔야 합니다!)
                if (activeEnemySO.encounterDialogue != null)
                {
                    // 예시: DialogueManager.Instance.StartDialogue(activeEnemySO.encounterDialogue);

                    // [팁] 대사가 끝난 후 메뉴 버튼을 다시 켜주기 위해 코루틴이나 이벤트를 활용해야 합니다.
                    StartCoroutine(WaitForEncounterDialogueEnd());
                }
                else
                {
                    // 대사 SO가 비어있다면 바로 메뉴 보여주기
                    menuButtonsParent.SetActive(true);
                }
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

    // 3. 조우 대사가 끝날 때까지 기다렸다가 메뉴 버튼을 켜주는 헬퍼 코루틴 예시
    private IEnumerator WaitForEncounterDialogueEnd()
    {
        // 예시: 본인의 다이얼로그 시스템이 "나 지금 대사 띄우는 중이야"라는 bool 변수(isShowing)를 가지고 있다면 활용
        // while (DialogueManager.Instance.isShowing) { yield return null; }

        // 임시로 2초 뒤에 끝난다고 가정 (실제 다이얼로그 종료 시점과 연결해야 합니다)
        yield return new WaitForSeconds(2.5f);

        // 대사가 끝났으니 플레이어가 선택할 수 있게 메뉴 버튼 활성화!
        menuButtonsParent.SetActive(true);
    }

    // ==========================================
    // 🔘 4대 버튼 UI 이벤트
    // ==========================================

    // 1. FIGHT (공격)
    public void OnFightButtonClicked()
    {
        // 1. battleLogText가 연결되어 있을 때만 텍스트를 비웁니다 (다이얼로그 전환 시 에러 방지)
        if (battleLogText != null)
        {
            battleLogText.text = "";
        }

        // 2. 어택바가 제대로 연결되어 있는지 검사합니다.
        if (attackBar != null)
        {
            attackBar.StartAttack(); // 정상 실행
        }
        else
        {
            // 연결이 안 되었다면 콘솔창에 빨간색으로 경고를 띄웁니다.
            Debug.LogError("🚨 [오류] BattleManager 인스펙터 창에 'Attack Bar' 오브젝트가 연결되지 않았습니다! 드래그해서 넣어주세요.");
        }
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

        // 행동 대사 다이얼로그 SO 실행
        if (activeEnemySO.actActionDialogue != null)
        {
            // DialogueManager.Instance.StartDialogue(activeEnemySO.actActionDialogue);
        }

        isSpareable = true;

        // 행동 대사가 끝난 뒤(예: 2.5초 후) 적의 턴으로 넘어가도록 처리
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
