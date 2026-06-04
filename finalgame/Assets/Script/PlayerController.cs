using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("이동 설정")]
    public float walkSpeed = 4f;
    public float runSpeed = 10f;

    [Header("플레이어 스탯")]
    public int maxHealth=1000;
    public int currentHealth=100;
    public int attackPower=20;
    public int defensePower=50;
    public int gold=3000;
    public int goldMax=99999;

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

    void Start()
    {
       if (GameManager.Instance != null)
       {


           // 기존 플레이어 정보를 사용
           maxHealth = GameManager.Instance.currentData.playerMaxHP;
           currentHealth = GameManager.Instance.currentData.playerHP;
           attackPower = GameManager.Instance.currentData.playerAttackPower;
           defensePower = GameManager.Instance.currentData.playerDefensePower;
       }
       else
       {


           // 기본값 설정
           maxHealth = 100;
           currentHealth = maxHealth;
           attackPower = 10;
           defensePower = 5;
       }

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

    // ⭐ 현재 실시간 플레이어 정보를 GameManager의 세이브 데이터용 객체로 복사하는 함수
    public void SyncStatsToManager()
    {
        if (GameManager.Instance == null) return;

        GameManager.Instance.currentData.playerPosition = transform.position;
        GameManager.Instance.currentData.playerHP = currentHealth;
        GameManager.Instance.currentData.playerMaxHP = maxHealth;
        GameManager.Instance.currentData.playerGold = gold;
        GameManager.Instance.currentData.playerGoldMax = goldMax;
        GameManager.Instance.currentData.playerAttackPower = attackPower;
        GameManager.Instance.currentData.playerDefensePower = defensePower;
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
