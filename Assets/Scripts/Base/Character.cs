using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] GameObject deathVFX;
    [SerializeField] float maxHealth;

    protected float curHealth;

    protected virtual void OnEnable()
    {
        curHealth = maxHealth;
    }

    public virtual void TakeDamage(float damage)
    {
        curHealth -= damage;
        if (curHealth <= 0) 
            Die();
    }

    public virtual void Die()
    {
        curHealth = 0f;
        PoolManager.Release(deathVFX, transform.position);
        gameObject.SetActive(false); //TODO: Add pooling
    }

    public virtual void RestoreHealth(float amount)
    {
        if (curHealth == maxHealth)
            return;
        
        // curHealth += amount;
        // curHealth = Mathf.Clamp(curHealth, 0, maxHealth);
        curHealth = Mathf.Clamp(curHealth + amount, 0, maxHealth);
    }

    protected IEnumerator HealthRegenCoroutine(WaitForSeconds waitTime, float percent)
    {

        while (curHealth < maxHealth)
        {
            yield return waitTime;
            RestoreHealth(maxHealth * percent);
        }
    }

    protected IEnumerator DamageOverTimeCoroutine(WaitForSeconds waitTime, float percent)
    {

        while (curHealth > 0)
        {
            yield return waitTime;
            TakeDamage(maxHealth * percent);
        }
    }
}
