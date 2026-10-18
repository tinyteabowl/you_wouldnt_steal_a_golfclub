using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class VRScreenFade : MonoBehaviour
{
    [SerializeField] private Image fadeImage;
    [SerializeField] private float fadeDuration = 0.3f;

    private bool isFading = false;

    public IEnumerator FadeOut()
    {
        yield return Fade(0f, 1f);
    }

    public IEnumerator FadeIn()
    {
        yield return Fade(1f, 0f);
    }

    private IEnumerator Fade(float startAlpha, float endAlpha)
    {
        if (fadeImage == null || isFading)
            yield break;

        isFading = true;
        float elapsed = 0f;

        Color color = fadeImage.color;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(
                startAlpha, endAlpha,
                elapsed / fadeDuration
            );

            fadeImage.color = new Color(
                color.r, color.g, color.b, alpha
            );

            yield return null;
        }

        fadeImage.color = new Color(
            color.r, color.g, color.b, endAlpha
        );

        isFading = false;
    }
}