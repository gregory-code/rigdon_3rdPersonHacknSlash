using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class enemyNavMesh : MonoBehaviour
{

    private Animator enemyAnimator;
    private NavMeshAgent enemyNavMeshAgent;
    private Transform player;
    private float animSpeed;

    private Vector3 previousPos;

    private bool canMove;

    void Start()
    {
        enemyAnimator = GetComponent<Animator>();
        enemyNavMeshAgent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

        previousPos = transform.position;
    }

    void Update()
    {
        if(!canMove)
        {
            enemyNavMeshAgent.isStopped = true;
            LerpAnim(0);
        }

        enemyNavMeshAgent.isStopped = false;

        previousPos = transform.position;

        enemyNavMeshAgent.destination = player.position;

        LerpAnim(3);
    }

    private void LerpAnim(float target)
    {
        animSpeed = Mathf.Lerp(animSpeed, target, 6 * Time.deltaTime);
        enemyAnimator.SetFloat("speed", animSpeed);
    }
}
