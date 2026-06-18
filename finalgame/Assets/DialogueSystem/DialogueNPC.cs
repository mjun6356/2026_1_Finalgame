using UnityEngine;

public class DialogueNPC : MonoBehaviour
{
    public DialiogueDataSO myDialogue;
    private DialogeManager dialogeManager;
    // 플레이어가 범위 안에 있는지 체크하는 변수
    private bool isPlayerInRange = false;

   


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        dialogeManager = FindAnyObjectByType<DialogeManager>();

        if (dialogeManager == null)
        {
            Debug.Log("다이얼 로그 매니저가 없습니다.");
        }

        
    }

    //private void OnMouseDown()
    //{

    //    if (dialogeManager == null) return;
    //    if (dialogeManager.IsDialogueActive()) return;
    //    if (myDialogue == null) return;

    //    dialogeManager.StartDialogue(myDialogue);
    //}


    // Update is called once per frame
    void Update()
    {
        if (dialogeManager == null) return;
        if (dialogeManager.IsDialogueActive()) return;
        if (myDialogue == null) return;

        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            dialogeManager.StartDialogue(myDialogue);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            Debug.Log("E키를 눌러 NPC와 대화하세요.");
            //여기에 UI안내 텍스트를 껴주는 코드 넣으면 좋음
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            Debug.Log("대화 가능 범위를 벗어났습니다.");

            //활성화했던 UI 안내 텍스트를 다시 끄는 코드를 넣으시면 됩니다.
        }
    }
}

