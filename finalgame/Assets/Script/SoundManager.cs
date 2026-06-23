using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public Slider volumeSlider;   // 유니티 에디터에서 Slider를 연결해주세요.
    public AudioSource bgmSource; // 유니티 에디터에서 AudioSource를 연결해주세요.

    void Start()
    {
        // 1. 게임이 시작되면 저장된 볼륨 값을 불러옵니다. (저장된 게 없다면 기본값 0.5f)
        float savedVolume = PlayerPrefs.GetFloat("BGMVolume", 0.5f);

        // 2. 불러온 값을 오디오 소스와 UI 슬라이더에 실제로 적용합니다.
        bgmSource.volume = savedVolume;
        volumeSlider.value = savedVolume;

        // 3. 슬라이더를 움직일 때마다 OnVolumeChanged 메서드가 실행되도록 이벤트를 연결합니다.
        volumeSlider.onValueChanged.AddListener(OnVolumeChanged);
    }

    // 슬라이더 값이 변경될 때 실행되는 메서드
    public void OnVolumeChanged(float value)
    {
        // 실시간으로 오디오 소스의 볼륨을 바꾸고
        bgmSource.volume = value;

        // 그 값을 즉시 PlayerPrefs에 저장합니다.
        PlayerPrefs.SetFloat("BGMVolume", value);
    }
}
