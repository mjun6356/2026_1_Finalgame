using UnityEngine;
using UnityEngine.SceneManagement;

public class MoveObject : MonoBehaviour
{
    [Header("움직일 대상 오브젝트")]
    public Transform objectToMove;

    [Header("이동할 목표 위치 (빈 게임 오브젝트 권장)")]
    public Transform targetPosition;

    [Header("이동 속도")]
    public float moveSpeed = 5f;

    private bool isMoving = false;

    // 변경점 1: OnTriggerEnter2D 함수 사용
    // 변경점 2: 매개변수 타입을 Collider2D로 지정
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isMoving = true;
        }
    }

    private void Update()
    {
        if (isMoving && objectToMove != null && targetPosition != null)
        {
            // Transform.position은 기본적으로 Vector3를 사용하므로 
            // 2D 게임이어도 Vector3.MoveTowards를 그대로 사용해도 완벽하게 작동합니다.
            objectToMove.position = Vector3.MoveTowards(
                objectToMove.position,
                targetPosition.position,
                moveSpeed * Time.deltaTime
            );
        }
    }

}
