using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public DialogueData data; //에디터에서 만든 ScriptableObnect를 할당

    public void TriggerDialogue()
    {
        DialogueManager.instance.StartDialogue(data);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            TriggerDialogue();
        }

        
    }
}
