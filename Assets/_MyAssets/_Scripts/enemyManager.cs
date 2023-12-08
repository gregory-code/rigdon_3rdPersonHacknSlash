using System.Collections;
using System.Collections.Generic;
using TMPro;
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

    [SerializeField] Animator waveAnimator;

    [SerializeField] TextMeshProUGUI waveText;
    [SerializeField] TextMeshProUGUI detailsText;
    private int killAmount = 0;
    [SerializeField] TextMeshProUGUI killText;

    private int spawned = 1;
    private int difficultly = 0;

    void Start()
    {
        NewLeader();
        StartCoroutine(leaderCheck());
        StartCoroutine(SpawnTimer());
    }

    public void updateWaveText()
    {
        difficultly++;

        switch (spawned)
        {
            case 5:
                waveText.text = "Wave Difficulty: Medium";
                detailsText.text = "Enemies are at average strength";
                break;

            case 12:
                waveText.text = "Wave Difficulty: Hard";
                detailsText.text = "Enemies are faster and stronger";
                break;

            case 20:
                waveText.text = "Wave Difficulty: Impossible";
                detailsText.text = "You won't live much longer";
                break;
        }
    }

    public void increaseKillCount()
    {
        killAmount++;
        killText.text = "Kills: " + killAmount;
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
            spawned++;
            if(spawned == 5 || spawned == 12 || spawned == 20)
            {
                Debug.Log("Should Increase Diff");
                waveAnimator.SetTrigger("floatTo");
            }
            GameObject.Find("spawnSound").GetComponent<AudioSource>().Play();
            int randomSpawn = Random.Range(0, spawnParticles.Length);
            GameObject enemy = Instantiate(enemyPrefab, spawnParticles[randomSpawn].transform.position, spawnParticles[randomSpawn].transform.rotation);
            enemy.GetComponent<enemyNavMesh>().Init(difficultly);
            Instantiate(particle, spawnParticles[randomSpawn].transform.position, spawnParticles[randomSpawn].transform.rotation);

            if(waitTimer > 3)
                waitTimer -= 0.5f;
        }
    }

    private void NewLeader()
    {
        Debug.Log("Trying to assign new leader");
        enemyNavMesh[] activeEnemies = GameObject.FindObjectsOfType<enemyNavMesh>();
        int maxNum = 1;
        if(activeEnemies.Length > 3)
        {
            maxNum = 2;
        }
        if(activeEnemies.Length > 5)
        {
            maxNum = 3;
        }
        foreach (enemyNavMesh enemy in activeEnemies)
        {
            if(Random.Range(0, maxNum) == 0)
            {
                enemy.SetLeaderStatus(true);
            }
            else
            {
                enemy.SetLeaderStatus(false);
            }
        }
    }
}
