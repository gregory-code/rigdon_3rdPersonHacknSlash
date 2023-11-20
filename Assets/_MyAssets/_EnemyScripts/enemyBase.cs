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

    [SerializeField] GameObject hitEffect;
    [SerializeField] GameObject deathEffect;
    [SerializeField] GameObject deathSmokeEffect;

    [SerializeField] GameObject bloodEffect;
    [SerializeField] Transform[] indicatorSpawns;

    [SerializeField] GameObject damagepopPrefab;

    Animator enemyAnimator;
    Rigidbody enemyRigidbody;
    bool _bBeingExecuted;
    bool _bIsDead;

    [SerializeField] Transform player;
    [SerializeField] Transform killPos;

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

    private void TookDamage(float currentHealth, float amount, float maxHealth, GameObject instigator, string hitAnim)
    {
        enemyAnimator.SetTrigger(hitAnim);
        StartCoroutine(HitBack(-transform.forward, 4f, 0.1f));
        DamageEffects(amount);
    }

    private void DamageEffects(float damage)
    {
        GameObject damagePop = Instantiate(damagepopPrefab, FindObjectOfType<Canvas>().transform);
        damagePop.GetComponent<damagepop>().Init(indicatorSpawns, damage);

        GameObject hit = Instantiate(hitEffect, this.transform);
        Destroy(hit, 1);
    }

    private void BloodSpray(int spawn)
    {

        GameObject blood = Instantiate(bloodEffect, indicatorSpawns[spawn].position, indicatorSpawns[spawn].rotation);
        Destroy(blood, 1);
    }

    private void Death(float amount, float maxHealth)
    {
        enemyAnimator.SetTrigger("die1");
    }

    private void DeathSmoke()
    {
        HandleDeath();

        GameObject death = Instantiate(deathEffect, transform.position, transform.rotation);
        Destroy(death, 2);
        StartCoroutine(DeathDelay());
    }

    private void HandleDeath()
    {
        Destroy(healthBar.gameObject);
        GetComponent<BoxCollider>().center = new Vector3(0, 0.05f, 0);
        GetComponent<BoxCollider>().size = new Vector3(0.5f, 0.1f, 0.5f);
        gameObject.layer = 0;
        _bIsDead = true;
        _bBeingExecuted = false;
    }

    private IEnumerator DeathDelay()
    {
        yield return new WaitForSeconds(2);
        GameObject deathSmoke = Instantiate(deathSmokeEffect, transform.position, deathSmokeEffect.transform.rotation);
        Destroy(deathSmoke, 2);
        Destroy(this.gameObject);
    }

    public void KillSetup(int which)
    {
        _bBeingExecuted = true;
        enemyAnimator.SetTrigger("execute" + which);
    }

    private IEnumerator HitBack(Vector3 hitDir, float hitForce, float length)
    {
        hitDir = hitDir.normalized;
        hitDir.y = 0;
        while(length > 0)
        {
            hitForce *= 0.90f;
            length -= Time.deltaTime;
            enemyRigidbody.AddForce(hitDir * hitForce, ForceMode.Impulse);
            yield return new WaitForEndOfFrame();
        }
    }

    void Update()
    {
        if(_bIsDead == false) LookAtPlayer();
        if (_bBeingExecuted == true) BeingExecuted();
    }

    private void LookAtPlayer()
    {
        transform.LookAt(player.position);
        Quaternion roanofkds = transform.rotation;
        roanofkds.x = 0;
        roanofkds.z = 0;
        transform.rotation = roanofkds;
    }

    private void BeingExecuted()
    {
        transform.position = Vector3.Lerp(transform.position, killPos.position, 10 * Time.deltaTime);
    }
}
