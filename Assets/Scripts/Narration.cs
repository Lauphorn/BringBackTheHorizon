using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Narration : MonoBehaviour
{
    private static Narration _instance;
    public static Narration Instance { get { return _instance; } }
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

        foreach (Narrations state in System.Enum.GetValues(typeof(Narrations)))
        {
            NarrBool.Add(false);
        }
    }

    public enum Narrations { Fusible,FusibleOn, Electricity,KeyBibliotheque, KeyBureau };
    public Narrations currentState;

    public List<bool> NarrBool = new List<bool>();

    public bool NarrationHasChanged;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


    public int GetNarrationNumber(Narrations narr)
    {
        int i = 0;
        foreach (Narrations state in System.Enum.GetValues(typeof(Narrations)))
        {
            i++;
            if (state == narr)
                break;
        }
        return i - 1;
    }

    public bool CheckNarration(Narrations narr)
    {


        if (NarrBool[GetNarrationNumber(narr)] == true)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void ChangeNarration(Narrations narr, bool state)
    {
        NarrBool[GetNarrationNumber(narr)] = state;
        NarrationHasChanged = !NarrationHasChanged;
    } 
}
