using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeLight : MonoBehaviour
{

    public bool OnStart;
    public bool On;
    bool Done;

    public SousTitre Sub;

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

        if (Narration.Instance.CheckNarration(Narration.Narrations.Electricity) == false)
        {
            for (int i = 0; i < Emissive.Count; i++)
            {
                LightGO[i].enabled = false;
                Emissive[i].material.SetColor("_EmissiveColor", Emissive[i].material.GetColor("_EmissiveColor") * 0);
                On = false;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!Done && Narration.Instance.CheckNarration(Narration.Narrations.Electricity) == true)
        {
            if (OnStart)
            {
                SwitchLight();
            }
            Done = true;
        }
    }

    public void SwitchLight()
    {

        if (Narration.Instance.CheckNarration(Narration.Narrations.Electricity) == false && !VoiceDone)
        {
            Sub.Talk();
            VoiceDone = true;
        }


        if (Narration.Instance.CheckNarration(Narration.Narrations.Electricity) == true)
        {
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
                if (Narration.Instance.CheckNarration(Narration.Narrations.Electricity) == true)
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
}
