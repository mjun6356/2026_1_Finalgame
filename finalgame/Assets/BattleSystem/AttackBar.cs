using UnityEngine;
using UnityEngine.UI;


public class AttackBar : MonoBehaviour
{
    [Header("UI 요소 연결")]
    public RectTransform cursor;       // 움직이는 흰색 커서
    public RectTransform attackZone;   // 전체 어택 바 영역
    public RectTransform centerArea;   // 가운데 높은 데미지 영역 (이미지의 노란색/빨간색 구간)

    [Header("설정")]
    public float cursorSpeed = 500f;   // 커서 이동 속도
    public bool isAttacking = false;

    private float _halfBarWidth;

    private void Start()
    {
        _halfBarWidth = attackZone.rect.width / 2;
    }

    // 1. 공격 시작 (타이밍 바 활성화)
    public void StartAttack()
    {
        gameObject.SetActive(true);
        // 커서를 왼쪽 끝으로 초기화
        cursor.anchoredPosition = new Vector2(-_halfBarWidth, 0);
        isAttacking = true;
    }

    private void Update()
    {
        if (!isAttacking) return;

        // 2. 커서 오른쪽으로 이동
        cursor.Translate(Vector3.right * cursorSpeed * Time.deltaTime);

        // 커서가 바를 벗어나면 공격 실패로 처리
        if (cursor.anchoredPosition.x > _halfBarWidth)
        {
            EndAttack(0f); // 데미지 0
        }

        // 3. 플레이어 입력 감지 (예: Z키나 마우스 클릭)
        if (Input.GetKeyDown(KeyCode.Z) || Input.GetMouseButtonDown(0))
        {
            CalculateDamage();
        }
    }

    // 4. 데미지 계산
    private void CalculateDamage()
    {
        float cursorPos = cursor.anchoredPosition.x;
        float centerPos = centerArea.anchoredPosition.x;
        float centerWidth = centerArea.rect.width;

        // 가운데 지점과의 거리 계산
        float distanceFromCenter = Mathf.Abs(cursorPos - centerPos);
        float maxPossibleDistance = _halfBarWidth;

        // 거리에 따른 데미지 비율 계산 (가운데에 가까울수록 1.0f에 가까움)
        // 언더테일처럼 가운데 특정 범위 내에 들면 'CRITICAL' 판정을 줄 수도 있습니다.
        float damageMultiplier = 1.0f - (distanceFromCenter / maxPossibleDistance);

        // 예시: 정중앙 좁은 범위(critical 존)에 들면 보너스 데미지
        if (distanceFromCenter < (centerWidth / 2f))
        {
            damageMultiplier *= 1.5f; // 크리티컬 배율
            Debug.Log("Critical Hit!");
        }

        EndAttack(damageMultiplier);
    }

    private void EndAttack(float multiplier)
    {
        isAttacking = false;
        gameObject.SetActive(false);

        // GameManager나 Enemy 스크립트에 최종 데미지 전달
        // finalDamage = 플레이어 기본 공격력 * multiplier
        Debug.Log($"공격 종료! 데미지 배율: {multiplier}");
    }






}