using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class Bureau : MonoBehaviour
{
    public GameObject Screen;
    public VideoPlayer Video;
    public Vision visionscript;

    public Follow followscript;
    public Transform followpos;
    Transform oldfollow;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnScreen()
    {
        Screen.SetActive(true);
        Video.Play();
    }

    public void Pause()
    {
        Video.Pause();
    }

    public void PlayVideo()
    {
        Video.Play();
    }

    public void changefollow()
    {
        oldfollow = followscript.TargetPosition;
        followscript.Damp = 0.05f;
        followscript.TargetPosition = followpos;
    }
    public void FollowNormal()
    {
        followscript.Damp = 0.01f;
        followscript.TargetPosition = oldfollow;
    }
    public void followdamp()
    {
        followscript.Damp = 1f;
    }

    public void ChangeVision()
    {
        visionscript.ChangeVision(0.9f);
    }
}
