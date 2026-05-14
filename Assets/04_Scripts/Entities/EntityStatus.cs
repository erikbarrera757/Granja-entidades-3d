using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EntityStatus : MonoBehaviour
{
    public string entityName = "Entidad 01";
    public string currentState = "Calmada";

    public int hunger = 0;
    public int danger = 0;
    public int energyStored = 50;

    public float timer = 0f;
    public float interval = 5f;

    public Corral currentCorral;
    public GameObject hungerWarningIcon;
    public Animator entityAnimator;
    public bool isDead = false;

    private Renderer entityRenderer;

    public Slider hungerBar;
    public Slider dangerBar;
    public TMPro.TextMeshProUGUI energyText;
    public LightReactionType lightReaction = LightReactionType.Calm;

    public enum LightReactionType
    {
        Calm,
        Angry
    }

    void Start()
    {
        entityRenderer = GetComponent<Renderer>();

        if (hungerWarningIcon != null)
            hungerWarningIcon.SetActive(false);

        UpdateState();
    }

    void Update()
    {
        if (isDead) return;

        timer += Time.deltaTime;

        if (timer >= interval)
        {
            IncreaseNeeds();
            timer = 0f;
        }
    }

    public void Feed()
    {
        if (isDead) return;
        if (GameManager.Instance == null) return;

        if (!GameManager.Instance.UseEntityFood(1))
        {
            if (PlayerInteraction.Instance != null)
            {
                PlayerInteraction.Instance.ShowTemporaryMessage("No tienes comida para entidades");
            }

            Debug.Log("No tienes comida para entidades");
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
        if (isDead) return 0;

        if (energyStored > 0)
        {
            int extracted = energyStored;
            energyStored = 0;

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

            Debug.Log("Extrajiste toda la energía de " + entityName + ": " + extracted);
            return extracted;
        }

        Debug.Log(entityName + " no tiene energía disponible.");
        return 0;
    }

    public void IncreaseNeeds()
    {
        if (isDead) return;

        bool isNight = DayNightCycle.Instance != null && DayNightCycle.Instance.IsNight;

        if (isNight)
        {
            hunger += 10;
            if (hunger > 100) hunger = 100;

            if (hunger >= 100)
            {
                Die();
                return;
            }

            int dangerGain = 10;

            if (currentCorral != null && currentCorral.level >= 2)
            {
                dangerGain -= 3;
            }

            if (dangerGain < 0)
                dangerGain = 0;

            danger += dangerGain;
            if (danger > 100) danger = 100;

            energyStored += 2;
            if (energyStored > 100) energyStored = 100;
        }
        else
        {
            hunger += 5;
            if (hunger > 100) hunger = 100;

            if (hunger >= 100)
            {
                Die();
                return;
            }

            danger -= 15;
            if (danger < 0) danger = 0;

            int energyGain = 10;

            if (currentCorral != null && currentCorral.level >= 3)
            {
                energyGain = 20;
            }

            energyStored += energyGain;
            if (energyStored > 100) energyStored = 100;
        }

        UpdateState();
    }

    void UpdateState()
    {
        if (energyText != null)
        {
            energyText.text = "Energía: " + energyStored;
        }
        if (hungerBar != null)
            hungerBar.value = hunger;

        if (dangerBar != null)
            dangerBar.value = danger;
        if (isDead) return;

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

        if (hungerWarningIcon != null)
        {
            hungerWarningIcon.SetActive(currentState == "Hambrienta");
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

    void Die()
    {
        if (isDead) return;

        isDead = true;

        if (GameManager.Instance != null)
        {
            GameManager.Instance.UnregisterHungryEntity(entityName);
            GameManager.Instance.UnregisterHostileEntity(entityName);
        }

        if (currentCorral != null)
        {
            currentCorral.RemoveEntity(gameObject);
        }

        if (hungerWarningIcon != null)
        {
            hungerWarningIcon.SetActive(false);
        }

        Debug.Log(entityName + " murió por hambre.");
        if (entityAnimator != null)
        {
            EntityMovement movement = GetComponent<EntityMovement>();
            if (movement != null)
            {
                movement.canMove = false;
            }

            EntityAttack attack = GetComponent<EntityAttack>();
            if (attack != null)
            {
                attack.enabled = false;
            }
            entityAnimator.SetTrigger("die");
        }
        StartCoroutine(DeathRoutine());
    }

    public void RepelWithCrucifix(Transform player)
    {
        if (isDead) return;

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
        if (isDead) return;

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
    System.Collections.IEnumerator DeathRoutine()
    {
        yield return new WaitForSeconds(3f);

        Destroy(gameObject);
    }
}