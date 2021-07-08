using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Vision : MonoBehaviour
{

    public Volume Vol;

    public float ValueGoal;
    public float speed = 0.005f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        speed = 0.001f;
        if (Input.GetKeyDown(KeyCode.N))
        {
            //ChangeVision(ValueGoal);
        }
    }

    public void ChangeVision(float Value)
    {
        ValueGoal = Value;
        StartCoroutine(ChangeWeight());
    }

    public void ChangeSpeed(float TimeToGo)
    {
        speed = TimeToGo;
    }

    public IEnumerator ChangeWeight()
    {
        while (Vol.weight != ValueGoal)
        {
            Vol.weight = Mathf.MoveTowards(Vol.weight, ValueGoal, speed);
            yield return null;
        }
    }
}
