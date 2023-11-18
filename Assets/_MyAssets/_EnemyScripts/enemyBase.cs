using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;

public class enemyBase : MonoBehaviour
{
    Animator _animator;

    [SerializeField] EnemyHealth healthBarPrefab;
    [SerializeField] Transform healthBarAttachTransform;
    Health healthComponet;
    EnemyHealth healthBar;

    [SerializeField] Transform player;

    private void Awake()
    {
        healthBar = Instantiate(healthBarPrefab, FindObjectOfType<Canvas>().transform);
        UIAttachComponent attachmentComp = healthBar.AddComponent<UIAttachComponent>();
        attachmentComp.SetupAttachment(healthBarAttachTransform);

        healthComponet = GetComponent<Health>();

        healthComponet.onHealthChanged += HealthChanged;
        healthComponet.onTakenDamage += TookDamage;
        healthComponet.onHealthEmpty += Death;
    }

    private void HealthChanged(float currentHealth, float amount, float maxHealth)
    {
        healthBar.SetValue(currentHealth, maxHealth);
    }

    private void TookDamage(float currentHealth, float amount, float maxHealth, GameObject instigator)
    {

    }

    private void Death(float amount, float maxHealth)
    {
        
    }

    void Start()
    {
        
    }

    void Update()
    {
        transform.LookAt(player.position);
        Quaternion roanofkds = transform.rotation;
        roanofkds.x = 0;
        roanofkds.z = 0;
        transform.rotation = roanofkds;
    }
}
