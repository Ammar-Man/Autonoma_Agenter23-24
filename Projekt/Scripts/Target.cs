using Grpc.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
  
      
    public float distanse;
    public GameObject box;
    public GameObject floor;
    GameObject MagnetField;
    private RobotController MagnetScript;
    // Start is called before the first frame update
    void Start()
    {

        
        MagnetField = GameObject.Find("UR3");
        MagnetScript = MagnetField.GetComponent<RobotController>();
        
    }

    // Update is called once per frame
    void Update()
    {
        distanse = Vector3.Distance(this.transform.position, box.transform.position);

        if (distanse <= 0.2)
        {
            Debug.Log("gool from Box");
            MagnetScript.AddReward(10.0f);
            MagnetScript.EndEpisode();
        }
    }

 

    private void OnTriggerEnter(Collider other)
    {
       
   
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag==("floor")){

            Debug.Log("target on floor");
            
            MagnetScript.EndEpisode();
        }
    }
}
