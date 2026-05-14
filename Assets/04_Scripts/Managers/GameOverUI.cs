using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    public GameObject gameOverPanel;
    public CanvasGroup canvasGroup;
    public RectTransform panelRect;

    public AudioSource gameOverAudio;

    public float fadeSpeed = 3f;
    public float scaleSpeed = 8f;

    public Vector3 startScale = new Vector3(0.7f, 0.7f, 0.7f);
    public Vector3 endScale = Vector3.one;

    void Start()
    {
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        if (canvasGroup != null)
            canvasGroup.alpha = 0f;

        if (panelRect != null)
            panelRect.localScale = startScale;
    }

    public void ShowGameOver()
    {
        gameOverPanel.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (gameOverAudio != null)
            gameOverAudio.Play();

        StartCoroutine(GameOverAnimation());

        Time.timeScale = 0f;
    }

    IEnumerator GameOverAnimation()
    {
        float alpha = 0f;

        if (canvasGroup != null)
            canvasGroup.alpha = 0f;

        if (panelRect != null)
            panelRect.localScale = startScale;

        while (alpha < 1f)
        {
            alpha += Time.unscaledDeltaTime * fadeSpeed;

            if (canvasGroup != null)
                canvasGroup.alpha = alpha;

            if (panelRect != null)
            {
                panelRect.localScale = Vector3.Lerp(
                    panelRect.localScale,
                    endScale,
                    Time.unscaledDeltaTime * scaleSpeed
                );
            }

            yield return null;
        }

        if (canvasGroup != null)
            canvasGroup.alpha = 1f;

        if (panelRect != null)
            panelRect.localScale = endScale;
    }

    public void Retry()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GoToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}