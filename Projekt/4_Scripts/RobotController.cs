using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.XR;
using static UnityEngine.GraphicsBuffer;
using static UnityEditor.Progress;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using static RobotController;
using Unity.Burst.Intrinsics;

public class RobotController : Agent
{
    [System.Serializable]
    public struct Joint
    {
        public string inputAxis;
        public GameObject robotPart;
    }
    public Joint[] joints;

    /*
    public GameObject Hand;
    public GameObject Box;
    public GameObject Target;
    public GameObject Magnet;
*/
    private GameObject Hand;
    private GameObject Box;
    private GameObject Target;
    private GameObject Magnet;

    private GameObject MagnetField;
    private Magnet MagnetScript;
    PincherController pincherController;

    public bool ON = false;
    public bool MagnetOn = true;

    public float updatedDistanceToTarget;
    public float TargetDistance;
    public float StartDistanseToTarget;

    public float updatedDistanceToBox;
    public float BoxDistance;
    public float StartDistanseToBox;

    public bool IfTarget = false;
    public bool IfBox = false;
    public bool IfTable = false;
    public Vector3 speed;

    Vector3 TargetStartposition;
    Vector3 BoxStartposition;

    // initialStates
    private List<(ArticulationBody, Vector3, Quaternion)> initialStates = new List<(ArticulationBody, Vector3, Quaternion)>();
   
    Vector3 StartPosition;
    private ArticulationBody m_Rigidbody;
    Quaternion StartingRotation;

    float continuousHeurisitc = 0;

    public bool Stop = false;

    public override void OnActionReceived(ActionBuffers actions)
   
    {
       // Debug.Log("1");
        AddReward(-0.0001f);

        var discreteActions = actions.DiscreteActions;

        Stop = true;

     if (discreteActions[0] == 1 ){ KeyActions0();  Debug.Log(" test 2");}
      if (discreteActions[0] == 2 ) {KeyActions01();  Debug.Log("test 2"); }

        if (discreteActions[1] == 1)
        {
            
            KeyActions1();
            
        }
        if (discreteActions[1] == 2)
        {
            
           KeyActions11();
            
        }

        if (discreteActions[2] == 1)
        {
            KeyActions2();
        }
        if (discreteActions[2] == 2)
        {
            KeyActions2();
        }

        if (discreteActions[3] == 1)
        {
            KeyActions3();
        }
        if (discreteActions[3] == 2)
        {
            KeyActions3();
        }

        if (discreteActions[4] == 1)
        {
            KeyActions4();
        }
        if (discreteActions[4] == 2)
        {
            KeyActions4();
        }

        if (discreteActions[5] == 1)
        {
            KeyActions5();
        }
        if (discreteActions[5] == 2)
        {
            KeyActions5();
        }

        if (discreteActions[6] == 1)
        {
            KeyActions6();
        }
        if (discreteActions[6] == 2)
        {
            KeyActions6();
        }

        if (discreteActions[7] == 1)
        {
            KeyActions7();
        }

        

    }
    private bool hasWaitedTwoFrames = false;
    private void IfWrongEndEpisodes()
    {
        bool PageDown = Input.GetKeyDown(KeyCode.PageDown);
        bool anyKeyPressed = Input.GetKeyDown(KeyCode.RightArrow) ||
                        Input.GetKeyDown(KeyCode.LeftArrow) ||
                        Input.GetKeyDown(KeyCode.DownArrow) ||
                        Input.GetKeyDown(KeyCode.UpArrow) ||
                        Input.GetKeyDown(KeyCode.Q) ||
                        Input.GetKeyDown(KeyCode.A) ||
                        Input.GetKeyDown(KeyCode.W) ||
                        Input.GetKeyDown(KeyCode.S) ||
                        Input.GetKeyDown(KeyCode.E) ||
                        Input.GetKeyDown(KeyCode.D) ||
                        Input.GetKeyDown(KeyCode.R) ||
                        Input.GetKeyDown(KeyCode.F) ||
                        Input.GetKeyDown(KeyCode.T) ||
                        Input.GetKeyDown(KeyCode.G) ||
                        // Input.GetKeyDown(KeyCode.PageUp) ||
                        Input.GetKeyDown(KeyCode.PageDown);

        if (anyKeyPressed)
        {
            if (TargetDistance > 0) { GoToTarget(); }
            // if (!IfTarget) { GoToTarget(); }

            if (TargetDistance == 0)
            {
                GoToBox();
            }
        }
        if (PageDown && IfBox && IfTarget)
        {
            Debug.Log("1");
            Debug.Log("EndEpisod()");
            AddReward(1f);
            EndEpisode();

        }

        if (PageDown && IfTarget)
        {
            Debug.Log("-1");
            AddReward(-1f);

            EndEpisode();

        }
    }
    public override void Heuristic(in ActionBuffers actionsOut)
    {


     




        // base.Heuristic(actionsOut);
        // Debug.Log("3");
        AddReward(-0.0001f);
        //  Debug.Log("Lyssnar");

        IfWrongEndEpisodes();

        var discreteActions = actionsOut.DiscreteActions;
        var continuousActions = actionsOut.ContinuousActions;

        // Debug.Log("discreteActions" + discreteActions);
        // Debug.Log("discreteActions" + discreteActions);
        discreteActions[0] = 0;
        discreteActions[1] = 0;
        discreteActions[2] = 0;
        discreteActions[3] = 0;
        discreteActions[4] = 0;
        discreteActions[5] = 0;
        discreteActions[6] = 0;
        discreteActions[7] = 0;


        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            discreteActions[0] = 1;
            //Debug.Log("4");
        }
         

