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

    bool IsPlaying;


    public bool Delayed;
    public AudioClip AClipDelayed;
    public string STitreDelayed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsPlaying && Delayed)
        {
            LaunchVoice(AClipDelayed, STitreDelayed);
            Delayed = false;
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
            if(c == '1')
            {
                yield return new WaitForSeconds(0.1f);
                STitreUi.text = "";
            }
            else
            {
                STitreUi.text += c;
                yield return new WaitForSeconds(0.05f);
            }
        }
        yield return new WaitForSeconds(1f);
        STitreUi.text = "";
        IsPlaying = false;
    }
}
