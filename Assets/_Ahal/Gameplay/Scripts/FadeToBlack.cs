using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class ImageFader : MonoBehaviour
{
    public Image imageToFade;
    public float fadeDuration = 1.0f;

    public void StartFadeToBlack()
    {
        StartCoroutine(FadeImage());
    }
    private IEnumerator FadeImage()
    {
        Color startColor = imageToFade.color;
        Color endColor = new Color(startColor.r, startColor.g, startColor.b, 1.0f);

        float startTime = Time.time;
        float elapsedTime = 0;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime = Time.time - startTime;
            float percentage = elapsedTime / fadeDuration;
            imageToFade.color = Color.Lerp(startColor, endColor, percentage);
            yield return null;
        }

        imageToFade.color = endColor; // Ensure final alpha value
        SceneManager.LoadScene("MainMenu");
    }
}