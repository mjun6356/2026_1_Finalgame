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

    private EnemyDataSO activeEnemySO;
    private int currentEnemyHP;
    private bool isSpareable;

    [Header(" 플레이어 UI (TextMeshPro)")]
    public TextMeshProUGUI playerHPText;

    [Header("전투 상황판 텍스트 (추가됨!)")]
    public TextMeshProUGUI battleLogText; //  여기에 "전투 시작!", "적에게 10 데미지!" 글씨가 뜹니다.

    [Header(" 키보드 메뉴 네비게이션")]
    public GameObject[] menuButtons;
    public RectTransform soulUI;
    public Vector3 soulOffset = new Vector3(-45f, 0f, 0f);
    private int selectedButtonIndex = 0;

    [Header(" UI 오브젝트 연결")]
    public GameObject menuButtonsParent;
    public Image enemyImageUI;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        if (GameManager.Instance != null)
        {
            Debug.Log($"[로그] GameManager에 저장된 SO 이름: {(GameManager.Instance.currentBattleEnemySO != null ? GameManager.Instance.currentBattleEnemySO.name : "❌ 없음(Null)")}");
        }
        if (enemyImageUI == null)
        {
            Debug.LogError("❌ [에러] BattleManager 인스펙터 창에 'Enemy Image UI' 칸이 비어있습니다!");
        }

        if (GameManager.Instance != null && GameManager.Instance.currentBattleEnemySO != null)
        {
            activeEnemySO = GameManager.Instance.currentBattleEnemySO;
        }

        if (activeEnemySO != null)
        {
            currentEnemyHP = activeEnemySO.maxHP;
            if (enemyImageUI != null && activeEnemySO.enemySprite != null)
            {
                enemyImageUI.gameObject.SetActive(true);
                enemyImageUI.sprite = activeEnemySO.enemySprite; //  이미지 띄우는 로직
            }
            UpdateLog($"{activeEnemySO.enemyName}이(가) 나타났다!");
        }

        isSpareable = false;

        UpdatePlayerHPUI();
        ChangeState(BattleState.PlayerMenu);
    }

    private void Update()
    {
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

    private void OnFightSelected()
    {
        ChangeState(BattleState.PlayerAction);
        menuButtonsParent.SetActive(false);
        if (soulUI != null) soulUI.gameObject.SetActive(false);

        if (GameManager.Instance == null || activeEnemySO == null) return;

        int finalDamage = GameManager.Instance.currentData.playerAttackPower;
        currentEnemyHP -= finalDamage;

        UpdateLog($"{activeEnemySO.enemyName}에게 {finalDamage}의 피해를 입혔다!"); // 🎯 피드백

        if (currentEnemyHP <= 0)
        {
            currentEnemyHP = 0;
            StartCoroutine(EndBattleCo(true, "적을 쓰러뜨렸다!"));
        }
        else
        {
            if (currentEnemyHP <= activeEnemySO.maxHP * 0.3f) isSpareable = true;
            StartCoroutine(WaitAndSwitchTurn(1.5f, BattleState.EnemyTurn));
        }
    }

    private void OnActSelected()
    {
        ChangeState(BattleState.PlayerAction);
        menuButtonsParent.SetActive(false);
        if (soulUI != null) soulUI.gameObject.SetActive(false);

        UpdateLog($"{activeEnemySO.enemyName}의 동태를 살핀다..."); //  피드백
        isSpareable = true;
        StartCoroutine(WaitAndSwitchTurn(1.5f, BattleState.EnemyTurn));
    }

    private void OnItemSelected()
    {
        if (GameManager.Instance == null) return;

        if (GameManager.Instance.currentData.playerGold >= 10)
        {
            ChangeState(BattleState.PlayerAction);
            GameManager.Instance.currentData.playerGold -= 10;
            GameManager.Instance.currentData.playerHP += 15;

            if (GameManager.Instance.currentData.playerHP > GameManager.Instance.currentData.playerMaxHP)
                GameManager.Instance.currentData.playerHP = GameManager.Instance.currentData.playerMaxHP;

            UpdatePlayerHPUI();
            UpdateLog("포션을 마셨다! 체력을 15 회복했다."); //  피드백
            StartCoroutine(WaitAndSwitchTurn(1.5f, BattleState.EnemyTurn));
        }
        else
        {
            UpdateLog("돈이 부족해서 아이템을 쓸 수 없다...");
        }
    }

    private void OnMercySelected()
    {
        ChangeState(BattleState.PlayerAction);
        menuButtonsParent.SetActive(false);
        if (soulUI != null) soulUI.gameObject.SetActive(false);

        if (isSpareable)
        {
            StartCoroutine(EndBattleCo(false, "자비를 베풀었다. 전투 종료!"));
        }
        else
        {
            UpdateLog("아직 자비를 베풀 수 없다!");
            StartCoroutine(WaitAndSwitchTurn(1.5f, BattleState.EnemyTurn));
        }
    }

    public void ChangeState(BattleState newState)
    {
        currentState = newState;

        switch (currentState)
        {
            case BattleState.PlayerMenu:
                menuButtonsParent.SetActive(true);
                UpdateSoulPosition();
                UpdateLog("당신의 턴. 무엇을 할까?");
                break;

            case BattleState.EnemyTurn:
                menuButtonsParent.SetActive(false);
                if (soulUI != null) soulUI.gameObject.SetActive(false);
                StartCoroutine(EnemyAttackRoutine());
                break;
        }
    }

    private IEnumerator EnemyAttackRoutine()
    {
        UpdateLog("적의 공격이 날아온다!");
        yield return new WaitForSeconds(1f);

        TakeDamage();

        if (currentState != BattleState.Lost)
        {
            yield return new WaitForSeconds(1.5f);
            ChangeState(BattleState.PlayerMenu);
        }
    }

    private IEnumerator EndBattleCo(bool isVictory, string endMessage)
    {
        currentState = BattleState.Won;
        UpdateLog(endMessage); //  승리/자비 피드백

        if (isVictory && GameManager.Instance != null)
            GameManager.Instance.currentData.playerGold += 15;

        yield return new WaitForSeconds(2f);

        if (GameManager.Instance != null) GameManager.Instance.SaveGame();
        SceneManager.LoadScene("FieldScene");
    }

    public void TakeDamage()
    {
        if (GameManager.Instance == null || activeEnemySO == null) return;

        int enemyAtk = activeEnemySO.attackPower;
        int pDef = GameManager.Instance.currentData.playerDefensePower;
        int finalDamage = enemyAtk - pDef;
        if (finalDamage < 1) finalDamage = 1;

        GameManager.Instance.currentData.playerHP -= finalDamage;
        if (GameManager.Instance.currentData.playerHP < 0) GameManager.Instance.currentData.playerHP = 0;

        UpdatePlayerHPUI();
        UpdateLog($"{finalDamage}의 데미지를 받았다!"); // 🎯 피드백

        if (GameManager.Instance.currentData.playerHP <= 0)
        {
            currentState = BattleState.Lost;
            UpdateLog("체력이 0이 되었다... 게임 오버!");
        }
    }

    public void UpdatePlayerHPUI()
    {
        if (GameManager.Instance == null) return;
        if (playerHPText != null)
            playerHPText.text = $"{GameManager.Instance.currentData.playerHP} / {GameManager.Instance.currentData.playerMaxHP}";
    }

    //  텍스트 UI를 바꿔주는 헬퍼 함수
    private void UpdateLog(string message)
    {
        if (battleLogText != null) battleLogText.text = message;
    }

    private IEnumerator WaitAndSwitchTurn(float delay, BattleState nextState)
    {
        yield return new WaitForSeconds(delay);
        ChangeState(nextState);
    }
}

