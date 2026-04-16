using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int totalEnergy = 0;
    public TextMeshProUGUI energyText;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        UpdateUI();
    }

    public void AddEnergy(int amount)
    {
        totalEnergy += amount;
        UpdateUI();
    }

    void UpdateUI()
    {
        if (energyText != null)
        {
            energyText.text = "Energía Total: " + totalEnergy;
        }
    }
}