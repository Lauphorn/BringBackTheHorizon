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
        zm.a = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsPlaying && Delayed)
        {
            LaunchVoice(AClipDelayed, STitreDelayed);
            Delayed = false;
        }

        STitreUi.color = zm;  //  makes a new color zm

        if (IsPlaying)
        {
            if (fadeOn && zm.a<=1)
            {
                zm.a += 0.05f;
            }

            if (!fadeOn && zm.a >= 0 )
            {
                zm.a -= 0.05f;
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

            if (c == '{')
            {
                STitreUi.text = "";
                fadeOn = true;
            }

            if (c == '}')
            {
                fadeOn = false;
                yield return new WaitForSeconds(1f);
            }


            if (c == '0')
            {
                yield return new WaitForSeconds(1f);
            }

            if (c != '0' && c != '\n' && c != '&' && c != '=' && c != '{' && c != '}')
            {
                STitreUi.text += c;
                //yield return new WaitForSeconds(Random.Range(0, 0.1f));
            }



        }
        yield return new WaitForSeconds(1f);
        STitreUi.text = "";
        IsPlaying = false;
        fadeOn = false;
    }

}
