using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using UnityEngine;

public class GateController : Agent
{
    bool closed = true;
    bool opening = false;
    float speed = 0.1f;
    float OpenGap = 12.0f;
    Vector3 startPosition;


    //Lyssnar p� "kommandon" fr�n neruonn�tet
    public override void OnActionReceived(ActionBuffers actions)
    {
        Debug.Log(GetCumulativeReward());
        Debug.Log("Lyssnar");

        var DiscreteActions = actions.DiscreteActions;

        if (DiscreteActions[0] == 1)
            OpenGate();   
    }


    //Om vi k�r simulationen i "m�nniskostyrt l�ge" s� st�ller vi in vilka "actions"
    //som ska utf�ras
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var DiscreteActions = actionsOut.DiscreteActions;

        DiscreteActions[0] = 0;

        if (Input.GetKey(KeyCode.Space))
        {
            DiscreteActions[0] = 1;  
        }
    }


    // Funktion definierad Agent-klassen f�r initialisering av variabler
    public override void Initialize()
    {
        startPosition = transform.position;
    }

    //Anropas varje g�ng som en ny "episod" p�b�rjars, dvs. i praktiken efter anrop av
    //EndEpisode()
    public override void OnEpisodeBegin()
    {
        Reset();
    }


    private void Reset()
    {
        transform.position = startPosition;
        closed = true;
        opening = false;
    }



    //Vid regelbundna tidsintervall
    void FixedUpdate()
    {
        //Om porten inte �r st�ngd, skjut den till v�nster eller h�ger
        if (!closed)
            if (opening) ShiftRight();
            else ShiftLeft();

        //Om portens x-position �r mindre �n ursprungspositionen, l�s porten vid ursprungspositionen
        if (transform.position.x < startPosition.x)
        {
            closed = true;
            transform.position = startPosition;
        }

        //Om porten �r maximalt �ppnad
        if ((transform.position.x - startPosition.x) > OpenGap)
        {
            transform.position = new Vector3(startPosition.x + OpenGap, transform.position.y, transform.position.z);
            opening = false;
        }

        if (closed)
        {
            RequestDecision();
        }
    }

    void OpenGate()
    {
        AddReward(0.001f);
        closed = false;
        opening = true;
    }

    //Anroas om bilen tr�ffar "r�relsesensorn" bakom porten
    public void PassedGate()
    {
        //h�r ska agenten f� en bel�ning
        AddReward(0.1f);
        Debug.Log("AddReward 0.1f");
    }

    void ShiftRight()
    {
        transform.Translate(speed, 0, 0);
    }

    void ShiftLeft()
    {
        transform.Translate(-speed, 0, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Cars")
        {
            AddReward(-1.0f);
            Debug.Log(GetCumulativeReward());
            Debug.Log("AddReward -1.0f");
            Destroy(other.gameObject);
            EndEpisode();
        }
    }
}
