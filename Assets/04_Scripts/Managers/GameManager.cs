using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Text;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int totalEnergy = 0;
    public int stability = 100;
    public int winEnergy = 100;

    public TextMeshProUGUI energyText;
    public TextMeshProUGUI stabilityText;
    public GameObject gameOverText;
    public GameObject restartText;
    public GameObject victoryText;
    public TextMeshProUGUI warningText;
    public float blinkSpeed = 4f;

    private bool warningActive = false;
    private string currentWarningMessage = "";

    public Slider stabilityBar;
    public Image stabilityFillImage;

    // NUEVO: barra suave
    public float displayedStability;
    public float barSmoothSpeed = 5f;

    // NUEVO: flash de daño
    public Image damageFlashImage;
    public float damageFlashDuration = 0.2f;
    private float damageFlashTimer = 0f;

    private bool isGameOver = false;
    private List<string> hostileEntities = new List<string>();
    private List<string> hungryEntities = new List<string>();

    public int food = 3;

    public void AddFood(int amount)
    {
        food += amount;
        UpdateUI();
    }
    public TextMeshProUGUI foodText;
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
        displayedStability = stability;
        UpdateUI();

        if (gameOverText != null) gameOverText.SetActive(false);
        if (restartText != null) restartText.SetActive(false);
        if (victoryText != null) victoryText.SetActive(false);

        if (damageFlashImage != null)
        {
            Color c = damageFlashImage.color;
            c.a = 0f;
            damageFlashImage.color = c;
        }
    }

    void Update()
    {
        if (isGameOver && Input.GetKeyDown(KeyCode.Space))
        {
            RestartGame();
        }

        if (warningActive && warningText != null)
        {
            float alpha = (Mathf.Sin(Time.unscaledTime * blinkSpeed) + 1f) / 2f;
            Color color = warningText.color;
            color.a = Mathf.Lerp(0.3f, 1f, alpha);
            warningText.color = color;
        }

        // NUEVO: suavizado de barra
        if (stabilityBar != null)
        {
            displayedStability = Mathf.Lerp(displayedStability, stability, Time.unscaledDeltaTime * barSmoothSpeed);
            stabilityBar.value = displayedStability;
        }

        // NUEVO: flash rojo de daño
        if (damageFlashImage != null)
        {
            if (damageFlashTimer > 0f)
            {
                damageFlashTimer -= Time.unscaledDeltaTime;

                Color c = damageFlashImage.color;
                c.a = Mathf.Lerp(0f, 0.35f, damageFlashTimer / damageFlashDuration);
                damageFlashImage.color = c;
            }
            else
            {
                Color c = damageFlashImage.color;
                c.a = 0f;
                damageFlashImage.color = c;
            }
        }
    }

    public void AddEnergy(int amount)
    {
        if (isGameOver) return;

        totalEnergy += amount;
        UpdateUI();


    }

    public void LoseStability(int amount)
    {
        if (isGameOver) return;

        stability -= amount;
        if (stability < 0) stability = 0;

        // NUEVO: activar flash
        damageFlashTimer = damageFlashDuration;

        UpdateUI();

        if (stability <= 0)
        {
            GameOver();
        }
    }

    void GameOver()
    {
        isGameOver = true;

        if (gameOverText != null) gameOverText.SetActive(true);
        if (restartText != null) restartText.SetActive(true);

        Time.timeScale = 0f;
        Debug.Log("GAME OVER");
    }

    void Victory()
    {
        isGameOver = true;

        if (victoryText != null) victoryText.SetActive(true);
        if (restartText != null) restartText.SetActive(true);

        Time.timeScale = 0f;
        Debug.Log("VICTORIA");
    }

    void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void UpdateUI()
    {
        if (energyText != null)
            energyText.text = "Energía Total: " + totalEnergy;

        if (stabilityText != null)
            stabilityText.text = "Estabilidad: " + stability;

        if (foodText != null)
            foodText.text = "Comida: " + food;

        if (stabilityFillImage != null)
        {
            if (stability > 60)
                stabilityFillImage.color = Color.green;
            else if (stability > 30)
                stabilityFillImage.color = Color.yellow;
            else
                stabilityFillImage.color = Color.red;
        }
    }

    public void ShowWarning(string message)
    {
        if (warningText != null)
        {
            currentWarningMessage = message;
            warningText.text = currentWarningMessage;
            warningActive = true;
        }
    }

    public void ClearWarning()
    {
        if (warningText != null)
        {
            warningText.text = "";
            warningActive = false;
            currentWarningMessage = "";
        }
    }

    public void RegisterHungryEntity(string entityName)
    {
        if (!hungryEntities.Contains(entityName))
        {
            hungryEntities.Add(entityName);
            UpdateWarningText();
        }
    }

    public void UnregisterHungryEntity(string entityName)
    {
        if (hungryEntities.Contains(entityName))
        {
            hungryEntities.Remove(entityName);
            UpdateWarningText();
        }
    }

    public void RegisterHostileEntity(string entityName)
    {
        if (!hostileEntities.Contains(entityName))
        {
            hostileEntities.Add(entityName);
            UpdateWarningText();
        }
    }

    public void UnregisterHostileEntity(string entityName)
    {
        if (hostileEntities.Contains(entityName))
        {
            hostileEntities.Remove(entityName);
            UpdateWarningText();
        }
    }

    void UpdateWarningText()
    {
        if (warningText == null) return;

        if (hungryEntities.Count == 0 && hostileEntities.Count == 0)
        {
            warningText.text = "";
            return;
        }

        StringBuilder sb = new StringBuilder();

        if (hungryEntities.Count > 0)
        {
            sb.AppendLine("? Entidades hambrientas:");
            foreach (string entity in hungryEntities)
            {
                sb.AppendLine("- " + entity);
            }
            sb.AppendLine();
        }

        if (hostileEntities.Count > 0)
        {
            sb.AppendLine("? Entidades hostiles:");
            foreach (string entity in hostileEntities)
            {
                sb.AppendLine("- " + entity);
            }
        }

        warningText.text = sb.ToString();
    }
    public bool SpendEnergy(int amount)
    {
        if (totalEnergy >= amount)
        {
            totalEnergy -= amount;
            UpdateUI();
            return true;
        }

        return false;
    }
    public bool UseFood(int amount)
    {
        if (food >= amount)
        {
            food -= amount;
            UpdateUI();
            return true;
        }

        return false;
    }

}