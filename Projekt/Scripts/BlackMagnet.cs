using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackMagnet : MonoBehaviour
{
 
    GameObject MagnetField;

    private RobotController MagnetScript;

    void Start()
    {
     
        MagnetField = GameObject.Find("UR3");
        MagnetScript = MagnetField.GetComponent<RobotController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision other)
    {
   

        

        if (other.gameObject.tag == ("Table"))
        {
            Debug.Log(" Table cresh");
            MagnetScript.AddReward(-1);
            MagnetScript.EndEpisode();



        }


    }
}
