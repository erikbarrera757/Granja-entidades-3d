using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class ShopUIManager : MonoBehaviour
{
    [Header("Paneles")]
    public GameObject entitiesPanel;
    public GameObject foodPanel;

    [Header("Texto energía")]
    public TextMeshProUGUI shopEnergyText;

    [Header("Botones de pestañas")]
    public Image entitiesTabImage;
    public Image foodTabImage;

    public Color activeTabColor = Color.white;
    public Color inactiveTabColor = new Color(1f, 1f, 1f, 0.45f);

    [Header("Botones de compra")]
    public Button buyEntity1Button;
    public Button buyEntity2Button;
    public Button buyEntity3Button;

    public Button buySmallFoodButton;
    public Button buyMediumFoodButton;
    public Button buyLargeFoodButton;
    public Button buyEntityFoodButton;

    [Header("Costos")]
    public ShopManager shopManager;

    [Header("Sonido")]
    public AudioSource tabAudio;

    [Header("Animación")]
    public float animDuration = 0.15f;
    public Vector3 hiddenScale = new Vector3(0.96f, 0.96f, 0.96f);
    public Vector3 visibleScale = Vector3.one;

    private GameObject currentPanel;

    void Start()
    {
        ShowEntities();
    }

    void Update()
    {
        UpdateShopUI();
    }

    public void ShowEntities()
    {
        if (tabAudio != null) tabAudio.Play();

        entitiesPanel.SetActive(true);
        foodPanel.SetActive(false);

        currentPanel = entitiesPanel;

        if (entitiesTabImage != null) entitiesTabImage.color = activeTabColor;
        if (foodTabImage != null) foodTabImage.color = inactiveTabColor;

        AnimatePanel(entitiesPanel);
    }

    public void ShowFood()
    {
        if (tabAudio != null) tabAudio.Play();

        entitiesPanel.SetActive(false);
        foodPanel.SetActive(true);

        currentPanel = foodPanel;

        if (entitiesTabImage != null) entitiesTabImage.color = inactiveTabColor;
        if (foodTabImage != null) foodTabImage.color = activeTabColor;

        AnimatePanel(foodPanel);
    }

    void UpdateShopUI()
    {
        if (GameManager.Instance == null || shopManager == null) return;

        int energy = GameManager.Instance.totalEnergy;

        if (shopEnergyText != null)
            shopEnergyText.text = "Energía: " + energy;

        SetButtonState(buyEntity1Button, energy >= shopManager.entity1Cost);
        SetButtonState(buyEntity2Button, energy >= shopManager.entity2Cost);
        SetButtonState(buyEntity3Button, energy >= shopManager.entity3Cost);

        SetButtonState(buySmallFoodButton, energy >= shopManager.smallFoodCost);
        SetButtonState(buyMediumFoodButton, energy >= shopManager.mediumFoodCost);
        SetButtonState(buyLargeFoodButton, energy >= shopManager.largeFoodCost);
        SetButtonState(buyEntityFoodButton, energy >= shopManager.entityFoodCost);
    }

    void SetButtonState(Button button, bool canBuy)
    {
        if (button == null) return;

        button.interactable = canBuy;

        Image img = button.GetComponent<Image>();

        if (img != null)
        {
            img.color = canBuy
                ? Color.white
                : new Color(0.45f, 0.45f, 0.45f, 0.75f);
        }
    }

    void AnimatePanel(GameObject panel)
    {
        if (panel == null) return;

        StopAllCoroutines();
        StartCoroutine(PanelScaleAnimation(panel.transform));
    }

    IEnumerator PanelScaleAnimation(Transform panelTransform)
    {
        float timer = 0f;

        panelTransform.localScale = hiddenScale;

        while (timer < animDuration)
        {
            timer += Time.unscaledDeltaTime;
            float t = timer / animDuration;

            panelTransform.localScale = Vector3.Lerp(hiddenScale, visibleScale, t);

            yield return null;
        }

        panelTransform.localScale = visibleScale;
    }
}