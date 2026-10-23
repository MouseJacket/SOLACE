using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;


public class IntroCutscene : MonoBehaviour
{
    public VideoClip intro;
    public string scene_to_load;
    // Start is called before the first frame update
    

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            LoadScene();
        }
    }

     void Start()
    {
        //Start the coroutine we define below named ExampleCoroutine.
        StartCoroutine(ExampleCoroutine());
    }

    IEnumerator ExampleCoroutine()
    {

        yield return new WaitForSeconds((float)intro.length);
        LoadScene();
        

    }
    void LoadScene()
    {
        SceneManager.LoadScene(scene_to_load);
    }
}

