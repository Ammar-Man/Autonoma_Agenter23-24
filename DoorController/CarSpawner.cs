using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSpawner : MonoBehaviour
{
    public GameObject CarPf;
    public GameObject SpawnPoint;
    float SpawnTime = 0;
    float SpawnDelay = 0;

    // Start is called before the first frame update
    void Start()
    {
        SpawnDelay = Random.Range(5, 10);
    }

    // Update is called once per frame
    void FixedUpdate()
    {   
        if((Time.time - SpawnTime) > SpawnDelay)
            Spawn();
    }

    void Spawn()
    {
        GameObject Car = Instantiate(CarPf, SpawnPoint.transform.position, SpawnPoint.transform.rotation);
        Car.transform.parent = transform;
        SpawnTime = Time.time;
        SpawnDelay = Random.Range(5, 10);
    }
}
