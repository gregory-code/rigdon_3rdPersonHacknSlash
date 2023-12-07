using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fireBurn : MonoBehaviour
{
    [SerializeField] GameObject firePrefab;

    private void OnTriggerEnter(Collider other)
    {
        Health healthComponent = other.GetComponent<Health>();
        if (healthComponent != null)
        {
            healthComponent.GetFireDamage(firePrefab);
        }
    }

}
