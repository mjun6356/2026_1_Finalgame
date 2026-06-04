using UnityEngine;
using UnityEngine.SceneManagement;

// 전투의 상태를 정의
public enum BattleState { START,PLAYMENU, PLAYACTION, ENEMYTURN, CHECK }

public class BattleManger : MonoBehaviour
{
    public BatteryStatus currentState;

    //UI 패널들 연결
    public GameObject MainPanel;
    public GameObject attackTargetPanel;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
