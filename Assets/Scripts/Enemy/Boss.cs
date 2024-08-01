using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Enemy
{

    BossHealthBar bossHealthBar;

    Canvas healthBarCanvas;
    bool flag;
    protected override void Awake()
    {
        base.Awake();
        bossHealthBar = FindObjectOfType<BossHealthBar>();
        healthBarCanvas = bossHealthBar.GetComponentInChildren<Canvas>();
        //Debug.Log("Boss Health Bar: " + healthBarCanvas);
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        bossHealthBar.Initialize(curHealth, maxHealth);
        // bossHealthBar.Initialize(curHealth, maxHealth);
        if (flag)
        {
            healthBarCanvas.enabled = true;
        }
        //healthBarCanvas.enabled = true; // Enable the health bar canvas when the boss is enabled
    }

    void Start() 
    {
        healthBarCanvas.enabled = true;
        flag = true;
    }
    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<Player>(out Player player))
        {
            player.Die();
        }
    }

    public override void Die()
    {
        healthBarCanvas.enabled = false; // Disable the health bar canvas when the boss dies
        base.Die();
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        bossHealthBar.UpdateStats(curHealth, maxHealth);
    }

    protected override void SetHealth()
    {
        maxHealth += EnemyManager.Instance.WaveNumber * healthFactor;
    }

}
