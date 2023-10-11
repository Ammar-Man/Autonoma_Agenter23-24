using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.XR;
using static UnityEngine.GraphicsBuffer;


public class RobotController : MonoBehaviour
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

    private void Start()
    {

        FindGameObjectsByName();
        InitializeMagnet();
        RandomPosition(Box, Target);
        MagnetOn = true;
        


    }
    private void Update()
    {
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
        pincherController = Hand.GetComponent<PincherController>();
        MagnetScript = MagnetField.GetComponent<Magnet>();
        MagnetScript.MoveTargetToMagnet(ON);
        StartDistanseToTarget = GetDistanceForTwo(Magnet, Target);
        StartDistanseToBox = GetDistanceForTwo(Magnet, Box);
    }
    private void HandleMagnetLogic()
    {
       
        ManualControl();
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
            Debug.Log("5");
            Debug.Log("EndEpisod()");
        }

        if (PageDown && IfTarget )
        {
            Debug.Log("-2");
          
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
       /*if (Input.GetKeyDown(KeyCode.PageUp))
            MagnetOn = true;
       */
        if (Input.GetKeyDown(KeyCode.PageDown)) {
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
        Vector3 randomOffsetBox = new Vector3(UnityEngine.Random.Range(-0.4f, 0.4f), transform.localPosition.y, UnityEngine.Random.Range(-0.5f, -0.3f));
        Vector3 randomOffsetTarget = new Vector3(UnityEngine.Random.Range(-0.4f, 0.4f), transform.localPosition.y, UnityEngine.Random.Range(-0.5f, -0.27f));

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
            StartDistanseToTarget = updatedDistanceToTarget;
        }

        if (updatedDistanceToTarget > StartDistanseToTarget)
        {
            Debug.Log("- go target : -0.1f");
            StartDistanseToTarget = updatedDistanceToTarget;
        }
    }
    void GoToBox() 
    {
        updatedDistanceToBox = GetDistanceForTwo(Magnet, Box);

        if (IfBox)
        {
            Debug.Log("Box with: 0.001f");
            return;
        }

        if (updatedDistanceToBox < StartDistanseToBox)
        {
            Debug.Log("go box: 0.1f");
            StartDistanseToBox = updatedDistanceToBox;
        }
        if (updatedDistanceToBox > StartDistanseToBox)
        {
            Debug.Log("- go box: -0.1f");
            StartDistanseToBox = updatedDistanceToBox;
        }

        
    }




}
