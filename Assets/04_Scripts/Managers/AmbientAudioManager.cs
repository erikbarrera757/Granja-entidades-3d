using UnityEngine;

public class AmbientAudioManager : MonoBehaviour
{
    public AudioSource dayAudio;
    public AudioSource nightAudio;

    public float transitionSpeed = 2f;

    void Start()
    {
        if (dayAudio != null) dayAudio.volume = 1f;
        if (nightAudio != null) nightAudio.volume = 0f;

        if (dayAudio != null) dayAudio.Play();
        if (nightAudio != null) nightAudio.Play();
    }

    void Update()
    {
        if (DayNightCycle.Instance == null) return;

        if (DayNightCycle.Instance.IsNight)
        {
            FadeToNight();
        }
        else
        {
            FadeToDay();
        }
    }

    void FadeToNight()
    {
        if (dayAudio != null)
            dayAudio.volume = Mathf.Lerp(dayAudio.volume, 0f, Time.deltaTime * transitionSpeed);

        if (nightAudio != null)
            nightAudio.volume = Mathf.Lerp(nightAudio.volume, 1f, Time.deltaTime * transitionSpeed);
    }

    void FadeToDay()
    {
        if (dayAudio != null)
            dayAudio.volume = Mathf.Lerp(dayAudio.volume, 1f, Time.deltaTime * transitionSpeed);

        if (nightAudio != null)
            nightAudio.volume = Mathf.Lerp(nightAudio.volume, 0f, Time.deltaTime * transitionSpeed);
    }
}