        if (Input.GetKeyDown(KeyCode.LeftArrow)) { discreteActions[0] = 2;  }
        if (Input.GetKeyDown(KeyCode.DownArrow)) { discreteActions[1] = 1;  }
           
            
        if (Input.GetKeyDown(KeyCode.UpArrow)) { discreteActions[1] = 2;  }
          
            
        if (Input.GetKeyDown(KeyCode.Q)) { discreteActions[2] = 1; }
           
        if (Input.GetKeyDown(KeyCode.A)) { discreteActions[2] = 2; }
            


        if (Input.GetKeyDown(KeyCode.W)) { discreteActions[3] = 1; }
         
        if (Input.GetKeyDown(KeyCode.S)) { discreteActions[3] = 2; }
            


        if (Input.GetKeyDown(KeyCode.E)) { discreteActions[4] = 1; }
            
        if (Input.GetKeyDown(KeyCode.D)) { discreteActions[4] = 2; }
            


        if (Input.GetKeyDown(KeyCode.R)) { discreteActions[5] = 1; }
            
        if (Input.GetKeyDown(KeyCode.F)) { discreteActions[5] = 2; }
            


        if (Input.GetKeyDown(KeyCode.T)) { discreteActions[6] = 1; }
            
        if (Input.GetKeyDown(KeyCode.G)) { discreteActions[6] = 2; }
            


        if (Input.GetKeyDown(KeyCode.PageDown)) 
        {
            discreteActions[7] = 1;
        }

