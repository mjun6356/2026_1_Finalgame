using UnityEngine;
using System.Collections;

public class End : MonoBehaviour
{
    [Header("엔딩 연출 설정")]
    [Tooltip("페이드 아웃할 스프라이트들을 순서대로 넣어주세요.")]
    public SpriteRenderer[] targetSprites;

    [Tooltip("각 스프라이트가 완전히 사라지는 데 걸리는 시간 (초)")]
    public float fadeDuration = 1.5f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // 씬이 시작될 때 연출을 바로 시작합니다.
        // 필요에 따라 버튼 클릭이나 특정 이벤트 발생 시 호출하도록 변경하셔도 됩니다.
        StartCoroutine(SequenceFadeOut());
    }
    private IEnumerator SequenceFadeOut()
    {
        // 배열에 있는 스프라이트를 처음부터 하나씩 꺼내어 처리합니다.
        for (int i = 0; i < targetSprites.Length; i++)
        {
            SpriteRenderer currentSprite = targetSprites[i];

            // 스프라이트가 할당되지 않은 경우 건너뜀 (에러 방지)
            if (currentSprite == null) continue;

            Color color = currentSprite.color;
            float elapsedTime = 0f;

            // 설정한 시간(fadeDuration)동안 알파값을 1에서 0으로 서서히 줄입니다.
            while (elapsedTime < fadeDuration)
            {
                elapsedTime += Time.deltaTime;

                // Mathf.Lerp를 사용하여 자연스럽게 값을 보간합니다.
                color.a = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
                currentSprite.color = color;

                // 다음 프레임까지 대기합니다.
                yield return null;
            }

            // 루프가 끝난 후, 알파값을 확실하게 0으로 고정하여 잔상을 없앱니다.
            color.a = 0f;
            currentSprite.color = color;
        }

        // 모든 스프라이트의 페이드아웃이 완료된 후 실행될 로직
        OnEndingSequenceComplete();
    }

    private void OnEndingSequenceComplete()
    {
        Debug.Log("모든 엔딩 스프라이트의 페이드아웃이 완료되었습니다!");
        // TODO: 타이틀 씬으로 넘어가기, 엔딩 크레딧 띄우기 등의 코드를 여기에 작성하세요.
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
