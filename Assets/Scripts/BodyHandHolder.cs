using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyHandHolder : MonoBehaviour
{
    public GameObject RightHand;
    public GameObject LeftHand;

    public GameObject DisabledModel;

    void Start()
    {
        DisabledModel.SetActive(false);
    }
}
