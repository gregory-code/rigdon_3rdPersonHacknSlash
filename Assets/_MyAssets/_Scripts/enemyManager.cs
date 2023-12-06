using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyManager : MonoBehaviour
{
    enemyNavMesh[] allActiveEnemies;
    bool gameIsLive = true;

    void Start()
    {
        allActiveEnemies = GameObject.FindObjectsOfType<enemyNavMesh>();
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
        int randomEnemy = Random.Range(0, allActiveEnemies.Length);
        foreach (enemyNavMesh enemy in allActiveEnemies)
        {
            enemy.SetLeaderStatus(false);
        }
        allActiveEnemies[randomEnemy].SetLeaderStatus(true);
    }

    void Update()
    {
        
    }
}
