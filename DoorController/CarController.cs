using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CarController : MonoBehaviour
{
    float speed = 0.3f;

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.Translate(0, 0, -speed);                 
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name == "VanishPoint") 
        {
            Debug.Log("VanishPoint");
            Destroy(this.gameObject);
        }
           



        if(other.gameObject.name == "MotionSensor")
        {
            Debug.Log("MotionSensor");
            GateController GC = GameObject.Find(transform.parent.name+"/Gate").GetComponent<GateController>();
            GC.PassedGate();
        }

    }
}

