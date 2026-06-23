using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using System;
using TMPro;

//public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST }

public class BattleManager : MonoBehaviour
{
    /* public BattleState state;

     public EnemyDataSO currentEnemy;

     [Header("UI 메뉴창")]
     public GameObject actionMenuUI; // 껐다 켰다 할 플레이어의 메뉴창 (버튼들이 들어있는 패널)

     // Start is called once before the first execution of Update after the MonoBehaviour is created
     void Start()
     {
         state = BattleState.START;
         StartCoroutine(SetupBattle());
     }


     IEnumerator SetupBattle()
     {
         // 1. 전투 시작 연출 (몬스터 등장, UI 초기화 등)
         actionMenuUI.SetActive(false); // 시작할 땐 메뉴를 꺼둡니다.

         Debug.Log("전투가 시작되었습니다!");

         // 씬 로딩이나 등장 애니메이션을 위해 잠시 대기
         yield return new WaitForSeconds(1.5f);

         // 2. 플레이어 턴으로 전환
         state = BattleState.PLAYERTURN;
         PlayerTurn();
     }

     void PlayerTurn()
     {
         Debug.Log("플레이어의 턴입니다. 행동을 선택하세요.");
         // 플레이어 턴이 오면 메뉴창을 켭니다.
         actionMenuUI.SetActive(true);
     }

     // [UI 공격 버튼의 OnClick 이벤트에 연결할 함수]
     public void OnAttackButton()
     {
         // 플레이어 턴이 아닐 때 버튼이 눌리는 것을 방지 (중복 클릭 방지)
         if (state != BattleState.PLAYERTURN) return;

         StartCoroutine(PlayerActionCoroutine());
     }

     IEnumerator PlayerActionCoroutine()
     {
         // 1. 플레이어가 행동을 선택했으므로 메뉴창을 잠시 끕니다.
         actionMenuUI.SetActive(false);

         // 2. 플레이어 공격 애니메이션 및 이펙트 실행
         Debug.Log("플레이어가 공격합니다!");

         //TODO: 여기에 만들어두신 스탯을 이용한 데미지 계산 로직 추가


         // 플레이어의 공격 연출이 끝날 때까지 대기 (예: 2초)
         // 나중에 애니메이션 길이에 맞춰서 이 시간을 조절하시면 됩니다.
         yield return new WaitForSeconds(2f);
         // 3. 적이 죽었는지 체크 (생략: 죽었다면 state = WON 및 전투 종료)

         // 4. 몬스터의 턴으로 넘깁니다.
         state = BattleState.ENEMYTURN;
         StartCoroutine(EnemyTurnCoroutine());
     }

     IEnumerator EnemyTurnCoroutine()
     {
         Debug.Log("몬스터의 턴입니다!");

         // 1. 몬스터 공격 애니메이션 및 이펙트 실행
         // TODO: 몬스터 AI 로직 및 데미지 계산 로직 추가

         // 몬스터의 공격 연출이 끝날 때까지 대기
         yield return new WaitForSeconds(2f);

         // 2. 플레이어가 죽었는지 체크 (생략: 죽었다면 state = LOST 및 전투 종료)

         // 3. 다시 플레이어 턴으로 돌아옵니다.
         state = BattleState.PLAYERTURN;
         PlayerTurn();
     }*/

    public static BattleManager Instance { get; private set; }

    public enum BattleState { PlayerMenu, PlayerAction, EnemyTurn, Won, Lost }
    [Header(" 상태 관리")]
    public BattleState currentState;

    //  굳이 List<EnemyDataSO> 데이터베이스를 들고 다니며 탐색할 필요가 없어졌습니다!
    private EnemyDataSO activeEnemySO;
    private int currentEnemyHP;
    private bool isSpareable;

    [Header(" 플레이어 HP UI 연결")]
    public Slider playerHPBar;
    public Text playerHPText;

    [Header(" 키보드 메뉴 네비게이션")]
    public GameObject[] menuButtons; // 0: FIGHT, 1: ACT, 2: ITEM, 3: MERCY
    public RectTransform soulUI;     // 선택창을 기어다닐 빨간 하트 이미지
    public Vector3 soulOffset = new Vector3(-45f, 0f, 0f); // 버튼 좌측 오프셋
    private int selectedButtonIndex = 0;

