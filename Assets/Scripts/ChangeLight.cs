using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeLight : MonoBehaviour
{

    public bool On;

    public AudioClip ElecCoupée;
    public string Voix;

    public List <Renderer> Emissive = new List<Renderer>();
    List <Color> emissiveColor = new List<Color>();
    List<Light> LightGO = new List<Light>();

    bool VoiceDone;

    const string kEmissiveColor = "_EmissiveColor";

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < Emissive.Count; i++)
        {
            emissiveColor.Add(Emissive[i].material.GetColor(kEmissiveColor));
            LightGO.Add(Emissive[i].GetComponentInChildren<Light>());
        }

        if (Narration.Instance.Objects["Electricity"] == false)
        {
            On = false;
        }

        if (On)
        {
            for (int i = 0; i < Emissive.Count; i++)
            {
                LightGO[i].enabled = true;
                Emissive[i].material.SetColor(kEmissiveColor, emissiveColor[i]);
            }
        }
        else
        {
            for (int i = 0; i < Emissive.Count; i++)
            {
                LightGO[i].enabled = false;
                Emissive[i].material.SetColor("_EmissiveColor", Emissive[i].material.GetColor("_EmissiveColor") * 0);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SwitchLight()
    {

        if (Narration.Instance.Objects["Electricity"] == false && !VoiceDone)
        {
            Voice.Instance.LaunchVoice(ElecCoupée, Voix);
            VoiceDone = true;
        }

        if (On)
        {
            for (int i = 0; i < Emissive.Count; i++)
            {
                LightGO[i].enabled = false;
                Emissive[i].material.SetColor("_EmissiveColor", Emissive[i].material.GetColor("_EmissiveColor") * 0);
            }
            On = false;
        }
        else
        {
            if (Narration.Instance.Objects["Electricity"] == true)
            {
                for (int i = 0; i < Emissive.Count; i++)
                {
                    LightGO[i].enabled = true;
                    Emissive[i].material.SetColor(kEmissiveColor, emissiveColor[i]);
                }
            }
            On = true;
        }
    }
}
