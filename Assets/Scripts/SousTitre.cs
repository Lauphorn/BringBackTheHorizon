using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SousTitre : MonoBehaviour
{

    public AudioClip Doublage;

    public string aide = "(& Left)(= Right)(0 wait 1 sec)(1 delete all)";
    [TextArea(15, 20)]
    public string VoixFr;

    [TextArea(15, 20)]
    public string VoixEng;


    public void Talk()
    {
        if(Settings.Instance.currentLang == Settings.Lang.FR)
        {
            Voice.Instance.LaunchVoice(Doublage, VoixFr);
        }

        if (Settings.Instance.currentLang == Settings.Lang.ENG)
        {
            Voice.Instance.LaunchVoice(Doublage, VoixEng);
        }
    }
}
