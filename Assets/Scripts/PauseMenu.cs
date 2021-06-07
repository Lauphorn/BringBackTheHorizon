using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    // Start is called before the first frame update

    public Animator Anim;

    public bool mnu;

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (!mnu)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (Anim.GetBool("Open"))
                {
                    Resume();
                }
                else
                {
                    ShowMenu();
                }
            }
        }

    }

    public void Show(Animator Anim)
    {
        Anim.SetBool("Show", true);
    }

    public void Hide(Animator Anim)
    {
        Anim.SetBool("Show", false);
    }

    public void ShowMenu()
    {
        Anim.SetBool("Open", true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        FpsController.Instance.pause = true;
        CameraController.Instance.pause = true;
    }

    public void Resume()
    {
        Anim.SetBool("Open", false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        FpsController.Instance.pause = false;
        CameraController.Instance.pause = false;
    }

    public void Menu()
    {
        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }

    public void Quit()
    {
        Application.Quit();
    }

}
