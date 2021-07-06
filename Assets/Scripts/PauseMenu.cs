using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    // Start is called before the first frame update

    public Animator Anim;
    public Animator settings_;

    private Exposure e;
    public Volume volume;
    public Slider slider;
    public bool mnu;

    void Start()
    {
        volume.sharedProfile.TryGet<Exposure>(out e);
    }

    // Update is called once per frame
    void Update()
    {

        e.fixedExposure.value = -slider.value;

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

            if (settings_.GetBool("Open"))
            {
                SettingsOff();
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

    public void Settings()
    {
        settings_.SetBool("Open", true);
        Anim.SetBool("Show", false);
    }
    public void SettingsOff()
    {
        settings_.SetBool("Open", false);
        Anim.SetBool("Show", true);
    }

    public void Quit()
    {
        Application.Quit();
    }

}
