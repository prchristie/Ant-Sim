using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntSpawner : MonoBehaviour
{
    public int numAnts = 300;

    public GameObject antPrefab;


    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < numAnts; i++) {
            GameObject ant = Instantiate(antPrefab, transform.position, Quaternion.identity);
            ant.GetComponent<AntFSM>().homeNest = gameObject;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
