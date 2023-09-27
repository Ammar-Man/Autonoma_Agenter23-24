using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class DriveScript : Agent
{
    // Start is called before the first frame update
    Vector3 StartPosition;
    float speed = 0.2f;
    float turnSpeed = 1.0f;
    public float AddForceSpeed = 20f;
    Rigidbody m_Rigidbody;
    public GameObject GoalDistanse;
    public GameObject StartDistance;
    public GameObject tresure;
    public Vector3 carPosition;
   public float carSpeed;
   public float updatedDistance;
    public float startDistance = 0;
    public GameObject Waypoint;
    Quaternion StartingRotation;

    //public GameObject Cylinder;
    // GameObject[] AllCylinder; 

    public override void OnActionReceived(ActionBuffers actions)
    {
        // Debug.Log("Lyssnar");
        //getGoalDistance(GoalDistanse);
        AddReward(-0.0025f);
        var discreteActions = actions.DiscreteActions;
        if (discreteActions[0] == 1) { DriveForward(); Score_Closer_To_Goal(); }
        if (discreteActions[0] == 2) { DriveBackwards(); Score_Closer_To_Goal(); } 
        if (discreteActions[1] == 1) {leftDrive(); Score_Closer_To_Goal(); }
        if (discreteActions[1] == 2){ RightDrive(); Score_Closer_To_Goal(); }

    }
    
    public override void CollectObservations(VectorSensor sensor)
    {

        //Observera att v�r kursl�rare inte testat att alla dessa observationsv�rden verkligen beh�vs :) Experimentera g�rna p� egen hand :)

        sensor.AddObservation(transform.localPosition); //bilens position (3 x float)
        sensor.AddObservation(Waypoint.transform.localPosition); //v�gpunktens position (3 x float)
        sensor.AddObservation(m_Rigidbody.velocity); //bilens hastighet (3 x float)
        sensor.AddObservation(Vector3.Distance(transform.localPosition, Waypoint.transform.localPosition)); //avst�ndet mellan bilen och v�gpunkten (1 x float)
        sensor.AddObservation(transform.rotation); //bilens rotationsvinkel (4 x float)
        //Som observationsv�rden skickar vi totalt 14 flyttalsv�rden till neuronn�tet. Denna inst�llning b�r g�ras i Behavour Parameter skriptet f�r agenten. 
    }


    void Start()
    {
       // GameObject[] allCylinders = GameObject.FindGameObjectsWithTag("Cylinder");
        StartPosition = transform.localPosition;
          StartingRotation = transform.localRotation;
        Debug.Log("Start");
        m_Rigidbody = GetComponent<Rigidbody>();

         
        startDistance = getGoalDistance(GoalDistanse);
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        var vel = m_Rigidbody.velocity;    

       

        carPosition = this.transform.position;
        carSpeed = vel.magnitude;
        RequestDecision();
    }
    public override void OnEpisodeBegin()
    {
        Reset();
    }
    void Reset()
    {
        transform.localPosition = StartPosition;
        transform.rotation =StartingRotation;
        m_Rigidbody.velocity.Set(0, 0, 0);
        

    }
    float getGoalDistance(GameObject name)
    {
        float dist = Vector3.Distance(transform.position, name.transform.position);
        // print(f"Distance to other: {name} " + dist);
        // Debug.Log($"Distance to {name}: {dist}");
        return dist;

    }
   
    void Score_Closer_To_Goal()
    {

        //Debug.Log(dist);
        // start 100 
        // m�l = 99 
        // if (start < m�l){ udate start }
        updatedDistance = getGoalDistance(GoalDistanse);

        if (updatedDistance < startDistance)
        {
            AddReward(0.1f); // L�gg till po�ng om agenten r�r sig n�rmare m�let
            Debug.Log("Distance decreased! Current score: 0.1f");
            startDistance = updatedDistance;
        }
        if (updatedDistance > startDistance)
        {
            AddReward(-0.1f); // L�gg till po�ng om agenten r�r sig n�rmare m�let
            Debug.Log("Distance decreased! Current score: -0.1f");
            startDistance = updatedDistance;
        }

    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        AddReward(-0.0025f);

        var discreteActions = actionsOut.DiscreteActions;

        discreteActions[0] = 0;
        discreteActions[1] = 0;

        if (Input.GetKey(KeyCode.UpArrow))
            discreteActions[0] = 1;
        if (Input.GetKey(KeyCode.DownArrow))
            discreteActions[0] = 2;
        if (Input.GetKey(KeyCode.LeftArrow))
            discreteActions[1] = 1;
        if (Input.GetKey(KeyCode.RightArrow))
            discreteActions[1] = 2;
    }

    void leftDrive()
    {
        this.transform.Rotate(0, - turnSpeed, 0);
    }
    void RightDrive()
    {
        this.transform.Rotate(0, turnSpeed, 0);
    }
    void DriveForward()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        m_Rigidbody.AddForce(transform.forward * AddForceSpeed);
        //this.transform.Translate(0, 0, speed);

    }
    void DriveBackwards()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        m_Rigidbody.AddForce(transform.forward * AddForceSpeed *-1);
        //this.transform.Translate(0, 0, speed);

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
            float x = UnityEngine.Random.Range(-30, 4);
            float z = UnityEngine.Random.Range(70, 150);
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
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "wall")
        {
            Debug.Log("AddReward(-2.0f)");
            // Destroy(this.gameObject);
            AddReward(-2.0f);
            EndEpisode();
        }
        else if (other.gameObject.tag == "waypoint")
        {
            // RandomPosition();

            AddReward(10.0f);
            Debug.Log("AddReward(10.0f)");
            EndEpisode();
        }

    }

    private void OnCollisionExit(Collision other)
 {

     if (other.gameObject.tag == "plane")
     {
            Debug.Log("out of plane");
            EndEpisode();
     }
 }

}
