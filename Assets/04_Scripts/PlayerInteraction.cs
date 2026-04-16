using UnityEngine;
using TMPro;

public class PlayerInteraction : MonoBehaviour
{
    public float interactionDistance = 3f;
    public TextMeshProUGUI interactionText;

    void Update()
    {
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactionDistance))
        {
            if (hit.collider.CompareTag("Entity"))
            {
                EntityStatus entity = hit.collider.GetComponent<EntityStatus>();

                if (entity != null)
                {
                    interactionText.text =
                        entity.entityName +
                        "\nEstado: " + entity.currentState +
                        "\nHambre: " + entity.hunger +
                        "\nPeligro: " + entity.danger +
                        "\nEnergía: " + entity.energyStored +
                        "\n[E] Alimentar" +
                        "\n[R] Extraer energía";

                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        entity.Feed();
                    }

                    if (Input.GetKeyDown(KeyCode.R))
                    {
                        int energy = entity.ExtractEnergy();

                        if (energy > 0)
                        {
                            GameManager.Instance.AddEnergy(energy);
                        }
                    }
                }
            }
            else
            {
                interactionText.text = "";
            }
        }
        else
        {
            interactionText.text = "";
        }
    }
}