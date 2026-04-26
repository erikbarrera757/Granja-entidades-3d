
using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    public static DayNightCycle Instance;

    public Light sun;

    [Range(0f, 24f)]
    public float timeOfDay = 12f;

    public float dayDuration = 60f;

    public Color dayColor = Color.white;
    public Color nightColor = new Color(0.005f, 0.005f, 0.02f);

    public bool IsNight => timeOfDay < 6f || timeOfDay >= 18f;
    public bool IsDay => !IsNight;

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        timeOfDay += (24f / dayDuration) * Time.deltaTime;

        if (timeOfDay >= 24f)
            timeOfDay = 0f;

        UpdateLighting();
    }

    void UpdateLighting()
    {
        float normalizedTime = timeOfDay / 24f;

        sun.transform.rotation = Quaternion.Euler((normalizedTime * 360f) - 90f, 170f, 0);

        float intensity = Mathf.Clamp01(Mathf.Cos((normalizedTime - 0.25f) * Mathf.PI * 2));

        sun.intensity = Mathf.Lerp(0.005f, 1.0f, intensity); ;

        RenderSettings.ambientLight = Color.Lerp(nightColor, dayColor, intensity);
    }
}
//void UpdateLighting()
//{
//    float sunRotation = (timeOfDay / 24f) * 360f;
//    directionalLight.transform.rotation = Quaternion.Euler(sunRotation - 90f, 170f, 0f);

//    float normalizedTime = timeOfDay / 24f;
//    float intensityMultiplier = Mathf.Clamp01(Mathf.Cos((normalizedTime - 0.25f) * 2f * Mathf.PI) * 1.2f);

//    directionalLight.intensity = Mathf.Lerp(0.05f, 1.2f, intensityMultiplier);
//    RenderSettings.ambientLight = Color.Lerp(nightAmbientColor, dayAmbientColor, intensityMultiplier);
//}