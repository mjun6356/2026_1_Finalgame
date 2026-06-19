using UnityEngine;
using System.Collections;

public class DialogueFrist : MonoBehaviour
{
    public DialiogueDataSO fristDialogue;
    private DialogeManager fristDialiogumanager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        fristDialiogumanager = FindAnyObjectByType<DialogeManager>();
        if (fristDialiogumanager == null)
        {
            Debug.Log("다이얼 로그 매니저가 없습니다.");
        }

        

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        

        if (other.CompareTag("Player"))
        {
            fristDialiogumanager.StartDialogue(fristDialogue);
        }

        
        


        Destroy(gameObject);

        

    }




    // Update is called once per frame
    void Update()
    {
        if (fristDialiogumanager == null) return;
        if (fristDialiogumanager .IsDialogueActive()) return;
        if (fristDialogue == null) return;
    }
}
