using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

public class Credits : MonoBehaviour
{
    public AudioClip menuMusic;
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI functionsText;
    public TextMeshProUGUI namesText;
    public float scrollSpeed = 35f;
    public Image fadeImage;
    public float fadeSpeed = 1.5f;

    private bool shouldScroll = true;

    void Start()
    {
        AudioManager.instance.PlayTrack(menuMusic, channel: 0, startingVolume: 0.5f);
        StartCoroutine(ScrollCredits());
    }

    IEnumerator ScrollCredits()
    {
        while (shouldScroll)
        {
            titleText.rectTransform.anchoredPosition += Vector2.up * scrollSpeed * Time.deltaTime;
            functionsText.rectTransform.anchoredPosition += Vector2.up * scrollSpeed * Time.deltaTime;
            namesText.rectTransform.anchoredPosition += Vector2.up * scrollSpeed * Time.deltaTime;

            if (namesText.rectTransform.anchoredPosition.y >= Screen.height)
            {
                shouldScroll = false;
                yield return StartCoroutine(FadeOutAndTransition());
            }

            yield return null;
        }
    }


    IEnumerator FadeOutAndTransition()
    {
        fadeImage.gameObject.SetActive(true);

        Color originalColor = fadeImage.color;
        float alpha = 0f;

        while (alpha < 1f)
        {
            alpha += fadeSpeed * Time.deltaTime;
            fadeImage.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            yield return null;
        }

        TransitionToNextScene();
    }

    void TransitionToNextScene()
    {
        SceneManager.LoadScene("Main Menu");
    }


}