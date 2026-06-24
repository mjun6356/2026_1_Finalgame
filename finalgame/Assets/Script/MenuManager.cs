using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public GameObject helpPanel;
  
    [SerializeField] private Image fadeImage; // 페이드에 사용할 UI 이미지
    [SerializeField] private float fadeDuration = 2.0f; // 서서히 나타나는 시간 (초)
    [SerializeField] private string nextSceneName; // 이동할 다음 씬 이름

    private bool isFading = false; // 중복 클릭 방지용 플래그

    private void Start()
    {
        fadeImage.gameObject.SetActive(false);
    }

    public void OnButtonClick()
    {
        // 이미 페이드 중이라면 중복 실행하지 않음
        if (isFading) return;

        StartCoroutine(FadeInAndChangeScene());
    }

    private IEnumerator FadeInAndChangeScene()
    {
        fadeImage.gameObject.SetActive(true);

        isFading = true;

        // 페이드 이미지의 Raycast Target을 켜서 페이드 도중 다른 UI 클릭 방지
        fadeImage.raycastTarget = true;

        float elapsedTime = 0f;
        Color color = fadeImage.color;

        // 알파 값을 0에서 1까지 서서히 증가시킴
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Clamp01(elapsedTime / fadeDuration);
            fadeImage.color = color;
            yield return null; // 다음 프레임까지 대기
        }

        // 확실하게 알파 값을 1로 고정
        color.a = 1f;
        fadeImage.color = color;


        // 완전히 생겨났으니 다음 씬으로 전환
        SceneManager.LoadScene("Would");
    }
    // 1. '게임 시작' 버튼을 누르면 실행될 함수
    
    
        
        

        
    

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
