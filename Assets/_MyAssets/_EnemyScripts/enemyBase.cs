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

    Animator enemyAnimator;
    Rigidbody enemyRigidbody;

    [SerializeField] Transform player;

    private void Awake()
    {
        healthBar = Instantiate(healthBarPrefab, FindObjectOfType<Canvas>().transform);
        UIAttachComponent attachmentComp = healthBar.AddComponent<UIAttachComponent>();
        attachmentComp.SetupAttachment(healthBarAttachTransform);

        enemyAnimator = GetComponent<Animator>();
        enemyRigidbody = GetComponent<Rigidbody>();

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
        enemyAnimator.SetTrigger("hit");
        StartCoroutine(HitBack(-transform.forward, 3f, 0.2f));
    }

    private void Death(float amount, float maxHealth)
    {
        
    }

    private IEnumerator HitBack(Vector3 hitDir, float hitForce, float length)
    {
        hitDir = hitDir.normalized;
        hitDir.y = 0;
        while(length > 0)
        {
            Debug.Log("Pushing");
            hitForce *= 0.95f;
            length -= Time.deltaTime;
            enemyRigidbody.AddForce(hitDir * hitForce, ForceMode.Impulse);
            yield return new WaitForEndOfFrame();
        }
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
