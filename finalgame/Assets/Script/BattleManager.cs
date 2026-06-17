using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST }

public class BattleManager : MonoBehaviour
{
    public BattleState state;


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
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
