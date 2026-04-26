using UnityEngine;

public class CrucifixPickup : MonoBehaviour
{
    public string pickupMessage = "Presiona E para recoger el crucifijo";
    private bool playerInside = false;

    void Update()
    {
        if (playerInside && Input.GetKeyDown(KeyCode.E))
        {
            if (PlayerInteraction.Instance != null)
            {
                PlayerInteraction.Instance.hasCrucifix = true;
                PlayerInteraction.Instance.HideTemporaryMessage();
                Debug.Log("Recogiste el crucifijo.");
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