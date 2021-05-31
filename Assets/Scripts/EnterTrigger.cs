using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using MyBox;


public class EnterTrigger : MonoBehaviour
{
    public UnityEvent Function;

    public bool TimedFunc;
    [ConditionalField("TimedFunc")] public UnityEvent TimedFunction;
    [ConditionalField("TimedFunc")] public float TimeWait;
    bool start;
    bool timeddone;

    public bool destroy_;
    bool done;

    public bool NeedNarration;
    [ConditionalField("NeedNarration")] public Narration.Narrations ShowWithNarration;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (start)
        {
            TimeWait -= Time.deltaTime;
        }
        if (TimeWait <= 0 && !timeddone)
        {
            TimedFunction.Invoke();
            timeddone = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!done)
        {
            if (other.CompareTag("Player"))
            {
                if (NeedNarration)
                {
                    if (Narration.Instance.CheckNarration(ShowWithNarration))
                    {
                        Function.Invoke();


                        if (TimedFunc)
                        {
                            start = true;
                        }
                        if (destroy_)
                        {
                            done = true;
                        }
                    }
                }
                else
                {
                    Function.Invoke();


                    if (TimedFunc)
                    {
                        start = true;
                    }
                    if (destroy_)
                    {
                        done = true;
                    }
                }
            }
        }

    }
}
