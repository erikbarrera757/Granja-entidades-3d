using UnityEngine;

public class ButtonClickSound : MonoBehaviour
{
    public AudioSource audioSource;

    public void PlayClick()
    {
        if (audioSource != null)
        {
            audioSource.Play();
        }
    }
}