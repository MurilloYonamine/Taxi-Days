using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundManager : MonoBehaviour
{
    public List<GameObject> backgrounds = new List<GameObject>();
    private List<CanvasGroup> canvasGroups = new List<CanvasGroup>();

    void Start()
    {
        foreach (var background in backgrounds)
        {
            var canvasGroup = background.GetComponent<CanvasGroup>();
            if (canvasGroup != null)
            {
                canvasGroups.Add(canvasGroup);
            }
        }
    }

    public void ShowAll(float duration = 0.5f)
    {
        StartCoroutine(FadeCanvasGroups(canvasGroups, 1f, duration));
    }

    public void HideAll(float duration = 0.5f)
    {
        StartCoroutine(FadeCanvasGroups(canvasGroups, 0f, duration));
    }

    private IEnumerator FadeCanvasGroups(List<CanvasGroup> groups, float targetAlpha, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(groups[0].alpha, targetAlpha, elapsed / duration);

            foreach (var group in groups)
            {
                group.alpha = alpha;
                group.interactable = targetAlpha > 0.5f;
                group.blocksRaycasts = targetAlpha > 0.5f;
            }
            yield return null;
        }

        foreach (var group in groups)
        {
            group.alpha = targetAlpha;
            group.interactable = targetAlpha > 0.5f;
            group.blocksRaycasts = targetAlpha > 0.5f;
        }
    }
}
