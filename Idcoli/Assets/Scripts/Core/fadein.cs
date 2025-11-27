using UnityEngine;

using UnityEngine.UI;
using System.Collections;


   


public class fadein : MonoBehaviour
{
    public static fadein Instance; // singleton for easy access
    private Image fadeImage;
    private void Awake()
    {
        if (Instance == null) Instance = this;
        fadeImage = GetComponent<Image>();
    }

    public IEnumerator FadeOut(float duration)
    {
        Color color = fadeImage.color;
        float startAlpha = color.a;

        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            float alpha = Mathf.Lerp(startAlpha, 1f, t / duration);
            fadeImage.color = new Color(color.r, color.g, color.b, alpha);
            yield return null;
        }
        fadeImage.color = new Color(color.r, color.g, color.b, 1f);
    }

    public IEnumerator FadeIn(float duration)
    {
        Color color = fadeImage.color;
        float startAlpha = color.a;

        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            float alpha = Mathf.Lerp(startAlpha, 0f, t / duration);
            fadeImage.color = new Color(color.r, color.g, color.b, alpha);
            yield return null;
        }
        fadeImage.color = new Color(color.r, color.g, color.b, 0f);
    }
}
