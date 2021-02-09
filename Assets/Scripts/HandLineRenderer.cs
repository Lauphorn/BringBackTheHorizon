using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class HandLineRenderer : MonoBehaviour
{

    public GameObject Pouce;
    public GameObject Index;
    public GameObject Majeur;
    public GameObject Annulaire;
    public GameObject Auriculaire;

    public bool ShowLine;

    float Slow;

    void Start()
    {

    }

    void Update()
    {
        /*
        if (ShowLine && Slow>=0)
        {
            Slow -= Time.deltaTime;

            Pouce.GetComponent.positionCount = 5;
            Pouce.SetPosition(0, Pouce.gameObject.transform.parent.position);
            Pouce.SetPosition(1, Pouce.gameObject.transform.position);
            Pouce.SetPosition(2, Pouce.gameObject.transform.GetChild(0).position);
            Pouce.SetPosition(3, Pouce.gameObject.transform.GetChild(0).GetChild(0).position);
            Pouce.SetPosition(4, Pouce.gameObject.transform.GetChild(0).GetChild(0).GetChild(0).position);

            Index.positionCount = 5;
            Index.SetPosition(0, Index.gameObject.transform.parent.position);
            Index.SetPosition(1, Index.gameObject.transform.position);
            Index.SetPosition(2, Index.gameObject.transform.GetChild(0).position);
            Index.SetPosition(3, Index.gameObject.transform.GetChild(0).GetChild(0).position);
            Index.SetPosition(4, Index.gameObject.transform.GetChild(0).GetChild(0).GetChild(0).position);

            Majeur.positionCount = 5;
            Majeur.SetPosition(0, Majeur.gameObject.transform.parent.position);
            Majeur.SetPosition(1, Majeur.gameObject.transform.position);
            Majeur.SetPosition(2, Majeur.gameObject.transform.GetChild(0).position);
            Majeur.SetPosition(3, Majeur.gameObject.transform.GetChild(0).GetChild(0).position);
            Majeur.SetPosition(4, Majeur.gameObject.transform.GetChild(0).GetChild(0).GetChild(0).position);

            Annulaire.positionCount = 5;
            Annulaire.SetPosition(0, Annulaire.gameObject.transform.parent.position);
            Annulaire.SetPosition(1, Annulaire.gameObject.transform.position);
            Annulaire.SetPosition(2, Annulaire.gameObject.transform.GetChild(0).position);
            Annulaire.SetPosition(3, Annulaire.gameObject.transform.GetChild(0).GetChild(0).position);
            Annulaire.SetPosition(4, Annulaire.gameObject.transform.GetChild(0).GetChild(0).GetChild(0).position);

            Auriculaire.positionCount = 5;
            Auriculaire.SetPosition(0, Auriculaire.gameObject.transform.parent.position);
            Auriculaire.SetPosition(1, Auriculaire.gameObject.transform.position);
            Auriculaire.SetPosition(2, Auriculaire.gameObject.transform.GetChild(0).position);
            Auriculaire.SetPosition(3, Auriculaire.gameObject.transform.GetChild(0).GetChild(0).position);
            Auriculaire.SetPosition(4, Auriculaire.gameObject.transform.GetChild(0).GetChild(0).GetChild(0).position);

            if (Pouce.enabled != true)
            {
                Pouce.enabled = true;
                Index.enabled = true;
                Majeur.enabled = true;
                Annulaire.enabled = true;
                Auriculaire.enabled = true;
            }
        }

        if (Slow <= 0)
        {
            Slow = 0.1f;
        }*/

    }

}
