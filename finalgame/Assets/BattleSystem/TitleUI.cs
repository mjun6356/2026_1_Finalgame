using TMPro;
using UnityEngine;

public class TitleUI : MonoBehaviour
{
    public TextMeshProUGUI highScoreText;

    void Start()
    {
        highScoreText.text =
            "HIGH SCORE : "
            + PlayerPrefs.GetInt("HighScore", 0);
    }
}
