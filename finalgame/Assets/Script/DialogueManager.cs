using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;  // TextMashPro 사용 권장
using UnityEngine.UI;
using System;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;

    public TMP_Text nameText;
    public TMP_Text dialogueText;
    public Image portraitImage;
    public GameObject diologuePanel;

    private Queue<DialogueEntry> sentences = new Queue<DialogueEntry>();
    public bool isDialogueActive = false;

    private bool isTyping = false;
    private string currentFullSentence;

    void Awake() => instance = this;

    public void StartDialogue(DialogueData data)
    {
        isDialogueActive=true;
        diologuePanel.SetActive(true);
        sentences.Clear();
    } 

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        DialogueEntry currentEntry = sentences.Dequeue();
        nameText.text = currentEntry.name;
        portraitImage.sprite = currentEntry.portrait;

        StopAllCoroutines();
        StartCoroutine(TypeSentence(currentEntry.sentence));

    }

    IEnumerator TypeSentence(string sentence)
    {
        isTyping = true;
        currentFullSentence = sentence;

        dialogueText.text = "";

        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(0.05f);
        }
        isTyping=false;

    }

    // 클릭(또는 스페이스바) 시 호출할 함수
    public void OnClickDialogue()
    {
        if (isTyping)
        {
            // 타이핑 중이면 즉시 전체 문장 표시
            StopAllCoroutines();
            dialogueText.text = currentFullSentence;
            isTyping = false;
        }
        else
        {
            // 타이핑이 끝났으면 다음 문장으로
            DisplayNextSentence();
        }
    }


    void EndDialogue()
    {
        isDialogueActive = false;
        diologuePanel.SetActive(false);
    }
}
