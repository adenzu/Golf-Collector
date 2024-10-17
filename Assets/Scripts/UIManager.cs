using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [SerializeField] private Slider healthSlider;
    [SerializeField] private TMPro.TextMeshProUGUI healthText;
    [SerializeField] private TMPro.TextMeshProUGUI scoreText;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button resumeButton;
    [SerializeField] private GameObject pauseMenu;

    private void Awake()
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

    private void Start()
    {
        restartButton.onClick.AddListener(GameManager.Instance.RestartGame);
        resumeButton.onClick.AddListener(ResumeGame);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    private void TogglePause()
    {
        pauseMenu.SetActive(!pauseMenu.activeSelf);
        if (pauseMenu.activeSelf)
        {
            GameManager.Instance.PauseGame();
        }
        else
        {
            GameManager.Instance.ResumeGame();
        }
    }

    private void ResumeGame()
    {
        pauseMenu.SetActive(false);
        GameManager.Instance.ResumeGame();
    }

    public void UpdateHealthUI(int currentHealth, int maxHealth)
    {
        healthSlider.value = (float)currentHealth / maxHealth;
        if (currentHealth <= 0)
        {
            healthText.text = "Dead!";
        }
    }

    public void UpdateScoreUI(int score)
    {
        scoreText.text = $"Score\n{score}";
    }
}
