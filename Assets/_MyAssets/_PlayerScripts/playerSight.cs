using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerSight : MonoBehaviour
{
    [SerializeField] float sightRange = 20f;
    [SerializeField] float eyeHeight = 0f;
    [SerializeField] float halfPeripheralAngle = 45f;

    [SerializeField] float grabRadius = 15;
    [SerializeField] LayerMask enemyLayerMask;

    Transform _targetedEnemy;
    Collider[] nearbyEnemies;

    playerScript player;


    public void Start()
    {
        player = GameObject.FindObjectOfType<playerScript>();
        player.onTargetLockUpdated += TargetLockUpdated;
    }

    private void TargetLockUpdated(bool state, Transform target)
    {
        _targetedEnemy = (state) ? target : null;
    }

    public Transform GetClosestEnemy(bool shiftFocus, bool shiftLeft)
    {
        List<Transform> inRangeEnemies = GetEnemiesInRange();
        if (inRangeEnemies.Count <= 0) 
            return null;

        if(shiftFocus == false)
        {
            return GetClosestOnDistance(inRangeEnemies);
        }

        if(shiftLeft)
        {
            return GetClosestOnLeft(inRangeEnemies);
        }
        else
        {
            return GetClosestOnRight(inRangeEnemies);
        }
    }

    private Transform GetClosestOnDistance(List<Transform> enemylist)
    {
        float closestValue = float.MaxValue;
        Transform closestDis = null;

        foreach (Transform enemy in enemylist)
        {
            if (enemy.transform.root == _targetedEnemy) continue;
            float dis = Vector3.Distance(enemy.position, player.transform.position);

            if (dis < closestValue)
            {
                closestValue = dis;
                closestDis = enemy.transform.root;
            }
        }

        return closestDis;
    }

    private Transform GetClosestOnLeft(List<Transform> enemylist)
    {
        float closestAngle = float.MinValue;
        Transform closestEnemy = null;

        foreach (Transform enemy in enemylist)
        {
            if (enemy.transform.root == _targetedEnemy) continue;

            float angle = CheckFocusAngle(enemy);
            if (angle >= 0) continue;

            if (angle > closestAngle)
            {
                closestEnemy = enemy.transform.root;
                closestAngle = angle;
            }
        }

        return closestEnemy;
    }

    private Transform GetClosestOnRight(List<Transform> enemylist)
    {
        float closestAngle = float.MaxValue;
        Transform closestEnemy = null;

        foreach (Transform enemy in enemylist)
        {
            if (enemy.transform.root == _targetedEnemy) continue;

            float angle = CheckFocusAngle(enemy);
            if (angle < 0) continue;

            if (angle < closestAngle)
            {
                closestEnemy = enemy.transform.root;
                closestAngle = angle;
            }
        }

        return closestEnemy;
    }

    private float CheckFocusAngle(Transform enemy)
    {
        Vector3 stimuliDir = (enemy.transform.position - transform.position).normalized;
        Vector3 ownerForward = transform.forward;

        return (Vector3.Cross(stimuliDir, ownerForward).y);
    }

    private List<Transform> GetEnemiesInRange()
    {
        nearbyEnemies = Physics.OverlapSphere(transform.position, grabRadius, enemyLayerMask);

        List<Transform> enemiesInRange = new List<Transform>();
        
        if (nearbyEnemies.Length <= 0) return enemiesInRange;


        foreach (Collider t in nearbyEnemies)
        {
            if (Vector3.Distance(t.transform.position, transform.position) > sightRange)
            {
                continue;
            }

            Vector3 stimuliDir = (t.transform.position - transform.position).normalized;
            Vector3 ownerForward = transform.forward;

            if (Vector3.Angle(stimuliDir, ownerForward) > halfPeripheralAngle)
            {
                continue;
            }

            enemiesInRange.Add(t.transform);
        }

        return enemiesInRange;
    }

    private void OnDrawGizmos()
    {
        Vector3 drawCenter = transform.position + Vector3.up * eyeHeight;
        //Gizmos.DrawWireSphere(drawCenter, sightRange);

        Gizmos.color = Color.green;

        Vector3 leftDir = Quaternion.AngleAxis(halfPeripheralAngle, Vector3.up) * transform.forward;
        Vector3 rightDir = Quaternion.AngleAxis(-halfPeripheralAngle, Vector3.up) * transform.forward;

        Gizmos.DrawLine(drawCenter, drawCenter + leftDir * sightRange);
        Gizmos.DrawLine(drawCenter, drawCenter + rightDir * sightRange);

        Gizmos.color = Color.yellow;

        Gizmos.DrawWireSphere(transform.position, grabRadius);
    }
}
