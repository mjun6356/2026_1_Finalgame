using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

public class ChoiceButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("마우스를 올렸을 때 표시할 이미지")]
    public GameObject imageObject;

   

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (imageObject != null)
            imageObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (imageObject != null)
            imageObject.SetActive(false);
    }

    public void ChangeScene()
    {
        SceneManager.LoadScene("meinmenu");
    }

    public void EixtGame()
    {
        UnityEditor.EditorApplication.isPlaying = false;
    }

 
}
