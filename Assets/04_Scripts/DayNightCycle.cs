using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    public static DayNightCycle Instance;

    public Light sun;

    [Range(0f, 24f)]
    public float timeOfDay = 12f;

    public float dayDuration = 60f;

    public Color dayColor = Color.white;
    public Color nightColor = new Color(0f, 0f, 0.01f);
    public float daySpeed = 1f;
    public float nightSpeed = 0.45f;
    public bool IsNight => timeOfDay < 6f || timeOfDay >= 18f;
    public bool IsDay => !IsNight;

    private bool morningTriggered = false;

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        float speedMultiplier = IsNight ? nightSpeed : daySpeed;

        timeOfDay += (24f / dayDuration) * Time.deltaTime * speedMultiplier;

        if (timeOfDay >= 24f)
        {
            timeOfDay = 0f;
        }

        CheckMorningReturn();

        UpdateLighting();
    }

    void CheckMorningReturn()
    {
        if (timeOfDay >= 6f && !morningTriggered)
        {
            morningTriggered = true;

            EntityMovement[] entities = Object.FindObjectsByType<EntityMovement>(
                FindObjectsSortMode.None
            );

            foreach (EntityMovement entity in entities)
            {
                entity.ReturnToCorral();
            }

            Debug.Log("Amaneció: las entidades vuelven a sus corrales.");
        }

        if (timeOfDay < 5f)
        {
            morningTriggered = false;
        }
    }

    void UpdateLighting()
    {
        float normalizedTime = timeOfDay / 24f;

        sun.transform.rotation = Quaternion.Euler((normalizedTime * 360f) - 90f, 170f, 0);

        float intensity = Mathf.Clamp01(Mathf.Sin(normalizedTime * Mathf.PI));

        sun.intensity = Mathf.Lerp(0f, 1.3f, intensity);

        RenderSettings.ambientLight = Color.Lerp(nightColor, dayColor, intensity);
        RenderSettings.ambientIntensity = Mathf.Lerp(0.2f, 1.2f, intensity);

        RenderSettings.fog = true;

        Color fogDay = new Color(0.7f, 0.8f, 0.9f);
        Color fogNight = new Color(0.02f, 0.02f, 0.05f);

        RenderSettings.fogColor = Color.Lerp(fogNight, fogDay, intensity);
        RenderSettings.fogDensity = Mathf.Lerp(0.03f, 0.005f, intensity);
    }
}