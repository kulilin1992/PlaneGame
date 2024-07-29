using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    [SerializeField] int getDeathEmenyEnergy = 3;

    [SerializeField] int scorePoint = 100;

    [SerializeField] protected int healthFactor;

    protected override void OnEnable()
    {
        SetHealth();
        base.OnEnable();
    }
    public override void Die()
    {
        ScoreManager.Instance.AddScore(scorePoint);
        PlayerEnergy.Instance.SetEnergy(getDeathEmenyEnergy);
        EnemyManager.Instance.RemoveEnemy(gameObject);
        base.Die();
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<Player>(out Player player))
        {
            player.Die();
            Die();
        }
    }

    protected virtual void SetHealth()
    {
        maxHealth += (int)(EnemyManager.Instance.WaveNumber / healthFactor);
    }
}
