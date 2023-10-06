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

    public GameObject Hand;
    public GameObject Box;
    public GameObject Target;
    public GameObject Magnet;
    PincherController pincherController;
    public bool NO = false;
    public bool MagnetOn = true;

    public float updatedDistanceToTarget;
    public float TargetDistance;
    public float StartDistanseToTarget;


    public float updatedDistanceToBox;
    public float BoxDistance;
    public float StartDistanseToBox;

    public bool TargetInHand = false;
    public bool IfTarget = false;
    public bool IfBox = false;

    private void Start()
    {
        // RandomPosition(Box, Target);
        MagnetOn = true;
        pincherController = Hand.GetComponent<PincherController>();

       GameObject MS = GameObject.Find("Magnet fild");
        Magnet scriptM = MS.GetComponent<Magnet>();
        scriptM.MoveTargetToMagnet(NO);
        StartDistanseToTarget = GetDistanceForTwo(Magnet, Target);
        StartDistanseToBox = GetDistanceForTwo(Magnet, Box);
        
    }
    private void Update()
    {
        GameObject MS = GameObject.Find("Magnet fild");
        Magnet scriptM = MS.GetComponent<Magnet>();
        ManualControl();
        scriptM.MoveTargetToMagnet(NO);
        
        IfTarget = scriptM.IfTarget;
        IfBox = scriptM.IfBox;
        scriptM.callON = MagnetOn;

        BoxDistance = GetDistanceForTwo(Magnet,Box);
        TargetDistance = GetDistanceForTwo(Magnet,Target);


        if (IfTarget)
        {
            Debug.Log(" go Target: 0.001f");
           
        }

        if (IfTarget && IfBox)
        {
            Debug.Log(" go Target: 0.001f");

        }


        if (TargetInHand )
        {
            Debug.Log("Target in hand: 2 f");
            TargetInHand =false;
        }

        if (scriptM.CompareTag("Target"))
        {
            Debug.Log("Magnet colliderat med Target!! ;)");
            // Code to handle when the object has the "Target" tag
        }

        // GoToTarget();
    }


    public void ManualControl()
    {

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
                        Input.GetKeyDown(KeyCode.PageUp) ||
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
        if(Input.GetKeyDown(KeyCode.PageDown) && IfBox && IfTarget)
        {
            Debug.Log("EndEpisod()");
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
        if (Input.GetKeyDown(KeyCode.PageUp))
            MagnetOn = true;
        if (Input.GetKeyDown(KeyCode.PageDown))
                MagnetOn =  false;

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

    void RandomPosition(GameObject one, GameObject two)
    {
        
           float x = UnityEngine.Random.Range(-0.4f, 0.4f);
          float z = UnityEngine.Random.Range(-0.5f, 0.35f);
        one.transform.localPosition = new Vector3(x, transform.localPosition.y, z);

         x = UnityEngine.Random.Range(-0.4f, 0.4f);
         z = UnityEngine.Random.Range(-0.5f, -0.27f);
        two.transform.localPosition = new Vector3(x, transform.localPosition.y, z);


    }


    float GetDistanceForTwo(GameObject to, GameObject from)
    {
        float dist = Vector3.Distance(from.transform.position, to.transform.position);
        return dist;
    }

    void GoToTarget()
    {
        updatedDistanceToTarget = GetDistanceForTwo(Magnet, Target);

      

        if (updatedDistanceToTarget < StartDistanseToTarget)
        {
  //          AddReward(0.1f); // Lägg till poäng om agenten rör sig närmare målet
            Debug.Log("go target : 0.1f");
            StartDistanseToTarget = updatedDistanceToTarget;
        }
        if (updatedDistanceToTarget > StartDistanseToTarget)
        {
            Debug.Log("- go target : -0.1f");
//            AddReward(-0.1f); // Lägg till poäng om agenten rör sig närmare målet
                              // Debug.Log("Distance decreased! Current score: -0.1f");
            StartDistanseToTarget = updatedDistanceToTarget;
        }
    }
    void GoToBox() 
    {
        updatedDistanceToBox = GetDistanceForTwo(Magnet, Box);

        if (IfBox)
        {
            Debug.Log(" If Box: 0.001f");
            return;
        }

        if (updatedDistanceToBox < StartDistanseToBox)
        {
            //          AddReward(0.1f); // Lägg till poäng om agenten rör sig närmare målet
            Debug.Log("go box: 0.1f");
            StartDistanseToBox = updatedDistanceToBox;
        }
        if (updatedDistanceToBox > StartDistanseToBox)
        {
            Debug.Log("- go box: -0.1f");
            //            AddReward(-0.1f); // Lägg till poäng om agenten rör sig närmare målet
            // Debug.Log("Distance decreased! Current score: -0.1f");
            StartDistanseToBox = updatedDistanceToBox;
        }

        
    }

    private void OnTriggerEnter(Collider other)
    {
  
    }


}
