using UnityEngine;

public class EntityStatus : MonoBehaviour
{
    public string entityName = "Entidad 01";
    public string currentState = "Calmada";
    public int hunger = 0;
    public int danger = 0;
    public int energyStored = 50;

    public float timer = 0f;
    public float interval = 5f;

    private Renderer entityRenderer;

    void Start()
    {
        entityRenderer = GetComponent<Renderer>();
        UpdateState();
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= interval)
        {
            IncreaseNeeds();
            timer = 0f;
        }
    }

    public void Feed()
    {
        hunger -= 20;
        if (hunger < 0) hunger = 0;

        danger += 5;
        if (danger > 100) danger = 100;

        UpdateState();
        Debug.Log(entityName + " fue alimentada.");
    }

    public int ExtractEnergy()
    {
        if (energyStored > 0)
        {
            int extracted = 10;
            energyStored -= extracted;

            if (energyStored < 0)
                energyStored = 0;

            danger += 15;
            if (danger > 100) danger = 100;

            UpdateState();
            Debug.Log("Extrajiste energía de " + entityName);
            return extracted;
        }

        Debug.Log(entityName + " no tiene energía disponible.");
        return 0;
    }

    public void IncreaseNeeds()
    {
        hunger += 10;
        if (hunger > 100) hunger = 100;

        danger += 2;
        if (danger > 100) danger = 100;

        if (energyStored < 100)
            energyStored += 5;

        UpdateState();
    }

    void UpdateState()
    {
        if (danger >= 70)
        {
            currentState = "Hostil";
        }
        else if (hunger >= 50)
        {
            currentState = "Hambrienta";
        }
        else
        {
            currentState = "Calmada";
        }

        UpdateColor();
    }

    void UpdateColor()
    {
        if (entityRenderer == null) return;

        if (currentState == "Calmada")
        {
            entityRenderer.material.color = Color.green;
        }
        else if (currentState == "Hambrienta")
        {
            entityRenderer.material.color = Color.yellow;
        }
        else if (currentState == "Hostil")
        {
            entityRenderer.material.color = Color.red;
        }
    }
}