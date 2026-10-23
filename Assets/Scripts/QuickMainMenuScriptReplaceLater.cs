using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class QuickMainMenuScriptReplaceLater : MonoBehaviour
{
    public Button start;
    public Button exit;
    public string mainScene;
    // Start is called before the first frame update
    void Start()
    {
        exit.onClick.AddListener(Exit);
        start.onClick.AddListener(Load);
    }

    void Exit()
    {
        Application.Quit();
    }
    void Load()
    {
        Debug.Log("click");
        SceneManager.LoadScene(mainScene);
    }
}
