using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class boat : MonoBehaviour
{
    Vector3 location;

    void Start()
    {
        GameObject[] docks = GameObject.FindGameObjectsWithTag("dock");

        location = docks[Random.Range(0, docks.Length)].transform.position;
    }


    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, location, 1 * Time.deltaTime);
    }
}
