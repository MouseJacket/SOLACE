using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIActions : MonoBehaviour
{
    [SerializeField] private GameManager gameMan;
    [SerializeField] private List<Button> mainMenuButtons;
    [SerializeField] private List<Button> restartButtons;
    [SerializeField] private List<Button> quitButtons;
    private void Awake()
    {
        gameMan = gameObject.GetComponent<GameManager>();
        foreach (Button button in mainMenuButtons)
        {
            button.onClick.AddListener(QuitToMenu);
        }

        foreach (Button button in quitButtons)
        {
            button.onClick.AddListener(QuitGame);
        }
        foreach (Button button in restartButtons)
        {
            button.onClick.AddListener(Restart);
        }
    }

    private void QuitToMenu()
    {
        gameMan.SceneLoad("MainMenu");
    }

    private void QuitGame()
    {
        Application.Quit();
    }

    private void Restart()
    {
        gameMan.SceneLoad(SceneManager.GetActiveScene().name);
    }
}
