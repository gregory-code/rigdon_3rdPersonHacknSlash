using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class enemyManager : MonoBehaviour
{
    bool gameIsLive = true;
    [SerializeField] ParticleSystem particle;
    [SerializeField] Transform[] spawnParticles;
    [SerializeField] GameObject enemyPrefab;
    private float waitTimer = 10;

    void Start()
    {
        NewLeader();
        StartCoroutine(leaderCheck());
        StartCoroutine(SpawnTimer());
    }

    //sound effects
    //wave time/text
     //start/pause/lose
     //rage mechanic(if I still want)
     //sound settings
      //credits

    private IEnumerator leaderCheck()
    {
        while(gameIsLive)
        {
            yield return new WaitForSeconds(6);
            NewLeader();
        }
    }

    private IEnumerator SpawnTimer()
    {
        while (gameIsLive)
        {
            yield return new WaitForSeconds(waitTimer);
            int randomSpawn = Random.Range(0, spawnParticles.Length);
            Instantiate(enemyPrefab, spawnParticles[randomSpawn].transform.position, spawnParticles[randomSpawn].transform.rotation);
            Instantiate(particle, spawnParticles[randomSpawn].transform.position, spawnParticles[randomSpawn].transform.rotation);

            if(waitTimer > 3)
                waitTimer -= 0.5f;
        }
    }

    private void NewLeader()
    {
        Debug.Log("Trying to assign new leader");
        enemyNavMesh[] activeEnemies = GameObject.FindObjectsOfType<enemyNavMesh>();
        int randomEnemy = Random.Range(0, activeEnemies.Length);
        foreach (enemyNavMesh enemy in activeEnemies)
        {
            enemy.SetLeaderStatus(false);
        }

        activeEnemies[randomEnemy].SetLeaderStatus(true);
    }
}
