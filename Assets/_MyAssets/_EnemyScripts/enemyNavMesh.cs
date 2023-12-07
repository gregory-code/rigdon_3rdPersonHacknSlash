using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.VFX;
using static UnityEngine.UI.GridLayoutGroup;

public class enemyNavMesh : MonoBehaviour, IEventDispatcher
{

    private Animator enemyAnimator;
    private NavMeshAgent enemyNavMeshAgent;
    private Transform player;
    private float animSpeed;



    private int attackIndex;

    [SerializeField] VisualEffect _slashVisualEffect;

    private Vector3 previousPos;

    private bool canMove = true;
    private bool withinRange = false;

    [SerializeField] KatanaHit katanaHit;
    [SerializeField] Transform _vfxPlacement;

    private float desiredSpeed = 3;

    private bool isAlive = true;

    private bool isLeader;

    void Awake()
    {
        enemyAnimator = GetComponent<Animator>();
        enemyNavMeshAgent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

        previousPos = transform.position;

        StartCoroutine(CheckAttack());
    }

    public void SetLeaderStatus(bool status)
    {
        desiredSpeed = (status) ? 6 : 3 ;
        enemyNavMeshAgent.speed = (status) ? 6 : 3 ;
        isLeader = status;
    }

    void Update()
    {

        float distance = (Vector3.Distance(transform.position, player.position));
        
        if (distance <= 2f)
        {
            withinRange = true;
        }
        else
        {
            withinRange = false;
        }

        if (canMove == false)
        {
            LerpAnim(0);
            return;
        }

        if (isLeader)
        {
            CheckLeaderDistance(distance);
        }
        else
        {
            CheckMoveDistance(distance);
        }
    }

    private IEnumerator CheckAttack()
    {
        while(isAlive)
        {
            yield return new WaitForSeconds(0.5f);

            int randomChance = (isLeader) ? 0 : UnityEngine.Random.Range(0, 5); 
            
            if(withinRange && randomChance == 0 && canMove == true)
            {
                canMove = false;
                enemyNavMeshAgent.isStopped = true;
                attackIndex = 0;
                enemyAnimator.SetBool("attack" + attackIndex, true);
            }
        }
    }

    private void CheckNextAttack()
    {
        attackIndex++;
        if (withinRange)
        {
            enemyAnimator.SetBool("attack" + attackIndex, true);
        }
    }

    private void AttackHit(string hitAnim)
    {
        List<GameObject> enemies = katanaHit.GetHitEnemies();

        foreach (GameObject enemy in enemies)
        {
            //HitScreenEffects();
            //SetFreshBlood(enemy);
            Health enemyHealth = enemy.GetComponent<Health>();
            enemyHealth.ChangeHealth(-5, transform.gameObject, hitAnim);
        }
    }

    private void createSwordVFX(Transform spawnLocation)
    {
        VisualEffect swordVFX = Instantiate(_slashVisualEffect, spawnLocation.transform.position, spawnLocation.transform.rotation);
        //swordVFX.transform.SetParent(spawnLocation);
        Destroy(swordVFX.gameObject, 2);
    }

    private void CheckLeaderDistance(float distance)
    {
        if (distance > 1.4f)
        {
            Move(player.position, 1);
        }
        else
        {
            enemyNavMeshAgent.isStopped = true;
            LerpAnim(0);
        }
    }

    private void CheckMoveDistance(float distance)
    {
        if (distance < 6 && distance > 4f)
        {
            enemyNavMeshAgent.isStopped = true;
            LerpAnim(0);
        }
        else if (distance > 6f)
        {
            withinRange = false;
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

        LerpAnim(desiredSpeed);

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
        animSpeed = Mathf.Lerp(animSpeed, target, 10 * Time.deltaTime);
        enemyAnimator.SetFloat("speed", animSpeed);
    }

    private IEnumerator SetBurst(float time)
    {
        enemyNavMeshAgent.destination = player.position;
        enemyNavMeshAgent.isStopped = false;
        yield return new WaitForSeconds(time);
        enemyNavMeshAgent.isStopped = true;
    }

    public void SendEvent(AnimEvent animEvent)
    {
        if (animEvent.soundEfx != null)
        {
            //playAudio(animEvent.soundEfx);
        }

        switch (animEvent.functionName)
        {
            case "FinishFlourish":
                enemyAnimator.SetBool("attack0", false);
                enemyAnimator.SetBool("attack1", false);
                enemyAnimator.SetBool("attack2", false);
                enemyAnimator.SetBool("attack3", false);
                enemyAnimator.SetBool("attack4", false);
                canMove = true;
                break;

            case "Attack":
                AttackHit(animEvent.hitAnim);
                break;

            case "StartSwing":
                createSwordVFX(_vfxPlacement);
                break;

            case "StepFoward":
                StartCoroutine(SetBurst(0.3f));
                break;

            case "CheckAttack":
                CheckNextAttack();
                break;

            case "SlowDown":
                if(withinRange) 
                    player.GetComponent<playerScript>().SlowDownAttacked(gameObject.transform);
                break;
        }
    }
}
