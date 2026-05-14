using UnityEngine;

public class EntityScream : MonoBehaviour
{
    public EntityStatus entityStatus;
    public AudioSource screamAudio;

    public float screamInterval = 6f;
    public float randomOffset = 2f;

    private float timer = 0f;
    private float nextScreamTime = 0f;

    void Start()
    {
        if (entityStatus == null)
            entityStatus = GetComponent<EntityStatus>();

        SetNextScreamTime();
    }

    void Update()
    {
        if (entityStatus == null || screamAudio == null) return;

        if (entityStatus.currentState == "Hostil")
        {
            timer += Time.deltaTime;

            if (timer >= nextScreamTime)
            {
                screamAudio.Stop();
                screamAudio.Play();

                timer = 0f;
                SetNextScreamTime();
            }
        }
        else
        {
            timer = 0f;
        }
    }

    void SetNextScreamTime()
    {
        nextScreamTime = screamInterval + Random.Range(-randomOffset, randomOffset);

        if (nextScreamTime < 1f)
            nextScreamTime = 1f;
    }
}