      StopAllJointRotations();


    }

   
    public override void CollectObservations(VectorSensor sensor)
    {
        // base.CollectObservations(sensor);
        // Debug.Log("Lyssnar");

                 sensor.AddObservation(transform.localPosition); //bilens position (3 x float)
                 sensor.AddObservation(Target.transform.localPosition); //vägpunktens position (3 x float)
                                                                          // sensor.AddObservation(point.transform.localPosition); //vägpunktens position (3 x float)

                 sensor.AddObservation(Box.transform.localPosition);

                 sensor.AddObservation(m_Rigidbody.velocity); //bilens hastighet (3 x float)
                 sensor.AddObservation(Vector3.Distance(transform.localPosition, Target.transform.localPosition)); //avståndet mellan bilen och vägpunkten (1 x float)
                 sensor.AddObservation(Vector3.Distance(transform.localPosition, Box.transform.localPosition)); //avståndet mellan bilen och vägpunkten (1 x float)

                 sensor.AddObservation(transform.rotation); //bilens rotationsvinkel (4 x float)
        
    }

     private void Start()
     {
        

        FindGameObjectsByName();
         InitializeMagnet();
         RandomPosition(Box, Target);
         MagnetOn = true;
        GetInitialStates();



     }
     private void Update()
     {

        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            continuousHeurisitc = 0;
        }
        //speed = m_Rigidbody.velocity;
        ManualControl();
        HandleMagnetLogic();
         CheckCollisions();
     }

     private void FindGameObjectsByName()
     {
         MagnetField = GameObject.Find("Magnet_fild");
         Hand = GameObject.Find("HandE"); // Replace "HandObjectName" with the actual name of your hand object
         Box = GameObject.Find("BOX");   // Replace "BoxObjectName" with the actual name of your box object
         Target = GameObject.Find("Cube"); // Replace "TargetObjectName" with the actual name of your target object
         Magnet = GameObject.Find("Magnet_fild"); // Replace "MagnetObjectName" with the actual name of your magnet object

         if (Hand == null || Box == null || Target == null || Magnet == null || MagnetField == null)
         {
             Debug.LogError("One or more game objects not found!");
         }
     }

     private void InitializeMagnet()
     {
        TargetStartposition = Target.transform.localPosition;
        BoxStartposition = Box.transform.localPosition;
       m_Rigidbody = GetComponent<ArticulationBody>();
        
    //   StartPosition = transform.localPosition;
      //    StartingRotation = transform.localRotation;

         pincherController = Hand.GetComponent<PincherController>();
         MagnetScript = MagnetField.GetComponent<Magnet>();
         MagnetScript.MoveTargetToMagnet(ON);
         StartDistanseToTarget = GetDistanceForTwo(Magnet, Target);
         StartDistanseToBox = GetDistanceForTwo(Magnet, Box);
     }
     public override void OnEpisodeBegin()
     {
         Reset();
     }


     void Reset()
     {
        // transform.localPosition = StartPosition;
        // transform.rotation = StartingRotation;
        Box.transform.localPosition = BoxStartposition;
        Target.transform.localPosition = TargetStartposition;
        m_Rigidbody.velocity = Vector3.zero;

        ResetToInitialState();
        RandomPosition(Box, Target);
    }
     private void HandleMagnetLogic()
     {

         //ManualControl();
         MagnetScript.MoveTargetToMagnet(ON);
         MagnetScript.callON = MagnetOn;

         BoxDistance = GetDistanceForTwo(Magnet, Box);
         TargetDistance = GetDistanceForTwo(Magnet, Target);
     }

     private void CheckCollisions()
     {
         bool PageDown = Input.GetKeyDown(KeyCode.PageDown);
         IfTarget = MagnetScript.IfTarget;
         IfBox = MagnetScript.IfBox;
        IfTable = MagnetScript.IfTable;


        if (IfTable)
        {
            Debug.Log("end episod");
            AddReward(-1);
            EndEpisode();

        }

        if (!IfTarget) { MagnetOn = true; }

         if (IfTarget)
         {
           //  Debug.Log(" go Target: 0.001f");

         }

         if (IfTarget && IfBox)
         {
            // Debug.Log(" go Target: 0.001f");

         }

     }

   
     public void ManualControl()
     {
         bool PageDown = Input.GetKeyDown(KeyCode.PageDown);
         bool anyKeyPressed = Input.GetKeyDown(KeyCode.RightArrow) ||
                         Input.GetKeyDown(KeyCode.LeftArrow) ||
                         Input.GetKeyDown(KeyCode.DownArrow) ||
                         Input.GetKeyDown(KeyCode.UpArrow) ||
                         Input.GetKeyDown(KeyCode.Q) ||
                         Input.GetKeyDown(KeyCode.A) ||
                         Input.GetKeyDown(KeyCode.W) ||
                         Input.GetKeyDown(KeyCode.S) ||
                         Input.GetKeyDown(KeyCode.E) ||
                         Input.GetKeyDown(KeyCode.D) ||
                         Input.GetKeyDown(KeyCode.R) ||
                         Input.GetKeyDown(KeyCode.F) ||
                         Input.GetKeyDown(KeyCode.T) ||
                         Input.GetKeyDown(KeyCode.G) ||
                        // Input.GetKeyDown(KeyCode.PageUp) ||
                         Input.GetKeyDown(KeyCode.PageDown);

         if (anyKeyPressed)
         {
           if (TargetDistance > 0) { GoToTarget(); }
            // if (!IfTarget) { GoToTarget(); }

             if (TargetDistance == 0)
             {
                 GoToBox();
             }  
         }
         if(PageDown && IfBox && IfTarget)
         {
             Debug.Log("1");
            Debug.Log("EndEpisod()");
            AddReward(1f);
            EndEpisode();
            
         }

         if (PageDown && IfTarget )
         {
            Debug.Log("-1");
            AddReward(-1f);
            
            EndEpisode();

        }



        

       if (Input.GetKeyDown(KeyCode.RightArrow))
            RotateJoint(0, RotationDirection.Positive);
        if (Input.GetKeyUp(KeyCode.RightArrow))
           RotateJoint(0, RotationDirection.None);


       if (Input.GetKeyDown(KeyCode.LeftArrow))
            RotateJoint(0, RotationDirection.Negative);
        if (Input.GetKeyUp(KeyCode.LeftArrow))
           RotateJoint(0, RotationDirection.None);

       if (Input.GetKeyDown(KeyCode.DownArrow))
           RotateJoint(1, RotationDirection.Positive);
        if (Input.GetKeyUp(KeyCode.DownArrow))
            RotateJoint(1, RotationDirection.None);


        if (Input.GetKeyDown(KeyCode.UpArrow))
             RotateJoint(1, RotationDirection.Negative);
        if (Input.GetKeyUp(KeyCode.UpArrow))
            RotateJoint(1, RotationDirection.None);

        if (Input.GetKeyDown(KeyCode.Q))
            RotateJoint(2, RotationDirection.Positive);
        if (Input.GetKeyUp(KeyCode.Q))
            RotateJoint(2, RotationDirection.None);

        if (Input.GetKeyDown(KeyCode.A))
            RotateJoint(2, RotationDirection.Negative);
        if (Input.GetKeyUp(KeyCode.A))
            RotateJoint(2, RotationDirection.None);

        if (Input.GetKeyDown(KeyCode.W))
            RotateJoint(3, RotationDirection.Positive);
        if (Input.GetKeyUp(KeyCode.W))
            RotateJoint(3, RotationDirection.None);


        if (Input.GetKeyDown(KeyCode.S))
            RotateJoint(3, RotationDirection.Negative);
        if (Input.GetKeyUp(KeyCode.S))
            RotateJoint(3, RotationDirection.None);

        if (Input.GetKeyDown(KeyCode.E))
            RotateJoint(4, RotationDirection.Positive);
        if (Input.GetKeyUp(KeyCode.E))
            RotateJoint(4, RotationDirection.None);


        if (Input.GetKeyDown(KeyCode.D))
            RotateJoint(4, RotationDirection.Negative);
        if (Input.GetKeyUp(KeyCode.D))
            RotateJoint(4, RotationDirection.None);

        if (Input.GetKeyDown(KeyCode.R))
            RotateJoint(5, RotationDirection.Positive);
        if (Input.GetKeyUp(KeyCode.R))
            RotateJoint(5, RotationDirection.None);


        if (Input.GetKeyDown(KeyCode.F))
            RotateJoint(5, RotationDirection.Negative);
        if (Input.GetKeyUp(KeyCode.F))
            RotateJoint(5, RotationDirection.None);

        if (Input.GetKeyDown(KeyCode.T))
            RotateJoint(6, RotationDirection.Positive);
        if (Input.GetKeyUp(KeyCode.T))
            RotateJoint(6, RotationDirection.None);


        if (Input.GetKeyDown(KeyCode.G))
            RotateJoint(6, RotationDirection.Negative);
        if (Input.GetKeyUp(KeyCode.G))
            RotateJoint(6, RotationDirection.None);

        if (Input.GetKeyDown(KeyCode.PageDown))
            MagnetOn = false;

        // if (Input.GetKeyDown(KeyCode.PageUp))
        //     MagnetOn = true;
    }


    public void GetInitialStates(){ 
        //Store the initial state of all ArticulationBodies in the robot
          initialStates = new List <(ArticulationBody, Vector3, Quaternion)>();
        ArticulationBody[] articulationBodies = GetComponentsInChildren<ArticulationBody>();
        foreach(ArticulationBody body in articulationBodies)
        {
            initialStates.Add((body, body.transform.localPosition, body.transform.localRotation));
        }
    }


        public void ResetToInitialState()
    {
        pincherController.ResetGripToOpen();

        foreach(var state in initialStates) {

            //Reset the position and rotation
            state.Item1.transform.localPosition = state.Item2;
            state.Item1.transform.localRotation = state.Item3;
            // Reset the velocity and angular velocity
            state.Item1.velocity = Vector3.zero;state.Item1.angularVelocity = Vector3.zero;
            // If the ArticulationBody is not the root, reset the xDrive
            if(state.Item1 != initialStates[0].Item1){var drive = state.Item1.xDrive;drive.target = 0f;state.Item1.xDrive = drive;}

        }
    }

    void KeyActions0() {RotateJoint(0, RotationDirection.Positive);}
    void KeyActions01()
    {

        Debug.Log("KeyActions0");
        //  if (Input.GetKeyUp(KeyCode.RightArrow))
        RotateJoint(0, RotationDirection.Negative);
    }
    void KeyActions1() {
        Debug.Log("KeyActions0");
        // if (Input.GetKeyDown(KeyCode.DownArrow))
        RotateJoint(1, RotationDirection.Positive);
       // if (Input.GetKeyUp(KeyCode.DownArrow))
         //   RotateJoint(1, RotationDirection.None);


    
    }

    void KeyActions11()
    {
        Debug.Log("KeyActions0");
        // if (Input.GetKeyDown(KeyCode.UpArrow))
        RotateJoint(1, RotationDirection.Negative);
        //if (Input.GetKeyUp(KeyCode.UpArrow))
          //  RotateJoint(1, RotationDirection.None);
    }
    void KeyActions2() {
        if (Input.GetKeyDown(KeyCode.Q))
            RotateJoint(2, RotationDirection.Positive);
     

        if (Input.GetKeyDown(KeyCode.A))
            RotateJoint(2, RotationDirection.Negative);

    }
    void KeyActions3()
    {
        if (Input.GetKeyDown(KeyCode.W))
            RotateJoint(3, RotationDirection.Positive);
  


        if (Input.GetKeyDown(KeyCode.S))
            RotateJoint(3, RotationDirection.Negative);
;
    }
    void KeyActions4() {
        if (Input.GetKeyDown(KeyCode.E))
            RotateJoint(4, RotationDirection.Positive);
    


        if (Input.GetKeyDown(KeyCode.D))
            RotateJoint(4, RotationDirection.Negative);
    
    }
    void KeyActions5() {
        if (Input.GetKeyDown(KeyCode.R))
            RotateJoint(5, RotationDirection.Positive);



        if (Input.GetKeyDown(KeyCode.F))
            RotateJoint(5, RotationDirection.Negative);
    
    }
    void KeyActions6() {
        if (Input.GetKeyDown(KeyCode.T))
            RotateJoint(6, RotationDirection.Positive);



        if (Input.GetKeyDown(KeyCode.G))
            RotateJoint(6, RotationDirection.Negative);
    
    }
    void KeyActions7() {
        if (Input.GetKeyDown(KeyCode.PageDown))
        {
            MagnetOn = false;
        }
    }



    // CONTROL

    public void StopAllJointRotations()
    {
        for (int i = 0; i < joints.Length; i++)
        {
            GameObject robotPart = joints[i].robotPart;
            UpdateRotationState(RotationDirection.None, robotPart);
        }
    }

    public void RotateJoint(int jointIndex, RotationDirection direction)
    {
        StopAllJointRotations();
        Joint joint = joints[jointIndex];
        UpdateRotationState(direction, joint.robotPart);
    }

    // HELPERS

    static void UpdateRotationState(RotationDirection direction, GameObject robotPart)
    {
        ArticulationJointController jointController = robotPart.GetComponent<ArticulationJointController>();
        jointController.rotationState = direction;
    }

    void RandomPosition(GameObject box, GameObject target)
    {
        Vector3 randomOffsetBox = new Vector3(UnityEngine.Random.Range(0.2f, 0.35f), transform.localPosition.y, UnityEngine.Random.Range(-0.5f, 0.26f));
        Vector3 randomOffsetTarget = new Vector3(UnityEngine.Random.Range(-0.4f, -0.1f), transform.localPosition.y, UnityEngine.Random.Range(-0.5f, 0.27f));

        box.transform.localPosition = randomOffsetBox;
        target.transform.localPosition = randomOffsetTarget;
    }


    float GetDistanceForTwo(GameObject to, GameObject from)
    {
        float dist = Vector3.Distance(from.transform.position, to.transform.position);
        return dist;
    }

    void GoToTarget()
    {
        updatedDistanceToTarget = GetDistanceForTwo(Magnet, Target);
        if (IfTarget)
        {
            Debug.Log("Target with: 0.001f");
            return;
        }

        if (updatedDistanceToTarget < StartDistanseToTarget)
        {
            // AddReward(0.1f); // Lägg till poäng om agenten rör sig närmare målet
            Debug.Log("go target : 0.1f");
            AddReward(0.1f);
            StartDistanseToTarget = updatedDistanceToTarget;
        }

        if (updatedDistanceToTarget > StartDistanseToTarget)
        {
            Debug.Log("- go target : -0.1f");
            AddReward(-0.1f);
            StartDistanseToTarget = updatedDistanceToTarget;
        }
    }
    void GoToBox() 
    {
        updatedDistanceToBox = GetDistanceForTwo(Magnet, Box);

        if (IfBox)
        {
            Debug.Log("Box with");
            return;
        }

        if (updatedDistanceToBox < StartDistanseToBox)
        {
            Debug.Log("go box: 0.1f");
            AddReward(0.1f);
            StartDistanseToBox = updatedDistanceToBox;
        }
        if (updatedDistanceToBox > StartDistanseToBox)
        {
            Debug.Log("- go box: -0.1f");
            AddReward(-0.1f);
            StartDistanseToBox = updatedDistanceToBox;
        }

        
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Table")
        {
            Debug.Log("EndEpisode");
            AddReward(-1.0f);
            EndEpisode();
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Table")
        {
            Debug.Log("EndEpisode");
            AddReward(-1.0f);
            EndEpisode();
        }
    }



}
