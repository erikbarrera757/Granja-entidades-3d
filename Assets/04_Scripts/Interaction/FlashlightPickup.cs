using UnityEngine;

public class FlashlightPickup : MonoBehaviour
{
    public string pickupMessage = "Presiona E para recoger la linterna";
    private bool playerInside = false;

    void Update()
    {
        if (playerInside && Input.GetKeyDown(KeyCode.E))
        {
            if (PlayerInteraction.Instance != null)
            {
                PlayerInteraction.Instance.hasFlashlight = true;
                PlayerInteraction.Instance.HideTemporaryMessage();

                if (PlayerInteraction.Instance.flashlight != null)
                {
                    PlayerInteraction.Instance.flashlight.gameObject.SetActive(false);
                }

                Debug.Log("Recogiste la linterna.");
            }

            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = true;

            if (PlayerInteraction.Instance != null)
            {
                PlayerInteraction.Instance.ShowTemporaryMessage(pickupMessage);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false;

            if (PlayerInteraction.Instance != null)
            {
                PlayerInteraction.Instance.HideTemporaryMessage();
            }
        }
    }
}