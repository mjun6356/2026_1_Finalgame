using System.Collections.Generic;
using UnityEngine;



    // ==========================================
    // 1. 저장할 데이터 구조 정의 (데이터 바구니)
    // ==========================================
    // ⚠️ 주의: 이 클래스는 MonoBehaviour를 상속받지 않습니다! (뒤에 : MonoBehaviour가 없음)
    [System.Serializable]
    public class GameData
    {
        // 플레이어 데이터
        public Vector2 playerPosition;
        public int playerHP;
        public int playerMaxHP;
        public int playerGold;
        public int playerGoldMax;
        public int playerAttackPower;
        public int playerDefensePower;

        // 로그라이크 진행 데이터
        public int currentFloor;
        public List<string> defeatedEnemyIDs = new List<string>(); // 처치된 적들의 ID 목록
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
   

