using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] GameObject deathVFX;

    [SerializeField] AudioData[] deathAudio;
    [SerializeField] protected float maxHealth;
    [SerializeField] StatsBar headHealthBar;
    [SerializeField] bool showHeadHealthBar = true;

    protected float curHealth;
    

    protected virtual void OnEnable()
    {
        curHealth = maxHealth;
        if (showHeadHealthBar)
        {
            ShowHeadHealthBar();
        }
        else
        {
            HideHeadHealthBar();
        }
    }

    public virtual void TakeDamage(float damage)
    {
        if (curHealth <= 0)
            return;
        curHealth -= damage;
        if (showHeadHealthBar)
        {
            headHealthBar.UpdateStats(curHealth, maxHealth);
        }
        if (curHealth <= 0) 
            Die();
    }

    public virtual void Die()
    {
        curHealth = 0f;
        AudioManager.Instance.PlayRandomSFX(deathAudio);
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
        if (showHeadHealthBar)
        {
            headHealthBar.UpdateStats(curHealth, maxHealth);
        }
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

    public void ShowHeadHealthBar()
    {
        headHealthBar.gameObject.SetActive(true);
        headHealthBar.Initialize(curHealth, maxHealth);
    }
    public void HideHeadHealthBar()
    {
        headHealthBar.gameObject.SetActive(false);
    }
}
