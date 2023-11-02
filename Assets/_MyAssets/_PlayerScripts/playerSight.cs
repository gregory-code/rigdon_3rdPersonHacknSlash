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

    Collider[] nearbyEnemies;
    List<Transform> inRangeEnemies = new List<Transform>();

    public Transform GetClosestEnemy()
    {
        nearbyEnemies = Physics.OverlapSphere(transform.position, grabRadius, enemyLayerMask);

        if (nearbyEnemies.Length <= 0) return null;

        inRangeEnemies.Clear();

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

            inRangeEnemies.Add(t.transform);
        }

        if (inRangeEnemies.Count <= 0) return null;

        float closestValue = 9999;
        Transform closestEnemy = null;

        foreach (Transform enemy in inRangeEnemies)
        {
            float dis = Vector3.Distance(enemy.position, transform.position);
            if (dis < closestValue)
            {
                closestValue = dis;
                closestEnemy = enemy;
            }
        }

        return closestEnemy.transform.root;
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
