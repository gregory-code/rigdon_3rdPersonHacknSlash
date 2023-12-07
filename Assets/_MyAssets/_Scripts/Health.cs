using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public delegate void OnHealthChanged(float currentHealth, float delta, float maxHealth);
    public delegate void OnHealthEmpty(float delta, float maxHealth);
    public delegate void OnTakenDamage(float currentHealth, float delta, float maxHealth, GameObject instigator, string hitAnim);

    public event OnHealthChanged onHealthChanged;
    public event OnHealthEmpty onHealthEmpty;
    public event OnTakenDamage onTakenDamage;

    [SerializeField] float currentHealth;
    [SerializeField] float maxHealth;

    public void GetFireDamage(GameObject firePrefab)
    {
        GameObject fire = Instantiate(firePrefab);
        fire.transform.SetParent(this.transform);
        fire.transform.localPosition = Vector3.zero;
        Destroy(fire, 3.5f);
        StartCoroutine(FireDamageCoroutine());
    }

    private IEnumerator FireDamageCoroutine()
    {
        yield return new WaitForSeconds(1);
        ChangeHealth(-5, this.gameObject, "none");
        yield return new WaitForSeconds(1);
        ChangeHealth(-5, this.gameObject, "none");
        yield return new WaitForSeconds(1);
        ChangeHealth(-5, this.gameObject, "none");
    }

    public void ChangeHealth(float amount, GameObject target, string hitAnim)
    {
        //using early return.
        if (amount == 0 || currentHealth == 0)
        {
            return;
        }

        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);

        onHealthChanged?.Invoke(currentHealth, amount, maxHealth);
        if (amount < 0)
        {
            onTakenDamage?.Invoke(currentHealth, amount, maxHealth, target, hitAnim);
        }

        if (currentHealth == 0)
        {
            onHealthEmpty?.Invoke(amount, maxHealth);
        }
    }

    internal bool IsFull()
    {
        if (currentHealth >= maxHealth)
            return true;

        return false;
    }

    internal float GetHealth()
    {
        return currentHealth;
    }

    internal float GetMaxHealth()
    {
        return maxHealth;
    }


}
