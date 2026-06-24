
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("이동 설정")]
    public float walkSpeed = 4f;
    public float runSpeed = 10f;

    [Header("플레이어 스탯")]
    public int maxHp = 100;
    public int currentHp;
    public int defense = 3;


    [Header("이동 애니메이션 프레임")]
    public Sprite[] spriteUp;
    public Sprite[] spriteDown;
    public Sprite[] spriteLeft;
    public Sprite[] spriteRight;
    public float frameTime = 0.15f;


    private SpriteRenderer sr;
    private Rigidbody2D rb;
    private Vector2 movement;
    private Vector2 input;
    
    private Sprite[] currentSprites;
    private int frameIndex = 0;
    private float timer = 0f;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
       
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();

        currentSprites = spriteDown;
        sr.sprite = currentSprites[0];
       
    }

    //  GameManager가 세이브할 때 플레이어의 최신 필드 위치를 가져가도록 싱크해주는 함수
   


    void Start()
    {
        currentHp = maxHp;
        
        
    }

    public int attackPower = 10;

    public void TakeDamage(int damage)
    {
        int finalDamage = Mathf.Max(1, damage - defense);

        currentHp -= finalDamage;

        if (currentHp <= 0)
        {
            GameManager.Instance.GameOver();
        }
    }

    public void OnMove(InputValue value)
    {
        input = value.Get<Vector2>();

       if (input.sqrMagnitude > 0.01f)
        {
            if (Mathf.Abs(input.x) > Mathf.Abs(input.y))
            {

                    if(input.x > 0)
                    ChangeSprites(spriteRight);
                    else
                        ChangeSprites(spriteLeft);
            }  
            else    
            {      
                    if (input.y > 0)
                        ChangeSprites(spriteUp);
                    else ChangeSprites(spriteDown);

            } 
        }

    }

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

    // ⭐ 현재 실시간 플레이어 정보를 GameManager의 세이브 데이터용 객체로 복사하는 함수
  


    private void ChangeSprites(Sprite[] newSprites)
    {
        if (currentSprites == newSprites)
            return;
        currentSprites = newSprites;
        frameIndex = 0;
        timer = 0f;
        sr.sprite = currentSprites[frameIndex];
    }
}
