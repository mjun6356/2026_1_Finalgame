using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemyData", menuName = "Scriptable Objects/EnemyData")]
public class EnemyDataSO : ScriptableObject
{
    [Header("기본 정보")]
   
    public string enemyName;    // 이름
    public Sprite enemySprite;  // 몬스터 이미지

    [Header("전투 스탯")]
    public int maxHP;           // 최대 체력
    public int attackPower;     // 공격력

    public int playerPlusGold;   // 플레이어에게 줄 골드량
    

    [Header("언더테일 풍 텍스트 설정")]
    public DialiogueDataSO encounterDialogue; // 전투 진입 대사
    public DialiogueDataSO actActionDialogue; // ACT(행동) 대사
}
