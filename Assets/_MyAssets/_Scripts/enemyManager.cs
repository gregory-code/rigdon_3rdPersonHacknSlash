using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyManager : MonoBehaviour
{
    bool gameIsLive = true;

    void Start()
    {
        NewLeader();
        StartCoroutine(leaderCheck());
    }

    private IEnumerator leaderCheck()
    {
        while(gameIsLive)
        {
            yield return new WaitForSeconds(8);
            NewLeader();
        }
    }

    private void NewLeader()
    {

        enemyNavMesh[] activeEnemies = GameObject.FindObjectsOfType<enemyNavMesh>();
        int randomEnemy = Random.Range(0, activeEnemies.Length);
        foreach (enemyNavMesh enemy in activeEnemies)
        {
            enemy.SetLeaderStatus(false);
        }

        activeEnemies[randomEnemy].SetLeaderStatus(true);
    }

    void Update()
    {
        
    }
}
