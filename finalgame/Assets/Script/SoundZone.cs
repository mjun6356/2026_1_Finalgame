using UnityEngine;
using UnityEngine.Audio;

public class SoundZone : MonoBehaviour
{
    private AudioSource audioSource;

    // 부드러운 전환을 위한 페이드 속도
    public float fadeSpeed = 1.5f;
    private bool isPlayerInside = false;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = 0; 
    }

    // Update is called once per frame
    void Update()
    {
        // 플레이어가 구역 안에 있으면 볼륨을 키우고, 나갔으면 줄입니다 (Fade In/Out)
        if (isPlayerInside)
        {
            audioSource.volume = Mathf.MoveTowards(audioSource.volume, 1f, fadeSpeed * Time.deltaTime);
        }
        else
        {
            audioSource.volume = Mathf.MoveTowards(audioSource.volume, 0f, fadeSpeed * Time.deltaTime);
            if (audioSource.volume == 0f && audioSource.isPlaying)
            {
                audioSource.Stop();
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        // 플레이어의 태그가 "Player"여야 합니다.
        if (other.CompareTag("Player"))
        {
            isPlayerInside = true;
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = false;
        }
    }

}
