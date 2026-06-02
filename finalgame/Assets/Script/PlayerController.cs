using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("이동 설정")]
    public float walkSpeed = 4f;
    public float runSpeed = 10f;

    [Header("플레이어 스탯")]
    public int currentHP;
    public int maxHP;
    public int gold;
    

    //[Header("이동 애니메이션 프레임")]

    //public Sprite[] spriteUp;
    //public Sprite[] spriteDown;
    //public Sprite[] spriteLeft;
    //public Sprite[] spriteRight;
    //public float frameTime = 0.15f;


    private SpriteRenderer sr;
    private Rigidbody2D rb;
    private Vector2 movement;
    private Vector2 input;
    
    //private Sprite[] currentSprites;
    //private int frameIndex = 0;
    //private float timer = 0f;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
       
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();

        //currentSprites = spriteDown;
        //sr.sprite = currentSprites[0];
       
    }

    private void Start()
    {
       /* // 씬이 로드될 때 GameManager의 데이터를 플레이어에게 반영
        if (GameManager.Instance != null)
        {
            // 1. 위치 복구 (저장된 위치가 있을 때만)
            if (GameManager.Instance.currentData.playerPosition != Vector2.zero)
            {
                transform.position = GameManager.Instance.currentData.playerPosition;
            }

            // 2. 스탯 복구 (만약 게임을 처음 시작해서 MaxHP가 0이라면 기본값 세팅)
            if (GameManager.Instance.currentData.playerMaxHP > 0)
            {
                currentHP = GameManager.Instance.currentData.playerHP;
                maxHP = GameManager.Instance.currentData.playerMaxHP;
                gold = GameManager.Instance.currentData.gold;
            }
            else
            {
                // [로그라이크 첫 시작] 기본 초기 능력치 설정
                maxHP = 100;
                currentHP = maxHP;
                gold = 0;
                
                // 매니저 데이터도 초기값으로 갱신
                SyncStatsToManager();
            }
        }*/
    }

    //public void OnMove(InputValue value)
    //{
    //    input = value.Get<Vector2>();

    //    if (input.sqrMagnitude > 0.01f)
    //    {
    //        if (Mathf.Abs(input.x) > Mathf.Abs(input.y))
    //        {

    //                if(input.x > 0)
    //                ChangeSprites(spriteRight);
    //                else
    //                    ChangeSprites(spriteLeft);
    //        }  
    //        else    
    //        {      
    //                if (input.y > 0)
    //                    ChangeSprites(spriteUp);
    //                else ChangeSprites(spriteDown);

    //        } 
    //    }

    //}

    // Update is called once per frame
    void Update()
    {
        //이게 이동 함수
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
    }
    void FixedUpdate()
    {
        // 쉬프트를 누르고 있다면 runSpeed, 아니면 walkSpeed를 즉시 선택
        float speed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;

        // 이동 값이 있을 때만 속도 적용
        if (movement.magnitude > 0)
        {
            rb.linearVelocity = movement.normalized * speed;
        }
        else
        {
            rb.linearVelocity = Vector2.zero; // 안 누르면 즉시 정지 (언더테일 느낌)
        }

    }
    //private void ChangeSprites(Sprite[] newSprites)
    //{
    //    if (currentSprites == newSprites)
    //        return;
    //    currentSprites = newSprites;
    //    frameIndex = 0;
    //    timer = 0f;
    //    sr.sprite = currentSprites[frameIndex];
    //}
}
