using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonAnimator : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    public Vector3 normalScale = Vector3.one;
    public Vector3 hoverScale = new Vector3(1.12f, 1.12f, 1.12f);
    public Vector3 clickScale = new Vector3(0.94f, 0.94f, 0.94f);

    public float smoothSpeed = 12f;
    public float hoverYOffset = 8f;

    private RectTransform rectTransform;
    private Vector3 targetScale;
    private Vector2 originalPosition;
    private Vector2 targetPosition;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();

        originalPosition = rectTransform.anchoredPosition;
        targetPosition = originalPosition;
        targetScale = normalScale;
    }

    void Update()
    {
        transform.localScale = Vector3.Lerp(
            transform.localScale,
            targetScale,
            Time.unscaledDeltaTime * smoothSpeed
        );

        rectTransform.anchoredPosition = Vector2.Lerp(
            rectTransform.anchoredPosition,
            targetPosition,
            Time.unscaledDeltaTime * smoothSpeed
        );
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        targetScale = hoverScale;
        targetPosition = originalPosition + new Vector2(0, hoverYOffset);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        targetScale = normalScale;
        targetPosition = originalPosition;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        StopAllCoroutines();
        StartCoroutine(ClickBounce());
    }

    IEnumerator ClickBounce()
    {
        targetScale = clickScale;
        yield return new WaitForSecondsRealtime(0.08f);

        targetScale = hoverScale;
    }
}