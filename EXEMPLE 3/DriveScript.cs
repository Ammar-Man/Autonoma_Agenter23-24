using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using UnityEngine;

public class DriveScript : Agent
{
    // Start is called before the first frame update
    Vector3 StartPosition;
    float speed = 0.2f;
    float turnSpeed = 1.0f;
    //public GameObject Cylinder;
   // GameObject[] AllCylinder; 

    public override void OnActionReceived(ActionBuffers actions)
    {
       // Debug.Log("Lyssnar");
        AddReward(-0.0025f);

        var discreteActions = actions.DiscreteActions;

        if (discreteActions[0] == 1) StraightForward();
        if (discreteActions[1] == 1) leftDrive();
        if (discreteActions[1] == 2) RightDrive();

    }

    void Start()
    {
       // GameObject[] allCylinders = GameObject.FindGameObjectsWithTag("Cylinder");
        StartPosition = transform.localPosition;
        Debug.Log("Start");
      
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        RequestDecision();
    }
    public override void OnEpisodeBegin()
    {
        Reset();
    }
    void Reset()
    {
        transform.localPosition = StartPosition;
        RandomPosition();
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var discreteActions = actionsOut.DiscreteActions;

        discreteActions[0] = 0;
        discreteActions[1] = 0;

        if (Input.GetKey(KeyCode.UpArrow))
            discreteActions[0] = 1;
        if (Input.GetKey(KeyCode.LeftArrow))
            discreteActions[1] = 1;
        if (Input.GetKey(KeyCode.RightArrow))
            discreteActions[1] = 2;
    }

    void leftDrive()
    {
        this.transform.Rotate(0, - turnSpeed, speed);
    }
    void RightDrive()
    {
        this.transform.Rotate(0, turnSpeed, speed);
    }
    void StraightForward()
    {
       
        this.transform.Translate(0, 0, speed);
    }

    void RandomPosition()
    {
        GameObject[] allCylinders = GameObject.FindGameObjectsWithTag("waypoint");
     //   float x = Random.Range(-30, 4);
      //  float z = Random.Range(70, 150);
      //  Cylinder.transform.localPosition = new Vector3(x, transform.localPosition.y, z);
        foreach (GameObject cylinder in allCylinders)

        {
          //  Debug.Log(cylinder);
            float x = Random.Range(-30, 4);
            float z = Random.Range(70, 150);
            cylinder.transform.localPosition = new Vector3(x, transform.localPosition.y, z);
        }
    }
    private void OnCollisionEnter(Collision other)
    {

        if (other.gameObject.tag == "wall")
        {
            Debug.Log("AddReward(-2.0f)");
            // Destroy(this.gameObject);
            AddReward(-2.0f);
            EndEpisode();
        }
        else if (other.gameObject.tag =="waypoint" )
        {
           // RandomPosition();
            
            AddReward(10.0f);
            Debug.Log("AddReward(10.0f)");
            EndEpisode();
        }



    }

     private void nCollisionExit(Collision other)
 {

     if (other.gameObject.tag == "plane")
     {
         EndEpisode();
     }
 }

}
