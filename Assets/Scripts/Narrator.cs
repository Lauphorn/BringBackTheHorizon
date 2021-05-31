using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Narrator : MonoBehaviour
{
    public Narration.Narrations EnableIfNarration;
    Narration Narr;

    bool check;
    public bool HideIfNarration;

    void Start()
    {
        Narr = Narration.Instance;
        check = true;
    }

    void Update()
    {
        if (Narr.NarrationHasChanged != check)
        {
            check = Narr.NarrationHasChanged;

            if (Narr.CheckNarration(EnableIfNarration))
            {
                gameObject.SetActive(!HideIfNarration);
            }
            else
            {
                gameObject.SetActive(HideIfNarration);
            }
        }

    }
}
