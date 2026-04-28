using UnityEngine;
using TMPro;

public class PlayerInteraction : MonoBehaviour
{
    public static PlayerInteraction Instance;

    public float interactionDistance = 3f;
    public TextMeshProUGUI interactionText;

    public bool hasCrucifix = false;
    public bool hasFlashlight = false;

    public Light flashlight;
    public HotbarUI hotbarUI;

    private string temporaryMessage = "";
    private bool showingTemporaryMessage = false;
    private bool cursorUnlocked = false;
    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        if (flashlight != null)
        {
            flashlight.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            ToggleCursor();
        }
        int currentSlot = -1;

        if (hotbarUI != null)
        {
            currentSlot = hotbarUI.GetSelectedSlot();
        }

        // Linterna solo funciona si está en el slot 1
        if (flashlight != null)
        {
            flashlight.gameObject.SetActive(hasFlashlight && currentSlot == 0 && Input.GetMouseButton(0));
        }

        // Mensajes temporales
        if (showingTemporaryMessage)
        {
            if (interactionText != null)
                interactionText.text = temporaryMessage;
            return;
        }

        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        RaycastHit hit;

        CorralUI corralUI = Object.FindFirstObjectByType<CorralUI>();

        if (Physics.Raycast(ray, out hit, interactionDistance))
        {
            Debug.Log("Raycast golpeó: " + hit.collider.name);
            // Detectar corral
            Corral corral = hit.collider.GetComponentInParent<Corral>();

            if (corral != null)
            {
                if (corralUI != null)
                    corralUI.ShowCorral(corral);

                if (interactionText != null)
                    interactionText.text = "";

                return;
            }
            else
            {
                if (corralUI != null)
                    corralUI.Hide();
            }

            // Detectar entidad
            if (hit.collider.CompareTag("Entity"))
            {
                EntityStatus entity = hit.collider.GetComponent<EntityStatus>();

                if (entity != null)
                {
                    string actionsText =
                        entity.entityName +
                        "\nEstado: " + entity.currentState +
                        "\nHambre: " + entity.hunger +
                        "\nPeligro: " + entity.danger +
                        "\nEnergía: " + entity.energyStored +
                        "\n[E] Alimentar" +
                        "\n[R] Extraer energía";

                    if (hasFlashlight && currentSlot == 0)
                    {
                        actionsText += "\n[Clic] Usar linterna";
                    }

                    if (hasCrucifix && currentSlot == 1 && entity.currentState == "Hostil")
                    {
                        actionsText += "\n[Clic] Usar crucifijo";
                    }

                    if (interactionText != null)
                        interactionText.text = actionsText;

                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        entity.Feed();
                    }

                    if (Input.GetKeyDown(KeyCode.R))
                    {
                        int energy = entity.ExtractEnergy();

                        if (energy > 0 && GameManager.Instance != null)
                        {
                            GameManager.Instance.AddEnergy(energy);
                        }
                    }

                    if (hasFlashlight && currentSlot == 0 && Input.GetMouseButton(0))
                    {
                        entity.ReactToLight();
                    }

                    if (hasCrucifix && currentSlot == 1 && Input.GetMouseButtonDown(0) && entity.currentState == "Hostil")
                    {
                        entity.RepelWithCrucifix(transform);
                    }
                }
            }
            else
            {
                if (interactionText != null)
                    interactionText.text = "";
            }
        }
        else
        {
            if (corralUI != null)
                corralUI.Hide();

            if (interactionText != null)
                interactionText.text = "";
        }
    }

    public void ShowTemporaryMessage(string message)
    {
        temporaryMessage = message;
        showingTemporaryMessage = true;
    }

    public void HideTemporaryMessage()
    {
        temporaryMessage = "";
        showingTemporaryMessage = false;

        if (interactionText != null)
        {
            interactionText.text = "";
        }
    }
    void ToggleCursor()
    {
        cursorUnlocked = !cursorUnlocked;

        if (cursorUnlocked)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}