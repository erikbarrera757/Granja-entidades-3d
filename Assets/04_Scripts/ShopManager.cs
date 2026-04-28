using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public GameObject shopPanel;

    public int foodCost = 20;
    public int entityCost = 80;
    public int upgradeCost = 60;

    private bool shopOpen = false;
    public GameObject entityPrefab;
    public Corral[] corrals;
    public int corralUpgradeCost = 50;
    public Corral selectedCorral;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            ToggleShop();
        }
    }

    void ToggleShop()
    {
        shopOpen = !shopOpen;

        if (shopPanel != null)
            shopPanel.SetActive(shopOpen);

        Cursor.lockState = shopOpen ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = shopOpen;
    }

    public void BuyFood()
    {
        if (GameManager.Instance.SpendEnergy(foodCost))
        {
            GameManager.Instance.AddFood(1); // ?? AQUÍ
            Debug.Log("Compraste comida");
        }
    }

    public void BuyEntity()
    {
        Corral availableCorral = GetAvailableCorral();

        if (availableCorral == null)
        {
            Debug.Log("No hay espacio en los corrales");
            return;
        }

        if (GameManager.Instance.SpendEnergy(entityCost))
        {
            availableCorral.AddEntity(entityPrefab);
            Debug.Log("Compraste una entidad");
        }
        else
        {
            Debug.Log("No tienes suficiente energía");
        }
    }

    Corral GetAvailableCorral()
    {
        foreach (Corral corral in corrals)
        {
            if (corral.HasSpace())
            {
                return corral;
            }
        }

        return null;
    }

    public void UpgradeSecurity()
    {
        if (GameManager.Instance.SpendEnergy(upgradeCost))
        {
            Debug.Log("Seguridad mejorada");
        }
        else
        {
            Debug.Log("No tienes suficiente energía");
        }
    }
    public void UpgradeCorral()
    {
        if (selectedCorral == null)
        {
            Debug.Log("Selecciona un corral");
            return;
        }

        if (GameManager.Instance.SpendEnergy(corralUpgradeCost))
        {
            selectedCorral.Upgrade();
        }
    }
}