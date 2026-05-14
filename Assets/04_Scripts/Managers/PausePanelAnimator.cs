using System.Collections;
using UnityEngine;

public class PausePanelAnimator : MonoBehaviour
{
    public RectTransform panel;

    public Vector2 hiddenPosition = new Vector2(-300, 200);
    public Vector2 visiblePosition = new Vector2(50, -50);

    public float speed = 8f;

    private Coroutine currentAnim;

    void Start()
    {
        panel.anchoredPosition = hiddenPosition;
    }

    public void Show()
    {
        gameObject.SetActive(true);

        if (currentAnim != null) StopCoroutine(currentAnim);
        currentAnim = StartCoroutine(MoveTo(visiblePosition));
    }

    public void Hide()
    {
        if (currentAnim != null) StopCoroutine(currentAnim);
        currentAnim = StartCoroutine(HideRoutine());
    }

    IEnumerator MoveTo(Vector2 target)
    {
        while (Vector2.Distance(panel.anchoredPosition, target) > 0.5f)
        {
            panel.anchoredPosition = Vector2.Lerp(
                panel.anchoredPosition,
                target,
                Time.unscaledDeltaTime * speed
            );
            yield return null;
        }

        panel.anchoredPosition = target;
    }

    IEnumerator HideRoutine()
    {
        yield return MoveTo(hiddenPosition);
        gameObject.SetActive(false);
    }
}