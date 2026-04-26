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

    public LightReactionType lightReaction = LightReactionType.Calm;

    public enum LightReactionType
    {
        Calm,
        Angry
    }

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
        if (!GameManager.Instance.UseFood(1))
        {
            Debug.Log("No tienes comida");
            return;
        }

        hunger -= 20;
        if (hunger < 0) hunger = 0;

        if (DayNightCycle.Instance != null && DayNightCycle.Instance.IsNight)
        {
            danger += 3;
            if (danger > 100) danger = 100;
        }

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

            // Extraer energía tiene más riesgo de noche
            if (DayNightCycle.Instance != null && DayNightCycle.Instance.IsNight)
            {
                danger += 15;
            }
            else
            {
                danger += 5;
            }

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
        bool isNight = DayNightCycle.Instance != null && DayNightCycle.Instance.IsNight;

        if (isNight)
        {
            // NOCHE: se vuelven peligrosas y hambrientas
            hunger += 15;
            if (hunger > 100) hunger = 100;

            danger += 10;
            if (danger > 100) danger = 100;

            // De noche producen poco
            energyStored += 2;
            if (energyStored > 100) energyStored = 100;
        }
        else
        {
            // DÍA: están tranquilas
            hunger -= 10;
            if (hunger < 0) hunger = 0;

            danger -= 15;
            if (danger < 0) danger = 0;

            // De día producen más energía
            energyStored += 10;
            if (energyStored > 100) energyStored = 100;
        }

        UpdateState();
    }

    void UpdateState()
    {
        bool isNight = DayNightCycle.Instance != null && DayNightCycle.Instance.IsNight;

        if (!isNight)
        {
            currentState = "Calmada";
        }
        else
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
        }

        UpdateColor();
        CheckWarning();
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

    void CheckWarning()
    {
        if (GameManager.Instance == null) return;

        if (currentState == "Hambrienta")
        {
            GameManager.Instance.RegisterHungryEntity(entityName);
            GameManager.Instance.UnregisterHostileEntity(entityName);
        }
        else if (currentState == "Hostil")
        {
            GameManager.Instance.UnregisterHungryEntity(entityName);
            GameManager.Instance.RegisterHostileEntity(entityName);
        }
        else
        {
            GameManager.Instance.UnregisterHungryEntity(entityName);
            GameManager.Instance.UnregisterHostileEntity(entityName);
        }
    }

    public void RepelWithCrucifix(Transform player)
    {
        danger -= 35;
        if (danger < 0) danger = 0;

        hunger += 10;
        if (hunger > 100) hunger = 100;

        Vector3 pushDirection = (transform.position - player.position).normalized;
        pushDirection.y = 0f;

        transform.position += pushDirection * 2.5f;

        UpdateState();
        Debug.Log(entityName + " fue repelida con el crucifijo.");
    }

    public void ReactToLight()
    {
        if (currentState == "Hostil")
        {
            if (lightReaction == LightReactionType.Calm)
            {
                danger -= 1;
                if (danger < 0) danger = 0;
            }
            else if (lightReaction == LightReactionType.Angry)
            {
                danger += 1;
                if (danger > 100) danger = 100;
            }

            UpdateState();
        }
    }
}