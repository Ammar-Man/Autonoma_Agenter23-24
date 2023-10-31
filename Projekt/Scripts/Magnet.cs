using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnet : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject Target;
    public GameObject Table;
    public GameObject Box;
    public GameObject MagnetObj;
    GameObject MagnetField;

    public bool ON = true;
    public bool callON = false;
    public bool IfTarget = false;
    public bool IfBox = false;
    public bool IfTable = false;

    public float ToTable;

    public float ToTarget;

    public float ToBox;

    private RobotController MagnetScript;

    void Start()
    {
        ON = true;
        MagnetField = GameObject.Find("UR3");
        MagnetScript = MagnetField.GetComponent<RobotController>();
    }

    // Update is called once per frame
    void Update()
    {
        ToTable = GetDistanceForTwo(Table, MagnetObj);
        ToBox = GetDistanceForTwo(Box, MagnetObj);
        ToTarget = GetDistanceForTwo(Target, MagnetObj);

       
    }

    float GetDistanceForTwo(GameObject to, GameObject from)
    {
        float dist = Vector3.Distance(from.transform.position, to.transform.position);
        return dist;
    }
    public void MoveTargetToMagnet(bool ON)
    {
        if (ON == true)
        {
            Target.GetComponent<Rigidbody>().isKinematic = true;
            Target.GetComponent<Rigidbody>().useGravity = false;
            Target.GetComponent<Collider>().isTrigger = true;
            Target.transform.SetParent(this.transform);
            Target.transform.localRotation = Quaternion.identity;
            Target.transform.localPosition = new Vector3(0, 0, 0);
        }

        else if(ON == false)
         {
            Target.GetComponent<Rigidbody>().useGravity = true;
            Target.GetComponent<Collider>().isTrigger = false;
            Target.GetComponent<Rigidbody>().isKinematic = false;
            Target.transform.SetParent(null);
           // Debug.Log("Target on plan");

        }
    }


  
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == ("Target") ){

            IfTarget = true;
            if (callON)
            {
                MoveTargetToMagnet(true);
            }
        }
        if(other.gameObject.tag == ("BOX")){

         if(GetDistanceForTwo(Box, MagnetObj) < 1)
            {
                IfBox = true;
            }
          
        }
/*
        if (other.gameObject.tag == ("Table"))
        {
            Debug.Log(" Table cresh");
            MagnetScript.AddReward(-1);
            MagnetScript.EndEpisode();
              
           
                
        }

        if (other.gameObject.tag == ("overtable"))
        {
            IfTable = true;
          
        }
*/
      
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == ("Target"))
        {
            IfTarget = false;
        }
        if (other.gameObject.tag == ("BOX"))
        {        
                IfBox = false;
        }

        if (other.gameObject.tag == ("overtable"))
        {
                IfTable = false;
        }

       


    }




    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == ("Target"))
        {

            IfTarget = true;
            if (callON)
            {
                MoveTargetToMagnet(true);
            }
        }
        if (other.gameObject.tag == ("BOX"))
        {

            IfBox = true;
        }

        if (other.gameObject.tag == ("Table"))
        {

            IfTable = true;
        }
         
        
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.tag == ("Target"))
        {
            IfTarget = false;
        }
        if (other.gameObject.tag == ("BOX"))
        {
            IfBox = false;
        }

        if (other.gameObject.tag == ("overtable"))
        {
            IfTable = false;
        }
    }



}
