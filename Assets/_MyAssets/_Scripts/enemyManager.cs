using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyManager : MonoBehaviour
{
    public List<enemyNavMesh> allActiveEnemies = new List<enemyNavMesh>();
    bool gameIsLive = true;

    void Start()
    {
        Refresh("");
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

    public void Refresh(string excludeName)
    {
        allActiveEnemies.Clear();

        enemyNavMesh[] startingEnemies = GameObject.FindObjectsOfType<enemyNavMesh>();
        foreach (enemyNavMesh enemy in startingEnemies)
        {

            if(enemy.name != excludeName)
            {
                allActiveEnemies.Add(enemy);
            }
            
            
        }
    }

    private void NewLeader()
    {
        int randomEnemy = Random.Range(0, allActiveEnemies.Count);
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
