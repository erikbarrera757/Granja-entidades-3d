using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CorralUI : MonoBehaviour
{
    public GameObject panel;

    public TextMeshProUGUI nameText;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI costText;

    public Button upgradeButton;
    public Image upgradeButtonImage;

    private Corral currentCorral;

    public Color activeColor = Color.yellow;
    public Color disabledColor = Color.gray;


    public GameObject confirmPanel;
    public TextMeshProUGUI confirmTitleText;
    public TextMeshProUGUI confirmInfoText;
    public Button confirmButton;
    void Start()
    {
        panel.SetActive(false);
    }

    public void ShowCorral(Corral corral)
    {
        currentCorral = corral;
        panel.SetActive(true);
        UpdateUI();
    }

    public void Hide()
    {
        panel.SetActive(false);
        currentCorral = null;
    }

    void Update()
    {
        if (currentCorral != null)
        {
            UpdateUI();
        }
    }

    void UpdateUI()
    {
        nameText.text = currentCorral.corralName;
        levelText.text = "Nivel: " + currentCorral.level;

        if (!currentCorral.CanUpgrade())
        {
            costText.text = "Nivel máximo";
            upgradeButton.interactable = false;
            upgradeButtonImage.color = disabledColor;
            return;
        }

        int cost = currentCorral.GetUpgradeCost();
        costText.text = "Costo: " + cost;

        if (GameManager.Instance.totalEnergy >= cost)
        {
            upgradeButton.interactable = true;
            upgradeButtonImage.color = activeColor;
        }
        else
        {
            upgradeButton.interactable = false;
            upgradeButtonImage.color = disabledColor;
        }
    }

    public void Upgrade()
    {
        if (currentCorral == null) return;

        int cost = currentCorral.GetUpgradeCost();

        if (GameManager.Instance.SpendEnergy(cost))
        {
            currentCorral.Upgrade();
        }
    }
    public void OpenConfirmPanel()
    {
        if (currentCorral == null) return;

        int nextLevel = currentCorral.level + 1;
        int cost = currentCorral.GetUpgradeCost();

        panel.SetActive(false);          // oculta panel anterior
        confirmPanel.SetActive(true);    // muestra confirmación

        confirmTitleText.text = "Mejorar Corral";

        string benefit = "";

        if (nextLevel == 2)
            benefit = "Nivel 2: reduce el peligro nocturno.";
        else if (nextLevel == 3)
            benefit = "Nivel 3: aumenta la producción de energía.";

        confirmInfoText.text =
            "Nivel actual: " + currentCorral.level +
            "\nNuevo nivel: " + nextLevel +
            "\nCosto: " + cost + " energía" +
            "\n\nMejora:\n" + benefit;
    }
    public void ConfirmUpgrade()
    {
        if (currentCorral == null) return;

        int cost = currentCorral.GetUpgradeCost();

        if (GameManager.Instance.SpendEnergy(cost))
        {
            currentCorral.Upgrade();
            UpdateUI();
        }

        confirmPanel.SetActive(false);
        panel.SetActive(false);
    }
    public void CancelUpgrade()
    {
        confirmPanel.SetActive(false);

        panel.SetActive(true);
    }
}