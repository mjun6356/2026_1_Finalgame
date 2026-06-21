using Unity.VisualScripting;
using UnityEngine;

public class MoveObject : MonoBehaviour
{

    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private Vector2 moveDirection = Vector2.zero;
    [SerializeField] private float distanceThreshold = 0.1f; // 목표 지점에 도달했는지 확인하는 거리 임계값
    private bool isMoving = false;



    public GameObject[] targetObject; // 이동할 대상 오브젝트
    public Vector2 targetPosition; // 이동할 목표 위치


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        targetPosition = transform.position;
        isMoving = false;
    }

    // Update is called once per frame
    void Update()
    {
       /* // Mathf.PingPong은 0부터 distance까지의 값을 왔다 갔다 반환합니다.
        float move = Mathf.PingPong(Time.time * moveSpeed, distanceThreshold);

        // 시작 위치에 더해줍니다.
        transform.position = targetPosition + (moveDirection.normalized * move);*/
    }

   /* public void Move(Vector2 direction)
    {
        transform.Translate(direction * moveSpeed * Time.deltaTime);
    }*/

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isMoving = true;

            // 이동할 목표 위치를 설정 (예: 현재 위치에서 5만큼 오른쪽으로 이동)
            targetPosition = new Vector2(transform.position.x + 5f, transform.position.y);

        }
    }
}
