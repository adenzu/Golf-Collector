using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public bool IsPaused { get; private set; }

    public delegate void OnPause(bool isPaused);
    public event OnPause OnPauseEvent;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            PauseGame();
        }
        else
        {
            Destroy(gameObject);
        }

    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        SetIsPaused(true);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        SetIsPaused(false);
    }

    public void RestartGame()
    {
        IsPaused = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    private void SetIsPaused(bool isPaused)
    {
        IsPaused = isPaused;
        OnPauseEvent?.Invoke(IsPaused);
    }
}
