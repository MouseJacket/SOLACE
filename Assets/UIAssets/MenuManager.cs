using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [Header("Menu Panels")]
    public GameObject mainMenu;
    public GameObject settingsMenu;
    public GameObject deathMenu;

    [Header("Gameplay UI")]
    public GameObject hudElements; // Drag your HUD (Health/Minimap) here

    void Start()
    {
        // Ensure the game starts in "Menu Mode"
        ShowMainMenu();
    }

    // 1. Logic to Start the Game
    public void PlayGame()
    {
        mainMenu.SetActive(false);    // Hide Menu
        settingsMenu.SetActive(false);
        hudElements.SetActive(true);   // Show HUD

        Time.timeScale = 1f;           // Unpause the 3D world
        Cursor.lockState = CursorLockMode.Locked; // Optional: Lock mouse for 3D gameplay
        Cursor.visible = false;
    }

    // 2. Navigation Functions
    public void OpenSettings()
    {
        mainMenu.SetActive(false);
        settingsMenu.SetActive(true);
    }

    public void ShowMainMenu()
    {
        mainMenu.SetActive(true);
        settingsMenu.SetActive(false);
        hudElements.SetActive(false); // Hide HUD while in menu

        Time.timeScale = 0f;          // Pause the 3D world
        Cursor.lockState = CursorLockMode.None; // Make mouse visible
        Cursor.visible = true;
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Game is exiting...");
    }
}