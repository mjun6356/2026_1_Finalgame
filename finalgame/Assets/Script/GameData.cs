using System.Collections.Generic;
using UnityEngine;



   
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

   

