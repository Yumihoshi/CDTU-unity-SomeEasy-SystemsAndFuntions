using UnityEngine;

public class FadeManager : MonoBehaviour
{
    public static FadeManager Instance;
    private CanvasGroup canvasGroup;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            canvasGroup = GetComponent<CanvasGroup>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void FadeIn(float duration)
    {
        StartCoroutine(Fade(1, duration));
    }

    public void FadeOut(float duration)
    {
        StartCoroutine(Fade(0, duration));
    }

    private IEnumerator Fade(float targetAlpha, float duration)
    {
        float startAlpha = canvasGroup.alpha;
        float time = 0;

        while (time < duration)
        {
            time += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, time / duration);
            yield return null;
        }

        canvasGroup.alpha = targetAlpha;
    }
}