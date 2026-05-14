using UnityEngine;

public class BackgroundDarkener : MonoBehaviour
{
    public Color dayColor = Color.white;
    public Color nightColor = new Color(0.2f, 0.2f, 0.3f);

    private SpriteRenderer[] sprites;

    void Start()
    {
        sprites = GetComponentsInChildren<SpriteRenderer>();
    }

    void Update()
    {
        if (DayNightCycle.Instance == null) return;

        float normalizedTime = DayNightCycle.Instance.timeOfDay / 24f;
        float intensity = Mathf.Clamp01(Mathf.Sin(normalizedTime * Mathf.PI));

        Color currentColor = Color.Lerp(nightColor, dayColor, intensity);

        foreach (SpriteRenderer sprite in sprites)
        {
            if (sprite != null)
            {
                sprite.color = currentColor;
            }
        }
    }
}