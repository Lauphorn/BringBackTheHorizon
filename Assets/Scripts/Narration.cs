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

        Objects.Add("Fusible", false);
        Objects.Add("Electricity", false);
    }

    public Dictionary<string, bool> Objects = new Dictionary<string, bool>();


    
    // Start is called before the first frame update
    void Start()
    {


    }

    // Update is called once per frame
    void Update()
    {

    }
}