    [Header(" UI 오브젝트 연결")]
    public GameObject enemyTurnBox;      // 탄막 피하는 하얀 상자
    public GameObject menuButtonsParent; // 하단 버튼 4개 부모
    public AttackBar attackBar;
    public Image enemyImageUI;           //  화면 중앙 고정 적 이미지

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        // 1.  GameManager가 다이렉트로 넘겨준 스크립터블 오브젝트를 날로 먹기(?)
        if (GameManager.Instance != null && GameManager.Instance.currentBattleEnemySO != null)
        {
            activeEnemySO = GameManager.Instance.currentBattleEnemySO;
        }

        // 2. 몬스터 스탯 및 중앙 이미지 설정
        if (activeEnemySO != null)
        {
            currentEnemyHP = activeEnemySO.EnemyMaxHP;
            if (enemyImageUI != null && activeEnemySO.enemySprite != null)
            {
                enemyImageUI.gameObject.SetActive(true); // 턴이 바뀌어도 꺼지지 않고 중앙 상단 고정
                enemyImageUI.sprite = activeEnemySO.enemySprite;
            }
        }

        isSpareable = false;

        // 3. 선언하신 스탯 데이터에 맞춰 UI 초기 동기화
        UpdatePlayerHPUI();

        // 4. 전투 시작
        ChangeState(BattleState.PlayerMenu);
    }

    private void Update()
    {
        // 키보드 네비게이션 조작
        if (currentState == BattleState.PlayerMenu)
        {
            HandleMenuNavigation();
        }
    }

    private void HandleMenuNavigation()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            selectedButtonIndex--;
            if (selectedButtonIndex < 0) selectedButtonIndex = menuButtons.Length - 1;
            UpdateSoulPosition();
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            selectedButtonIndex++;
            if (selectedButtonIndex >= menuButtons.Length) selectedButtonIndex = 0;
            UpdateSoulPosition();
        }

        if (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.Return))
        {
            SelectMenuAction(selectedButtonIndex);
        }
    }

    private void UpdateSoulPosition()
    {
        if (soulUI != null && menuButtons != null && menuButtons.Length > 0)
        {
            soulUI.gameObject.SetActive(true);
            soulUI.position = menuButtons[selectedButtonIndex].transform.position + soulOffset;
        }
    }

    private void SelectMenuAction(int index)
    {
        switch (index)
        {
            case 0: OnFightSelected(); break;
            case 1: OnActSelected(); break;
            case 2: OnItemSelected(); break;
            case 3: OnMercySelected(); break;
        }
    }

    // ==========================================
    //  버튼 액션 및 데이터 실시간 연동
    // ==========================================
    private void OnFightSelected()
    {
        ChangeState(BattleState.PlayerAction);
        menuButtonsParent.SetActive(false);
        soulUI.gameObject.SetActive(false);

        if (attackBar != null)
        {
            attackBar.gameObject.SetActive(true);
            attackBar.StartAttack();
        }
    }

    // 미니게임 완료 시 어택바에서 데미지 배율을 받아와 실행할 함수
    public void ProcessPlayerAttack(float damageMultiplier)
    {
        if (GameManager.Instance == null || activeEnemySO == null) return;

        //  내 변수 연동: playerAttackPower 반영
        int pAtk = GameManager.Instance.currentData.playerAttackPower;
        int finalDamage = Mathf.RoundToInt(pAtk * damageMultiplier);

        currentEnemyHP -= finalDamage;
        Debug.Log($"{activeEnemySO.enemyName}에게 {finalDamage} 피해! 남은 체력: {currentEnemyHP}");

        if (currentEnemyHP <= 0)
        {
            currentEnemyHP = 0;
            StartCoroutine(EndBattleCo(isVictory: true)); // 승리 씬 복귀
        }
        else
        {
            // 체력이 30% 이하로 떨어지면 노란색 자비(Spare) 가능 상태로 유도
            if (currentEnemyHP <= activeEnemySO.EnemyMaxHP * 0.3f) isSpareable = true;
            StartCoroutine(WaitAndSwitchTurn(2f, BattleState.EnemyTurn));
        }
    }

    private void OnActSelected()
    {
        ChangeState(BattleState.PlayerAction);
        menuButtonsParent.SetActive(false);
        soulUI.gameObject.SetActive(false);

        if (activeEnemySO != null && activeEnemySO.actActionDialogue != null)
        {
            // DialogueManager.Instance.StartDialogue(activeEnemySO.actActionDialogue);
        }

        isSpareable = true;
        StartCoroutine(WaitAndSwitchTurn(2.5f, BattleState.EnemyTurn));
    }

    private void OnItemSelected()
    {
        if (GameManager.Instance == null) return;

        //  내 변수 연동: playerGold 검사 및 소모 후 playerHP 회복
        if (GameManager.Instance.currentData.playerGold >= 10)
        {
            ChangeState(BattleState.PlayerAction);
            GameManager.Instance.currentData.playerGold -= 10;
            GameManager.Instance.currentData.playerHP += 15;

            if (GameManager.Instance.currentData.playerHP > GameManager.Instance.currentData.playerMaxHP)
            {
                GameManager.Instance.currentData.playerHP = GameManager.Instance.currentData.playerMaxHP;
            }

            UpdatePlayerHPUI();
            Debug.Log("아이템 사용 완료! HP 15 회복.");
            StartCoroutine(WaitAndSwitchTurn(2f, BattleState.EnemyTurn));
        }
    }

    private void OnMercySelected()
    {
        ChangeState(BattleState.PlayerAction);
        menuButtonsParent.SetActive(false);
        soulUI.gameObject.SetActive(false);

        if (isSpareable)
        {
            StartCoroutine(EndBattleCo(isVictory: false)); // 살려주고 전투 종료
        }
        else
        {
            Debug.Log("자비 실패!");
            StartCoroutine(WaitAndSwitchTurn(2f, BattleState.EnemyTurn));
        }
    }

    // ==========================================
    //  상태 제어 및 필드 복귀 (자폭 메커니즘 연동)
    // ==========================================
    public void ChangeState(BattleState newState)
    {
        currentState = newState;

        switch (currentState)
        {
            case BattleState.PlayerMenu:
                enemyTurnBox.SetActive(false);
                menuButtonsParent.SetActive(true);
                UpdateSoulPosition();
                break;

            case BattleState.EnemyTurn:
                menuButtonsParent.SetActive(false);
                soulUI.gameObject.SetActive(false);
                if (enemyTurnBox != null) enemyTurnBox.SetActive(true);
                break;
        }
    }

    private IEnumerator EndBattleCo(bool isVictory)
    {
        if (isVictory)
        {
            currentState = BattleState.Won;
            // 승리 보상 골드 지급 (playerGold 연동)
            if (GameManager.Instance != null) GameManager.Instance.currentData.playerGold += 15;
        }

        yield return new WaitForSeconds(1.5f);

        if (GameManager.Instance != null)
        {
            GameManager.Instance.SaveGame(); // 몬스터 정보를 포함해 안전하게 세이브
        }

        SceneManager.LoadScene("FieldScene"); // 본인의 실제 월드 맵 씬 이름 입력
    }

    // ==========================================
    //  실시간 HP 피격 계산 및 실시간 UI 드로우
    // ==========================================
    public void TakeDamage()
    {
        if (GameManager.Instance == null || activeEnemySO == null) return;

        //  내 변수 연동: 적 공격력 - 내 방어력(playerDefensePower) 공식 계산
        int enemyAtk = activeEnemySO.EnemyAttackPower;
        int pDef = GameManager.Instance.currentData.playerDefensePower;

        int finalDamage = enemyAtk - pDef;
        if (finalDamage < 1) finalDamage = 1;

        GameManager.Instance.currentData.playerHP -= finalDamage;
        if (GameManager.Instance.currentData.playerHP < 0) GameManager.Instance.currentData.playerHP = 0;

        UpdatePlayerHPUI();

        if (GameManager.Instance.currentData.playerHP <= 0)
        {
            currentState = BattleState.Lost;
            Debug.Log("게임 오버");
        }
    }

    public void UpdatePlayerHPUI()
    {
        if (GameManager.Instance == null) return;

        //  내 변수 명 완벽 매칭 연동 완료
        int currentHP = GameManager.Instance.currentData.playerHP;
        int maxHP = GameManager.Instance.currentData.playerMaxHP;

        if (playerHPBar != null)
        {
            playerHPBar.maxValue = maxHP;
            playerHPBar.value = currentHP;
        }

        if (playerHPText != null)
        {
            playerHPText.text = $"{currentHP} / {maxHP}";
        }
    }

    private IEnumerator WaitAndSwitchTurn(float delay, BattleState nextState)
    {
        yield return new WaitForSeconds(delay);
        ChangeState(nextState);
    }
}

