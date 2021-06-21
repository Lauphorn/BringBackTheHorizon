using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Vision : MonoBehaviour
{

    public Volume Vol;

    float ValueGoal;
    float speed;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

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
        float startingValue = Vol.weight;

        for (float t = 0; t < 1.0f; t += Time.deltaTime / speed)
        {
            Vol.weight = Mathf.Lerp(startingValue, ValueGoal, t);
            yield return null;
        }
    }

}
