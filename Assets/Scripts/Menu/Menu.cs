using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Menu : MonoBehaviour
{
    public AsyncOperation AO;

    // Start is called before the first frame update
    void Start()
    {
        Application.backgroundLoadingPriority = ThreadPriority.Normal;

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeScene()
    {
        StartCoroutine(LoadYourAsyncScene());
    }

    public void Quit()
    {
        Application.Quit();
    }

    IEnumerator LoadYourAsyncScene()
    {
        AO = SceneManager.LoadSceneAsync(1, LoadSceneMode.Single);
        AO.allowSceneActivation = false;
        return null;

    }
}
