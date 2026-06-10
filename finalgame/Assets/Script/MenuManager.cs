using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public GameObject helpPanel;
    
    

    

    // 1. '게임 시작' 버튼을 누르면 실행될 함수
    public void StartGame()
    {
        
        SceneManager.LoadScene("Would");

        
    }

    // 2. '도움말' 버튼을 누르면 실행될 함수
    public void ShowHelp()
    {
        // 도움말 창을 화면에 보이게 켭니다.
        helpPanel.SetActive(true);
    }

    // 3. 도움말 창을 닫고 싶을 때 쓸 함수 
    public void CloseHelp()
    {
        // 도움말 창을 다시 안 보이게 끕니다.
        helpPanel.SetActive(false);
    }

    public  void EixtButton()
    {
        UnityEditor.EditorApplication.isPlaying = false;
    }

}
