using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuUi : MonoBehaviour
{
    // Start is called before the first frame update
    public Image TransitionImg;
    public List<Sprite> TransitionSprite;

    public Menu menuScript;
    public AudioSource Aud;

    public float timer;
    bool TransitionStart;

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

        if (TransitionStart)
        {
            Aud.volume -= 0.005f;
            TransitionImg.sprite = TransitionSprite[Mathf.RoundToInt(menuScript.AO.progress * 100)];
            if (menuScript.AO.progress>=0.9f)
            {
                menuScript.AO.allowSceneActivation = true;
            }
        }
    }

    public void Transition()
    {
        TransitionStart = true;
    }

    public void Show(Animator Anim)
    {
        Anim.SetBool("Show", true);
    }

    public void Hide(Animator Anim)
    {
        Anim.SetBool("Show", false);
    }
}
