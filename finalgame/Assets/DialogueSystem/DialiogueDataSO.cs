using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "DialiogueDataSO", menuName = "Scriptable Objects/DialiogueDataSO")]
public class DialiogueDataSO : ScriptableObject
{
    [Header("캐릭터 정보")]
    public string characterName = "캐릭터"; // 대사 창에 표시될 캐릭터 이름
    public Sprite characterImage; // 대사 창에 표시될 캐릭터 초상화 이미지


    [Header("대화 내용")]
    [TextArea(3,10)]
    public List<string> dialogueLines = new List<string>(); // 대화 내용이 담긴 리스트





}
