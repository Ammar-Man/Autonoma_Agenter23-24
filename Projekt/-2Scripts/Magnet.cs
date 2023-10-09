using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnet : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject Target;
    public bool ON = true;
    public bool callON = false;
    public bool IfTarget = false;
    public bool IfBox = false;
    void Start()
    {
        ON = true; 
    }

    // Update is called once per frame
    void Update()
    {

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

    void RandomPosition(GameObject name)
    {
        float x = UnityEngine.Random.Range(-0.4f, 0.4f);
        float z = UnityEngine.Random.Range(-0.5f, -0.27f);
        name.transform.localPosition = new Vector3(0, transform.localPosition.y, 0);
    }
   
    public void SetNOFalse(bool input)
    {
        

    MoveTargetToMagnet(input);
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
           
            IfBox = true;
        }

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

    }
}
