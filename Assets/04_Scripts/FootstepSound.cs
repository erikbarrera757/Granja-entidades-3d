using UnityEngine;

public class FootstepSound : MonoBehaviour
{
    public CharacterController controller;
    public AudioSource footstepAudio;

    public float stepInterval = 0.45f;
    private float stepTimer = 0f;

    void Update()
    {
        if (Cursor.visible)
        {
            if (footstepAudio != null && footstepAudio.isPlaying)
                footstepAudio.Stop();

            stepTimer = 0f;
            return;
        }
        if (controller == null || footstepAudio == null) return;

        Vector3 velocity = controller.velocity;
        velocity.y = 0f;

        bool isWalking = velocity.magnitude > 0.2f;

        if (isWalking)
        {
            stepTimer += Time.deltaTime;

            if (stepTimer >= stepInterval)
            {
                footstepAudio.Stop();
                footstepAudio.Play();
                stepTimer = 0f;
            }
        }
        else
        {
            stepTimer = 0f;

            if (footstepAudio.isPlaying)
            {
                footstepAudio.Stop();
            }
        }
    }
}