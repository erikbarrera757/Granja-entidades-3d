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

    [Header("Entidades disponibles")]
    public GameObject entity1Prefab;
    public GameObject entity2Prefab;
    public GameObject entity3Prefab;

    public int entity1Cost = 80;
    public int entity2Cost = 120;
    public int entity3Cost = 150;
    public int entityFoodCost = 20;

    [Header("Comida para granjero")]
    public int smallFoodCost = 20;
    public int mediumFoodCost = 40;
    public int largeFoodCost = 70;

    public PlayerInteraction playerInteraction;
    void Update()
    {
        if (GameManager.Instance != null && GameManager.Instance.IsGameOver())
            return;

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

        if (shopOpen)
        {
            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Time.timeScale = 1f;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
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
    //nueva tienda
    public void BuyEntity1()
    {
        BuyEntity(entity1Prefab, entity1Cost);
    }

    public void BuyEntity2()
    {
        BuyEntity(entity2Prefab, entity2Cost);
    }

    public void BuyEntity3()
    {
        BuyEntity(entity3Prefab, entity3Cost);
    }

    void BuyEntity(GameObject prefab, int cost)
    {
        if (prefab == null)
        {
            Debug.Log("No hay prefab asignado.");
            return;
        }

        Corral availableCorral = GetAvailableCorral();

        if (availableCorral == null)
        {
            Debug.Log("No hay espacio en los corrales.");
            return;
        }

        if (GameManager.Instance.SpendEnergy(cost))
        {
            availableCorral.AddEntity(prefab);
            Debug.Log("Compraste una entidad.");
        }
        else
        {
            Debug.Log("No tienes suficiente energía.");
        }
    }
    public void BuySmallFarmerFood()
    {
        if (GameManager.Instance.SpendEnergy(smallFoodCost))
        {
            playerInteraction.smallFoodCount++;
            Debug.Log("Compraste comida pequeña");
        }
        else
        {
            Debug.Log("No tienes suficiente energía");
        }
    }

    public void BuyMediumFarmerFood()
    {
        if (GameManager.Instance.SpendEnergy(mediumFoodCost))
        {
            playerInteraction.mediumFoodCount++;
            Debug.Log("Compraste comida mediana");
        }
        else
        {
            Debug.Log("No tienes suficiente energía");
        }
    }

    public void BuyLargeFarmerFood()
    {
        if (GameManager.Instance.SpendEnergy(largeFoodCost))
        {
            playerInteraction.largeFoodCount++;
            Debug.Log("Compraste comida grande");
        }
        else
        {
            Debug.Log("No tienes suficiente energía");
        }
    }
    public void BuyEntityFood()
    {
        if (GameManager.Instance.SpendEnergy(entityFoodCost))
        {
            GameManager.Instance.AddEntityFood(1);
            Debug.Log("Compraste comida para entidades");
        }
        else
        {
            Debug.Log("No tienes suficiente energía");
        }
    }

}