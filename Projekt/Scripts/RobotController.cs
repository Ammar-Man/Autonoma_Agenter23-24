using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.XR;
using static UnityEngine.GraphicsBuffer;
//using static UnityEditor.Progress;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using static RobotController;
using Unity.Burst.Intrinsics;
using UnityEditor;

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
    public GameObject Magnet_fild;
    public float Speed  = 1;
    private GameObject Hand;
    private GameObject Box;
    private GameObject Target;
    private GameObject Magnet;
    private GameObject Floor;
    public GameObject OverTable;

    private GameObject MagnetField;
   
    private Magnet MagnetScript;
    private Target CubeScript;
    PincherController pincherController;

    public bool ON = false;
    public bool MagnetOn = false;

    public float StartDistanseToTarget;
    public float updatedDistanceToTarget;
    public float trainingTimer = 0;
    public bool nothing = false;
    public float updatedDistanceToBox;
    public float StartDistanseToBox;

    public bool IfTarget = false;
    public bool IfBox = false;
   

    public bool speed = false;

    public float plusAR;
    public float minusAR;
    public float timmer;

    

    Vector3 TargetStartposition;
    Vector3 TargetStartScale;
    Vector3 TargetScale;
    Vector3 BoxStartposition;

    // initialStates
    private List<(ArticulationBody, Vector3, Quaternion)> initialStates = new List<(ArticulationBody, Vector3, Quaternion)>();
   
    Vector3 StartPosition;
    private ArticulationBody m_Rigidbody;
    private Rigidbody rigidbody;

    Quaternion StartingRotation;
    Quaternion localRotation0;
    Quaternion localRotation1;
    Quaternion localRotation2;
    Quaternion localRotation3;
    Quaternion localRotation4;
    Quaternion localRotation5;
    Quaternion localRotation6;
    // Quaternion localRotation7;
   
    public bool inputAi = false;
    public float GetRotationZ;
    public float GetRotationZ_2;

    public override void OnActionReceived(ActionBuffers actions)
   
    {
       // Debug.Log("1");
        AddReward(-0.0001f);

        var discreteActions = actions.DiscreteActions;

      

        if (discreteActions[0] == 1 ){KeyActions0(true); MagnetOn = true; inputAi = true; }
        if (discreteActions[0] == 2 ) {KeyActions0(false); MagnetOn = true; inputAi = true; }
        


      if (discreteActions[1] == 1){ KeyActions1(true); MagnetOn = true; inputAi = true; }
      if (discreteActions[1] == 2){KeyActions1(false); MagnetOn = true; inputAi = true; }



      if (discreteActions[2] == 1){KeyActions2(); MagnetOn = true; inputAi = true; }
      if (discreteActions[2] == 2){ KeyActions2N(); MagnetOn = true; inputAi = true; }
      

      if (discreteActions[3] == 1){KeyActions3(); MagnetOn = true; inputAi = true; }
      if (discreteActions[3] == 2){KeyActions3N(); MagnetOn = true; inputAi = true; }


      if (discreteActions[4] == 1){
            KeyActions7();
            inputAi = true;

        }

        else { inputAi = false; }
      
     
    }

    public override void Heuristic(in ActionBuffers actionsOut)
     {
      
         AddReward(-0.0001f);

         var discreteActions = actionsOut.DiscreteActions;
         var continuousActions = actionsOut.ContinuousActions;

         discreteActions[0] = 0;
         discreteActions[1] = 0;
         discreteActions[2] = 0;
         discreteActions[3] = 0;
        /* discreteActions[4] = 0;
         discreteActions[5] = 0;
         discreteActions[6] = 0;
         discreteActions[7] = 0;
        */
        

         if (Input.GetKeyDown(KeyCode.RightArrow)){ discreteActions[0] = 1;}
         if (Input.GetKeyDown(KeyCode.LeftArrow)) { discreteActions[0] = 2;}

         if (Input.GetKeyDown(KeyCode.DownArrow)) { discreteActions[1] = 1; } 
         if (Input.GetKeyDown(KeyCode.UpArrow)) { discreteActions[1] = 2;  }

         if (Input.GetKeyDown(KeyCode.Q)) { discreteActions[2] = 1; }
         if (Input.GetKeyDown(KeyCode.A)) { discreteActions[2] = 2; }


         

         if (Input.GetKeyDown(KeyCode.W)) { discreteActions[3] = 1; }
         if (Input.GetKeyDown(KeyCode.S)) { discreteActions[3] = 2; }
/*

         if (Input.GetKeyDown(KeyCode.E)) { discreteActions[4] = 1; }
         if (Input.GetKeyDown(KeyCode.D)) { discreteActions[4] = 2; }


         if (Input.GetKeyDown(KeyCode.R)) { discreteActions[5] = 1; }
         if (Input.GetKeyDown(KeyCode.F)) { discreteActions[5] = 2; }

         
         
         if (Input.GetKeyDown(KeyCode.T)) { discreteActions[6] = 1; }
         if (Input.GetKeyDown(KeyCode.G)) { discreteActions[6] = 2; }
         */
         if (Input.GetKeyDown(KeyCode.PageDown)) {discreteActions[3] = 1;}
     }

   



    public override void CollectObservations(VectorSensor sensor)
    {
        // base.CollectObservations(sensor);
        // Debug.Log("Lyssnar");

       sensor.AddObservation(transform.localPosition); //bilens position (3 x float)
       sensor.AddObservation(Target.transform.localPosition); //vägpunktens position (3 x float)
       sensor.AddObservation(OverTable.transform.localPosition);                                                      // sensor.AddObservation(point.transform.localPosition); //vägpunktens position (3 x float)
       sensor.AddObservation(Box.transform.localPosition);
       //    sensor.AddObservation(m_Rigidbody.velocity); //bilens hastighet (3 x float)
       sensor.AddObservation(Vector3.Distance(transform.localPosition, Target.transform.localPosition)); //avståndet mellan bilen och vägpunkten (1 x float)
       sensor.AddObservation(Vector3.Distance(transform.localPosition, Box.transform.localPosition)); //avståndet mellan bilen och vägpunkten (1 x float)
       sensor.AddObservation(transform.rotation); //bilens rotationsvinkel (4 x float)
        
    }

     private void Start()
     {
        FindGameObjectsByName();
        InitializeMagnet();
        

        StartDistanseToTarget = GetDistanceForTwo(Magnet, Target);
        StartDistanseToBox = GetDistanceForTwo(Magnet, Box);
        RandomPosition(Box, Target);
        MagnetOn = false;
     }
    
    private void Update()
     {
        
        HandleMagnetLogic();
        CheckCollisions();
        Target.transform.localScale = new Vector3(0.15f, 0.15f, 0.15f);
        Magnet_fild.transform.localScale = new Vector3(4f, 4f, 4f);
        ManualControl();

    }

     private void FindGameObjectsByName()
     {
         MagnetField = GameObject.Find("Magnet_fild");
        Floor = GameObject.Find("Floor"); 
         Hand = GameObject.Find("HandE"); // Replace "HandObjectName" with the actual name of your hand object
         Box = GameObject.Find("BOX");   // Replace "BoxObjectName" with the actual name of your box object
         Target = GameObject.Find("Cube"); // Replace "TargetObjectName" with the actual name of your target object
         Magnet = GameObject.Find("Magnet_fild"); // Replace "MagnetObjectName" with the actual name of your magnet object

         if (Hand == null || Box == null || Target == null || Magnet == null || MagnetField == null || Floor == null)
         {
             Debug.LogError("One or more game objects not found!");
         }
     }

     private void InitializeMagnet()
     {
        // starting from start
        //GetInitialStates save all robots arm roation befor starting
        //Quaternion localRotation = joints[3].robotPart.transform.localRotation;
         localRotation0 = joints[0].robotPart.transform.localRotation;
         localRotation1 = joints[1].robotPart.transform.localRotation;
        localRotation2 = joints[2].robotPart.transform.localRotation;
         localRotation3 = joints[3].robotPart.transform.localRotation;
        localRotation4 = joints[4].robotPart.transform.localRotation;
        localRotation5 = joints[5].robotPart.transform.localRotation;
         localRotation6 = joints[6].robotPart.transform.localRotation;
      //  localRotation7 = joints[7].robotPart.transform.localRotation;


        TargetStartposition = Target.transform.localPosition;
        TargetStartScale = Target.transform.localScale;
        BoxStartposition = Box.transform.localPosition;

       m_Rigidbody = GetComponent<ArticulationBody>();
         rigidbody = Target.GetComponent<Rigidbody>();
        pincherController = Hand.GetComponent<PincherController>();
         MagnetScript = MagnetField.GetComponent<Magnet>();
        CubeScript = Target.GetComponent<Target>();
         MagnetScript.MoveTargetToMagnet(ON);
         StartDistanseToTarget = GetDistanceForTwo(Magnet, Target);
         StartDistanseToBox = GetDistanceForTwo(Magnet, Box);
     }
     public override void OnEpisodeBegin()
     {
        MagnetScript.IfBox = false;
       
        MagnetScript.IfTarget = false;
        Reset();
     }


     void Reset()
     {
        // RotateJoint(0, RotationDirection.None);
        //RotateJoint(2, RotationDirection.None);
        MagnetOn = false;
        MagnetScript.MoveTargetToMagnet(false);

        Box.transform.localPosition = BoxStartposition;
        Target.transform.localPosition = TargetStartposition;
        Target.transform.localScale = TargetStartScale;
        rigidbody.velocity = Vector3.zero;


         joints[0].robotPart.transform.localRotation = localRotation0 ;
        joints[1].robotPart.transform.localRotation = localRotation1;
        joints[2].robotPart.transform.localRotation = localRotation2;
        joints[3].robotPart.transform.localRotation = localRotation3;
        joints[4].robotPart.transform.localRotation = localRotation4;
        joints[5].robotPart.transform.localRotation = localRotation5;
        joints[6].robotPart.transform.localRotation = localRotation6;
       // joints[7].robotPart.transform.localRotation = localRotation7;



        RandomPosition(Box, Target);
       
        timmer = 0;
        plusAR = 0;
        minusAR = 0;
        trainingTimer = 0;
    }
     private void HandleMagnetLogic()
     {

        // runs form update 
   //ManualControl();
        MagnetScript.MoveTargetToMagnet(ON);
         MagnetScript.callON = MagnetOn;

       
     
     }

     private void CheckCollisions()
     {

        // this function is in update()
        GetRotationZ = joints[1].robotPart.transform.rotation.eulerAngles.z;
        GetRotationZ_2 = joints[2].robotPart.transform.rotation.eulerAngles.z;

        bool PageDown = Input.GetKeyDown(KeyCode.PageDown);
         IfTarget = MagnetScript.IfTarget;
         IfBox = MagnetScript.IfBox;
   
       
                if (PageDown && IfTarget && IfBox)
                {
                    Debug.Log("goal ,end episod");
                    AddReward(5);
                    EndEpisode();
                }


                // page down om han har target med
                if (PageDown && IfTarget && (IfBox == false))
                {
                    Debug.Log("Target drop ,end episod");
                    AddReward(-0.2f);

                }

   


      

                

              
      GoToTarget();
        GoToBox();



    }

   
    public void ManualControl()
     {
   
       //Debug.Log("ManualControl ");

        if (Input.GetKeyDown(KeyCode.RightArrow))

            //joints[0].robotPart.transform.Rotate(0, Speed, 0);
        KeyActions0(true);




        if (Input.GetKeyDown(KeyCode.LeftArrow))
            //joints[0].robotPart.transform.Rotate(0, -Speed, 0);
        KeyActions0(false);

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
        
            KeyActions1(true);
           // joints[1].robotPart.transform.Rotate(0, 0, Speed);
        }




        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
           

            KeyActions1(false);
               // joints[1].robotPart.transform.Rotate(0, 0, -Speed);




        }


        if (Input.GetKeyDown(KeyCode.Q))
            joints[2].robotPart.transform.Rotate(0, 0, Speed);


        if (Input.GetKeyDown(KeyCode.A))
            joints[2].robotPart.transform.Rotate(0, 0, -Speed);


        if (Input.GetKeyDown(KeyCode.W))
            joints[3].robotPart.transform.Rotate(0, Speed, 0);



        if (Input.GetKeyDown(KeyCode.S))
            joints[3].robotPart.transform.Rotate(0, -Speed, 0);


        if (Input.GetKeyDown(KeyCode.E))
            joints[4].robotPart.transform.Rotate(0, 0, Speed);



        if (Input.GetKeyDown(KeyCode.D))
            joints[4].robotPart.transform.Rotate(0, 0, -Speed);


        if (Input.GetKeyDown(KeyCode.R))
            joints[5].robotPart.transform.Rotate(0, Speed, 0);


        if (Input.GetKeyDown(KeyCode.F))
            joints[5].robotPart.transform.Rotate(0, -Speed, 0);
        /*

        if (Input.GetKeyDown(KeyCode.T)) {

     
           joints[6].robotPart.transform.Rotate(Speed, 0, 0);
        }
      



        if (Input.GetKeyDown(KeyCode.G))
        {
    
       joints[6].robotPart.transform.Rotate(-Speed, 0, 0);
        }
           */


        if (Input.GetKeyDown(KeyCode.PageDown))
            KeyActions7();



    }

  

   

    void KeyActions0(bool rotateClockwise) {
        MagnetOn = true;
        int rotationDirection = rotateClockwise ? 1 : -1;

        joints[0].robotPart.transform.Rotate(0, rotationDirection * Speed, 0);
    }

    void KeyActions1(bool rotateClockwise)
    {
        MagnetOn = true;
        int rotationDirection = rotateClockwise ? 1 : -1;
        joints[1].robotPart.transform.Rotate(0, 0, rotationDirection * Speed);
    }

    void KeyActions2() {
        MagnetOn = true;
        joints[2].robotPart.transform.Rotate(0, 0, Speed);
 }

 void KeyActions2N()
 {
        MagnetOn = true;
        joints[2].robotPart.transform.Rotate(0, 0, -Speed);  
 }
 void KeyActions3()
 {
        MagnetOn = true;

        joints[3].robotPart.transform.Rotate(0, Speed, 0);
    }
 void KeyActions3N()
 {
        MagnetOn = true;
        joints[3].robotPart.transform.Rotate(0, -Speed, 0);
    }
 void KeyActions4() {
        joints[4].robotPart.transform.Rotate(0, 0, Speed);
 }
 void KeyActions4N()
 {
        
        joints[4].robotPart.transform.Rotate(0, 0, -Speed);
    }

 void KeyActions5() {
        joints[5].robotPart.transform.Rotate(0, Speed, 0);
       
 }
 void KeyActions5N()
 {
       
        joints[5].robotPart.transform.Rotate(0, -Speed, 0);
    }
    /*
 void KeyActions6() {
        joints[6].robotPart.transform.Rotate(Speed, 0, 0);
 }
 void KeyActions6N()
 {
        joints[6].robotPart.transform.Rotate(-Speed, 0, 0);
        
 }
    */
 void KeyActions7() {
        
        if (IfBox && IfTarget) {
         
            MagnetOn = false; 
        }
       
      
 }
    

    void RandomPosition(GameObject box, GameObject target)
    {
        Vector3 randomOffsetBox = new Vector3(UnityEngine.Random.Range(0.3f, -0.3f), transform.localPosition.y, UnityEngine.Random.Range(0.2f, 0.27f));
        Vector3 randomOffsetTarget = new Vector3(UnityEngine.Random.Range(-0.3f, 0.3f), transform.localPosition.y, UnityEngine.Random.Range(-0.5f, -0.4f));
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

        if (IfTarget)
        {
            return;
        }

        updatedDistanceToTarget = GetDistanceForTwo(Magnet, Target);

        if (updatedDistanceToTarget < StartDistanseToTarget )
        {
            AddReward(0.1f);
            StartDistanseToTarget = updatedDistanceToTarget;
            trainingTimer = 0;
            plusAR += 0.1f;
        }
        else
        {
            if (inputAi && updatedDistanceToTarget>0.56)
            {
                AddReward(-0.001f);
                minusAR += 0.1f;
                trainingTimer++;
            }
            else
            {
                trainingTimer++;
            }


        }
    
        if (trainingTimer > 1500)
        {
            Debug.Log("GoToTarget Gameover");
            Debug.Log("plus: " + plusAR + " vs minus: " + minusAR);
            EndEpisode();
        }

    }
    void GoToBox() 
    {
        if (IfTarget)
        {
            updatedDistanceToBox = GetDistanceForTwo(Magnet, Box);

            if (updatedDistanceToBox < StartDistanseToBox)
            {
                AddReward(0.2f);
                StartDistanseToBox = updatedDistanceToBox;
                trainingTimer = 0;
                plusAR += 0.2f;
            }
            else
            {
                if (inputAi && updatedDistanceToTarget > 0.56)
                { 
                    AddReward(-0.001f);
                    minusAR += 0.001f;
                    trainingTimer++;
                }
                else
                {
                    trainingTimer++;
                }
            }

            // Assuming you want to increment the timer variable
         
             if (trainingTimer > 3000)
            {
                Debug.Log("GoToBox Gameover");
                Debug.Log("plus: " + plusAR + " vs minus: " + minusAR);
                EndEpisode();
            }
        }


    }

}
