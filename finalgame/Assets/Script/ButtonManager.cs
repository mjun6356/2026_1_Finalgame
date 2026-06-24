using UnityEngine;
using System.Collections;

public class ButtonManager : MonoBehaviour
{
    [Header("몇 초 후 버튼 표시")]
    public float showDelay = 3f;

    [Header("메뉴 버튼들")]
    public GameObject[] menuButtons;

    private void Start()
    {
        foreach (GameObject button in menuButtons)
        {
            button.SetActive(false);
        }

        StartCoroutine(ShowButtons());
    }

    IEnumerator ShowButtons()
    {
        yield return new WaitForSeconds(showDelay);

        foreach (GameObject button in menuButtons)
        {
            button.SetActive(true);
        }
    }
}
