using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using Unity.VisualScripting;
using UnityEngine;

public class DriveScript : Agent
{
    // Start is called before the first frame update
    Vector3 StartPosition;
    Vector3 PointStartPosition;
    float speed = 0.2f;
    float turnSpeed = 1.0f;
    public float AddForceSpeed = 20f;
    public float maxSpeed = 20.0f;
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
    GameObject[] point;

 

    //public GameObject Cylinder;
    // GameObject[] AllCylinder; 

    public override void OnActionReceived(ActionBuffers actions)
    {
        // Debug.Log("Lyssnar");
        //getGoalDistance(GoalDistanse);
        AddReward(-0.0025f);
        
        var discreteActions = actions.DiscreteActions;
        if (discreteActions[0] == 1) { DriveForward();//    Score_Closer_To_Goal();
        }
        if (discreteActions[0] == 2) { DriveBackwards();  //    Score_Closer_To_Goal();
        }
        if (discreteActions[1] == 1) {leftDrive();  }
        if (discreteActions[1] == 2){ RightDrive(); }
        
    }
    
    public override void CollectObservations(VectorSensor sensor)
    {

        //Observera att vår kurslärare inte testat att alla dessa observationsvärden verkligen behövs :) Experimentera gärna på egen hand :)

        sensor.AddObservation(transform.localPosition); //bilens position (3 x float)
        sensor.AddObservation(Waypoint.transform.localPosition); //vägpunktens position (3 x float)
        sensor.AddObservation(m_Rigidbody.velocity); //bilens hastighet (3 x float)
        sensor.AddObservation(Vector3.Distance(transform.localPosition, Waypoint.transform.localPosition)); //avståndet mellan bilen och vägpunkten (1 x float)
        sensor.AddObservation(transform.rotation); //bilens rotationsvinkel (4 x float)
        //Som observationsvärden skickar vi totalt 14 flyttalsvärden till neuronnätet. Denna inställning bör göras i Behavour Parameter skriptet för agenten. 
    }


    void Start()
    {
        point = GameObject.FindGameObjectsWithTag("point");

        // GameObject[] allCylinders = GameObject.FindGameObjectsWithTag("Cylinder");
        StartPosition = transform.localPosition;
        StartingRotation = transform.localRotation;
        Debug.Log("Start");
        m_Rigidbody = GetComponent<Rigidbody>();
        startDistance = getGoalDistance(GoalDistanse);
    }

    // Update is called once per frame
    float trainingTimer = 0;
    private void FixedUpdate()
    {
      
        updatedDistance = getGoalDistance(GoalDistanse);

        if (updatedDistance < startDistance)
        {
            AddReward(0.1f); // Lägg till poäng om agenten rör sig närmare målet
            Debug.Log("updatedDistance: +0.1f");
            trainingTimer = 0;
            startDistance = updatedDistance;
        }
        else
        {
            // Annars om den inte blivit bättre så öka countern.
            trainingTimer++;
            
            AddReward(-0.1f);
            Debug.Log(trainingTimer + "AddReward(-0.0001f)");
        }
        if (trainingTimer > 500)
        {
            Debug.Log("EndEpisode");
            trainingTimer = 0;
            EndEpisode();
        }

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
        m_Rigidbody.velocity= Vector3.zero;
    }
    float getGoalDistance(GameObject name)
    {
        float dist = Vector3.Distance(transform.position, name.transform.position);
        return dist;

    }
   
    void Score_Closer_To_Goal()
    {
        updatedDistance = getGoalDistance(GoalDistanse);

        if (updatedDistance < startDistance)
        {
            AddReward(0.1f); // Lägg till poäng om agenten rör sig närmare målet
            //Debug.Log("Distance decreased! Current score: 0.1f");
            startDistance = updatedDistance;
        }
        if (updatedDistance > startDistance)
        {
            AddReward(-0.1f); // Lägg till poäng om agenten rör sig närmare målet
           // Debug.Log("Distance decreased! Current score: -0.1f");
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
        this.transform.Rotate(0, - turnSpeed , 0);
    }
    void RightDrive()
    {
        this.transform.Rotate(0, turnSpeed , 0);
    }
    void DriveForward()
    {
        m_Rigidbody = GetComponent<Rigidbody>();

        // Check the current speed
        float currentSpeed = m_Rigidbody.velocity.magnitude;

        // If the current speed is below the maximum speed limit, add force
        if (currentSpeed < maxSpeed)
        {
            // Calculate the force needed to reach the maximum speed
            Vector3 force = transform.forward * (maxSpeed - currentSpeed);

            // Apply the force to the rigidbody
            m_Rigidbody.AddForce(force, ForceMode.VelocityChange);
        }

    }
    void DriveBackwards()
    {
        float currentSpeed = m_Rigidbody.velocity.magnitude;
        // If the current speed is below the maximum speed limit, add force
        if (currentSpeed < maxSpeed)
        {
            // Calculate the force needed to reach the maximum speed
            Vector3 force = transform.forward * (maxSpeed - currentSpeed);
            // Apply the force to the rigidbody
            m_Rigidbody.AddForce(force*-1, ForceMode.VelocityChange);
        }

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
    void otherRandomPosition(Collider name)
    {
        float x = UnityEngine.Random.Range(-30, 4);
        float z = UnityEngine.Random.Range(70, 150);
        name.transform.localPosition = new Vector3(x, transform.localPosition.y, z);

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
        if (other.gameObject.tag == "point")
        {
            // RandomPosition();
            
            AddReward(0.5f);
            otherRandomPosition(other);
          //  Destroy(other.gameObject);

            Debug.Log("AddReward(5.0f)");
            

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
            m_Rigidbody.velocity.Set(0, 0, 0);
            EndEpisode();
           
     }
 }

}
