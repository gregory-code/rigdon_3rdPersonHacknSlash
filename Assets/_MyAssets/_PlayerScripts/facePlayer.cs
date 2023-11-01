using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class facePlayer : MonoBehaviour
{
    [SerializeField] Transform player;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(player.position);
        Quaternion roanofkds = transform.rotation;
        roanofkds.x = 0;
        roanofkds.z = 0;
        transform.rotation = roanofkds;
    }
}
