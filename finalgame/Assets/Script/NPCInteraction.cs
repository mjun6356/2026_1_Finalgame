using UnityEngine;

public class NPCInteraction : MonoBehaviour
{
    // 플레이어가 범위 안에 있는지 체크하는 변수
    private bool isPlayerInRange = false;

    // Update is called once per frame
    void Update()
    {
        // 플레이어가 범위 안에 있고, E 키를 눌렀을 때
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            
        }
    }
}
