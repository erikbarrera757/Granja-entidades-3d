using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PauseBackgroundFade : MonoBehaviour
{
    public Image backgroundImage;
    public float speed = 4f;

    public IEnumerator FadeIn()
    {
        float alpha = backgroundImage.color.a;

        while (alpha < 0.6f)
        {
            alpha += Time.unscaledDeltaTime * speed;
            backgroundImage.color = new Color(0, 0, 0, alpha);
            yield return null;
        }
    }

    public IEnumerator FadeOut()
    {
        float alpha = backgroundImage.color.a;

        while (alpha > 0f)
        {
            alpha -= Time.unscaledDeltaTime * speed;
            backgroundImage.color = new Color(0, 0, 0, alpha);
            yield return null;
        }
    }
}