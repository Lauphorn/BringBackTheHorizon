using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableByRoom : MonoBehaviour
{

    public List <InteractableObject> Interactifs = new List<InteractableObject>();

    private void Awake()
    {
        gameObject.GetComponent<Collider>().enabled = false;
    }
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(FinishFirst());
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.tag == "Item")
        {
            Interactifs.Add(other.GetComponent<InteractableObject>());
        }

        if (other.tag == "Player")
        {
            for (int i = 0; i < Interactifs.Count; i++)
            {
                Interactifs[i].InRoom = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            for (int i = 0; i < Interactifs.Count; i++)
            {
                Interactifs[i].InRoom = false;
            }
        }
    }

    IEnumerator FinishFirst()
    {
        yield return new WaitForSeconds(1);
        gameObject.GetComponent<Collider>().enabled = true;
    }
}
