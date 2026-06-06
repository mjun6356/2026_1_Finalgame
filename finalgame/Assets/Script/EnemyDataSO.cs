using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemyData", menuName = "Scriptable Objects/EnemyData")]
public class EnemyDataSO : ScriptableObject
{
    [Header("기본 정보")]
    public int enemyID;         // 몬스터 도감 번호
    public string enemyName;    // 이름

    [Header("전투 스탯")]
    public int maxHP;           // 최대 체력
    public int attackPower;     // 공격력

    [Header("언더테일 풍 텍스트 설정")]
    [TextArea] public string encounterText; // 전투 시작 시 뜰 대사
    [TextArea] public string actActionText; // ACT(행동) 버튼 눌렀을 때 대사
}
