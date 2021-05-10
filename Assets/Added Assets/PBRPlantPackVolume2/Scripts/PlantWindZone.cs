using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class PlantWindZone : MonoBehaviour
{
    public LayerMask PlantLayer; //Layer on which the plants are located.
    public WindZone GameWindZone; //Optional Setting for using values from a scene windzone.
    public Vector3 WindDirection = new Vector3(0,0,1); //Direction of the wind force to apply.
    public float WindMain = 0.02F; //Constant force applied in direction of WindDirection.
    public float WindTurbulence = 0.01F; //Constant random turbulent windForce applied in direction of WindDirection.
    public float WindPulseMagnitude= 0.01F; //strength of the wind force from the wind cycles back/forth
    public float WindPulseFrequency =1F; //Increase to increase time between wind cycles back/forth.
    private List<Rigidbody> _rigidbodyList; //keeps track of which rigidbodies are in the zone to which to apply the windforce.
    void Start()
    { //Logs some helpful warning to users.
        Collider col = this.transform.GetComponent<Collider>();
        if (col == null)
        {
            Debug.LogWarning("PlantWindZone:" + this.gameObject.name + " does not have a collider attached. Plant Wind Zone will not function without a collider attached.");
        }
        else if (!col.enabled)
        {
            Debug.LogWarning("PlantWindZone:" + this.gameObject.name +
                             "'s collider is disabled, ensure it is enabled to ensure Plant Wind Zone functions properly.");
        }
        if (col != null && !col.isTrigger)
        {
            Debug.LogWarning("PlantWindZone:" + this.gameObject.name + " should be marked as a trigger to properly function.");
        }
        if (PlantLayer.value == 1 || PlantLayer.value == 0 || PlantLayer.value == -1) //1 is default, 0 is nothing, and -1 is everything in layer masks. Logs warning to tell user.
        {
            Debug.LogWarning("PlantWindZone: Plant Layer is currently set to Nothing, Default, or Everything. It is suggested to use a seperate plant layer for plant collisions.");
        }
        if (!this.gameObject.isStatic)
        {
            Debug.LogWarning("PlantWindZone:" + this.gameObject.name + " is not set to static, it will not apply wind properly to rigidbody colliders. It is suggested to mark as a static gameobject.");
        }
        if (WindDirection == Vector3.zero)
        {
            Debug.LogWarning("PlantWindZone:" + this.gameObject.name + " has a wind direction of zero, no wind will be applied to rigidbodies.");
        }
        _rigidbodyList = new List<Rigidbody>();
        if (GameWindZone != null)
        { //if wind zone is specified, uses those values instead.
            WindDirection = GameWindZone.transform.forward;
            WindPulseFrequency = GameWindZone.windPulseFrequency;
            WindPulseMagnitude = GameWindZone.windPulseMagnitude;
            WindMain = GameWindZone.windMain;
            WindTurbulence = GameWindZone.windTurbulence;
        }
    }

    void OnTriggerEnter(Collider col) 
    { //Adds rigidbody to list to which to add forces if the rigidbody is on the specified PlantLayer.
        if (1<<col.gameObject.layer == (PlantLayer.value & 1 << col.gameObject.layer))
        {
            Rigidbody rb = col.GetComponent<Rigidbody>();
            if (rb != null)
            {
                _rigidbodyList.Add(rb);
            }
        }
    }

    void OnTriggerExit(Collider col)
    { //Removes the rigidbody from the list if it exits the wind zone.
        Rigidbody rbody = col.GetComponent<Rigidbody>();
        if (rbody != null)
        {
            
            _rigidbodyList.Remove(rbody);
        }
    }

    void FixedUpdate()
    { //Calculates the force to apply to the rigidbodies in the rigidbody list based on specified values.
        float sinVal = 0;
        if (WindPulseFrequency > 0F)
        {
            sinVal = Mathf.Sin(Time.timeSinceLevelLoad / WindPulseFrequency); //Sin value for wind pulses. Based on time since level load, and the wind pulse frequency.
        }
        foreach (Rigidbody rbody in _rigidbodyList)
        { 
            Vector3 force = WindDirection*WindMain + WindDirection*sinVal*WindPulseMagnitude + WindDirection*Random.Range(0F,WindTurbulence);
            rbody.AddForce(force);
        }
    }
}


