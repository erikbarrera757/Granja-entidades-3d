using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public GameObject pausePanel;

    private bool isPaused = false;
    public PausePanelAnimator panelAnimator;
    public PauseBackgroundFade backgroundFade;
    public AudioSource pauseAudio;
    void Update()
    {
        if (GameManager.Instance != null && GameManager.Instance.IsGameOver())
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                Resume();
            else
                Pause();
        }
    }
    public void Pause()
    {

        isPaused = true;
        if (pauseAudio != null)
            pauseAudio.Play();

        panelAnimator.Show();
        StartCoroutine(backgroundFade.FadeIn());

        Time.timeScale = 0f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void Resume()
    {
        isPaused = false;

        panelAnimator.Hide();
        StartCoroutine(backgroundFade.FadeOut());

        Time.timeScale = 1f;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
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