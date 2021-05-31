using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Voice : MonoBehaviour
{
    private static Voice _instance;
    public static Voice Instance { get { return _instance; } }
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }


    public AudioSource ASource;
    public TextMeshProUGUI STitreUi;
    string STitre;

    bool fadeOn;
    Color zm;

    public bool Delayed, IsPlaying;
    public AudioClip AClipDelayed;
    public string STitreDelayed;

    // Start is called before the first frame update
    void Start()
    {
        zm = STitreUi.color;
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsPlaying && Delayed)
        {
            LaunchVoice(AClipDelayed, STitreDelayed);
            Delayed = false;
        }

        if (IsPlaying)
        {
            //STitreUi.color = zm;  //  makes a new color zm

            if (fadeOn)
            {
                //zm.a += 0.01f;
            }

            if (!fadeOn)
            {
                //zm.a = 0;
            }
        }
    }

    public void LaunchVoice(AudioClip Voix, string SousTitre)
    {
        if (!IsPlaying)
        {
            ASource.PlayOneShot(Voix);
            STitre = SousTitre;
            StartCoroutine("PlayText");

        }
        else
        {
            STitreDelayed = SousTitre;
            AClipDelayed = Voix;
            Delayed = true;
        }
    }

    IEnumerator PlayText()
    {
        foreach (char c in STitre)
        {
            IsPlaying = true;

            if (c == '&')
            {
                STitreUi.alignment = TextAlignmentOptions.Left;
            }

            if (c == '=')
            {
                STitreUi.alignment = TextAlignmentOptions.Right;

            }


            if (c == '0')
            {
                yield return new WaitForSeconds(1f);
            }

            if (c == '1')
            {
                STitreUi.text = "";
                fadeOn = false;
            }
            else
            {
                fadeOn = true;
            }

            if (c != '1' && c != '0' && c != '\n' && c != '&' && c != '=')
            {
                STitreUi.text += c;
                //yield return new WaitForSeconds(Random.Range(0, 0.1f));
            }



        }
        yield return new WaitForSeconds(1f);
        STitreUi.text = "";
        IsPlaying = false;
        fadeOn = false;
        zm.a =0;
        //STitreUi.color = zm;
    }
}
