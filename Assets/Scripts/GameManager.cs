using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    public float gravMultiplier;
    public float distMultiplier;
    public bool onFoot;
    public float hazardDamage;
    public int pieceCount;
    [SerializeField]
    private TextMeshProUGUI piecePrompt;
    [SerializeField]
    public GameObject pauseMenu;
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private GameObject winScreen;
    [SerializeField] private GameObject playerUI;
    [SerializeField] private GameObject rocketUI;
    [SerializeField] private Image healthBar;
    public float maxHealth;
    public float health;

    public bool pause;
    

    // Start is called before the first frame update
    void Start()
    {
        pieceCount = 0;
        health = maxHealth;
        pause = false;
        Application.targetFrameRate = 60;
        Time.timeScale = 1;
    }

    // Update is called once per frame
    void Update()
    {
        piecePrompt.text = $"Pieces - {pieceCount}/3";
        if (pieceCount >= 3)
        {
            GameEnd(winScreen);
        }
        float healthPercentage = health/maxHealth;
        healthBar.fillAmount = healthPercentage;
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (pause)
            {
                pauseMenu.SetActive(false);
                Time.timeScale = 1;
                pause = false;
            }
            else
            {
                pauseMenu.SetActive(true);
                Time.timeScale = 0;
                pause = true;
            }
        }
        if (health == 0)
        {
            GameEnd(gameOverScreen);
        }
    }
    public void SceneLoad(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void GameOver()
    {
        GameEnd(gameOverScreen);
    }
    private void GameEnd(GameObject resultScreen)
    {
        resultScreen.SetActive(true);
        playerUI.SetActive(false);
        rocketUI.SetActive(false);
        Time.timeScale = 0f;
    }
}
