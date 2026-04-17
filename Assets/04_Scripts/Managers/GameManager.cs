using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

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

    private bool isGameOver = false;

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

        if (gameOverText != null) gameOverText.SetActive(false);
        if (restartText != null) restartText.SetActive(false);
        if (victoryText != null) victoryText.SetActive(false);
    }

    void Update()
    {
        if (isGameOver && Input.GetKeyDown(KeyCode.Space))
        {
            RestartGame();
        }
    }

    public void AddEnergy(int amount)
    {
        if (isGameOver) return;

        totalEnergy += amount;
        UpdateUI();

        if (totalEnergy >= winEnergy)
        {
            Victory();
        }
    }

    public void LoseStability(int amount)
    {
        if (isGameOver) return;

        stability -= amount;
        if (stability < 0) stability = 0;

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

    void UpdateUI()
    {
        if (energyText != null)
            energyText.text = "Energía Total: " + totalEnergy;

        if (stabilityText != null)
            stabilityText.text = "Estabilidad: " + stability;
    }
}