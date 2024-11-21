using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonBehaviours : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private static ButtonBehaviours selectedButton = null;
    public Animator anim;
    private CanvasGroup containerCanvasGroup => GetComponentInChildren<CanvasGroup>();

    private void Awake()
    {
        containerCanvasGroup.alpha = 0.5f;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        containerCanvasGroup.alpha = 0.5f;
        anim.Play("Exit");
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (selectedButton != null && selectedButton != this)
        {
            selectedButton.OnPointerExit(null);
        }

        anim.Play("Enter");

        containerCanvasGroup.alpha = 1;

        selectedButton = this;
    }
}