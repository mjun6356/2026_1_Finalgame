using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System;

public class DialogeManager : MonoBehaviour
{
    [Header("UI 요소 -  인스펙터 창에서 연결")]
    public GameObject DialoguePanel; // 대화 패널 전체
    public TextMeshProUGUI characterNameText; // 캐릭터 이름 텍스트
    public TextMeshProUGUI dialogueText; // 대화 내용 텍스트
    public Image characterImage; // 캐릭터 초상화 이미지
    public Button nextButton; // '다음' 버튼


    [Header("기본 설정")]
    public Sprite defaultCharacterImage; // 기본 캐릭터 이미지 (인스펙터 창에서 연결)

    [Header("타이핑 효과 설정")]
    public float typingSpeed = 0.05f; // 타이핑 효과 속도 (초 단위)
    public bool skipTyoingOnClick = true; // 대화 텍스트를 클릭하면 타이핑 효과를 건너뛸지 여부
    public AudioSource audioSource;

    //내부 변수
    private DialiogueDataSO currentDialogue; // 현재 대화 데이터
    private int currentLineIndex; // 현재 대화 줄 인덱스
    private bool isTyping = false; // 타이핑 효과가 진행 중인지 여부
    private bool isDialogueActive = false; // 대화가 진행 중인지 확인하는 플래그
    private Coroutine typingCoroutine; // 타이핑 효과 코루틴 참조

    IEnumerator TypeDialogue(string line)
    {
        isTyping = true;
        dialogueText.text = ""; // 대화 텍스트 초기화

        foreach (char letter in line.ToCharArray())
        {
            dialogueText.text += letter; // 한 글자씩 추가
            yield return new WaitForSeconds(typingSpeed); // 타이핑 속도만큼 대기
        }

        isTyping = false; // 타이핑 완료
    }


    private void CompleteTyping()
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine); // 타이핑 코루틴 중지
        }
       
        isTyping = false; // 타이핑 완료 상태로 설정

        if (currentDialogue != null && currentLineIndex < currentDialogue.dialogueLines.Count)
        {
            dialogueText.text = currentDialogue.dialogueLines[currentLineIndex]; // 현재 대화 줄 전체 표시
        }
    }

    void ShowCurrentLine()
    {
        if (currentDialogue != null && currentLineIndex < currentDialogue.dialogueLines.Count)
        {
            if (typingCoroutine != null) // 이전 타이핑 코루틴이 있다면
            {
                StopCoroutine(typingCoroutine); // 이전 타이핑 코루틴 중지
            }
        }

        string currentText = currentDialogue.dialogueLines[currentLineIndex]; // 현재 대화 줄 가져오기
        typingCoroutine = StartCoroutine(TypeDialogue(currentText));

    }

    public void ShowNextLine()
    {
        currentLineIndex++;

        if (currentLineIndex >= currentDialogue.dialogueLines.Count)
        {
            EndDialogue(); // 대화 종료
        }
        else
        {
            ShowCurrentLine(); // 다음 대화 줄 표시
        }


    }

    void EndDialogue()
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine); // 타이핑 코루틴 중지
            typingCoroutine = null;
        }

        isDialogueActive = false;
        isTyping = false;
        DialoguePanel.SetActive(false);
        currentLineIndex = 0;

    }

    public void HandleNextInput()
    {
        if (isTyping && skipTyoingOnClick)
        {
            CompleteTyping(); // 타이핑 효과 건너뛰기
        }
        else if (!isTyping)
        {
            ShowNextLine(); // 다음 대화 줄 표시
        }
    }

    public void SkipDialogue()
    {
        EndDialogue(); // 대화 종료
    }

    public bool IsDialogueActive()
    {
        return isDialogueActive;
    }

    public void StartDialogue(DialiogueDataSO dialogue)
    {
        if (dialogue == null || dialogue.dialogueLines.Count == 0) return; // 대화 데이터가 유효하지 않으면 종료

        currentDialogue = dialogue;
        currentLineIndex = 0;
        isDialogueActive = true;

        // UI 요소 업데이트
        DialoguePanel.SetActive(true); // 대화 패널 활성화
        characterNameText.text = dialogue.characterName;

        if (characterImage != null )
        {
            if (dialogue.characterImage != null)
            {
                characterImage.sprite = dialogue.characterImage; // 캐릭터 이미지 설정
            }
            else
            {
                characterImage.sprite = defaultCharacterImage; // 기본 이미지 설정
            }
        }

        ShowCurrentLine(); // 첫 번째 대화 줄 표시
    }

    void Start()
    {
        DialoguePanel.SetActive(false); // 시작 시 대화 패널 비활성화
        nextButton.onClick.AddListener(HandleNextInput); // '다음' 버튼 클릭 이벤트에 핸들러 등록
    }


    void Update()
    {
        if (isDialogueActive && Input.GetKeyDown(KeyCode.Space))
        {
            HandleNextInput(); // 스페이스바 입력 처리
        }
    }







}
