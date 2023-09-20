using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSpawner : MonoBehaviour
{
    public GameObject CarPf;
    public GameObject SpawnPoint;
    public GameObject TaxiPf;
    float SpawnTime = 0;
    float SpawnDelay = 0;

    // Start is called before the first frame update
    void Start()
    {
      //  RandowmCarsTag = GameObject.FindWithTag("RandomCars");
        SpawnDelay = Random.Range(5, 10);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
        if ((Time.time - SpawnTime) > SpawnDelay)
        {
           
            float RaNu = Random.Range(1, 10);
            Debug.Log(RaNu);
            if (RaNu > 5)
            {
                Spawn(CarPf);
            }
            else
            {
                Spawn(TaxiPf);
            }
        }
       
    }

    void Spawn(GameObject name)
    {
      //  CarPf = RandowmCarsTag;//
        Debug.Log(name);
        GameObject CarT = Instantiate(name, SpawnPoint.transform.position, SpawnPoint.transform.rotation);
        CarT.transform.parent = transform;
        SpawnTime = Time.time;
        SpawnDelay = Random.Range(5, 10);
    }
}
