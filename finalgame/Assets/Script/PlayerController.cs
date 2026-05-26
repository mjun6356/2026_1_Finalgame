using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float walkSpeed = 4f;
    public float runSpeed = 10f;

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

    public void OnMove(InputValue value)
    {
        input = value.Get<Vector2>();

        if (input.sqrMagnitude > 0.01f)
        {
            if (Mathf.Abs(input.x) > Mathf.Abs(input.y))
            {
                if (input.x > 0)
                {
                    ChangeSprites(spriteRight);
                    else
                        ChangeSprites(spriteLeft);
                }
                else
                {
                    if (input.y > 0)
                        ChangeSprites(spriteUp);
                }
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
