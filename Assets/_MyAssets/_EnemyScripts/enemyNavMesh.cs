using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;
using static UnityEngine.UI.GridLayoutGroup;

public class enemyNavMesh : MonoBehaviour
{

    private Animator enemyAnimator;
    private NavMeshAgent enemyNavMeshAgent;
    private Transform player;
    private float animSpeed;

    private Vector3 previousPos;

    private bool canMove = true;

    void Start()
    {
        enemyAnimator = GetComponent<Animator>();
        enemyNavMeshAgent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

        previousPos = transform.position;
    }

    void Update()
    {
        if (canMove == false)
            return;

        float distance = (Vector3.Distance(transform.position, player.position));
        CheckMoveDistance(distance);
    }

    private void CheckMoveDistance(float distance)
    {
        if (distance < 6 && distance > 4)
        {
            enemyNavMeshAgent.isStopped = true;
            LerpAnim(0);
            canMove = false;
            return;
        }
        else if (distance > 6)
        {
            Move(player.position, 1);
        }
        else
        {
            Vector3 behindPosition = transform.position - (transform.forward * 10);
            Move(behindPosition, -1);
        }
    }

    private void Move(Vector3 destination, float fowardSpeed)
    {
        enemyNavMeshAgent.isStopped = false;
        enemyAnimator.SetFloat("fowardSpeed", fowardSpeed);
        canMove = true;

        LerpAnim(3);

        previousPos = transform.position;
        enemyNavMeshAgent.destination = destination;
    }

    public void SetActive(bool state)
    {
        enemyNavMeshAgent.isStopped = !state;
        canMove = state;
    }

    private void LerpAnim(float target)
    {
        animSpeed = Mathf.Lerp(animSpeed, target, 20 * Time.deltaTime);
        enemyAnimator.SetFloat("speed", animSpeed);
    }
}
