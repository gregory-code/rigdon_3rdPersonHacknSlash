using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public delegate void OnHealthChanged(float currentHealth, float delta, float maxHealth);
    public delegate void OnHealthEmpty(float delta, float maxHealth);
    public delegate void OnTakenDamage(float currentHealth, float delta, float maxHealth, GameObject instigator);

    public event OnHealthChanged onHealthChanged;
    public event OnHealthEmpty onHealthEmpty;
    public event OnTakenDamage onTakenDamage;

    [SerializeField] float currentHealth;
    [SerializeField] float maxHealth;

    public void ChangeHealth(float amount, GameObject target)
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
            onTakenDamage?.Invoke(currentHealth, amount, maxHealth, target);
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
