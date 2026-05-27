using UnityEngine;

[System.Serializable]
public struct DialogueEntry
{
    public string name;  // 캐릭터 이름
    [TextArea(3, 10)]
    public string sentence;  // 대사 내용
    public Sprite portrait;  // 캐릭터 초상화
}

[CreateAssetMenu(fileName = "NewDialogue" , menuName = "Dialogue/DialogueData")]
public class DialogueData : ScriptableObject
{
    public DialogueEntry[] dialogues;  // 여러 개의 대사를 담는 배열
}
