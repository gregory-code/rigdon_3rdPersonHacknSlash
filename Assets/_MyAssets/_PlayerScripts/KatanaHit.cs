using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KatanaHit : MonoBehaviour
{
    [SerializeField] private List<GameObject> hitEnemies = new List<GameObject>();

    [SerializeField] string enemyTag;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == enemyTag)
        {
            hitEnemies.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == enemyTag)
        {
            StartCoroutine(ForgetDelay(other.gameObject));
        }
    }

    private IEnumerator ForgetDelay(GameObject enemy)
    {
        yield return new WaitForSeconds(0.2f);
        if (hitEnemies.Contains(enemy))
        {
            hitEnemies.Remove(enemy);
        }
    }

    public List<GameObject> GetHitEnemies()
    {
        return hitEnemies;
    }
}
