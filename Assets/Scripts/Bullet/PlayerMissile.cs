using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMissile : PlayerPowerBullet
{

    [SerializeField] AudioData missileSFX;
    [Header("======SPEED======")]
    [SerializeField] float lowSpeed = 8f;
    [SerializeField] float highSpeed = 26f;

    [SerializeField] float varibleSpeedDelay = 0.5f;

    [Header("======EXPLOSION======")]

    [SerializeField] GameObject explosionVFX;
    [SerializeField] AudioData explosionSFX;

    [SerializeField] float explosionRadius = 3f;

    [SerializeField] LayerMask enemyLayer;

    [SerializeField] float explosionDamage = 100f;


    WaitForSeconds waitVaribleSpeedDelay;

    protected override void Awake()
    {
        base.Awake();
        waitVaribleSpeedDelay = new WaitForSeconds(varibleSpeedDelay);
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        StartCoroutine(nameof(VaribleSpeedCoutinue));
    }


    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);

        PoolManager.Release(explosionVFX, transform.position);

        AudioManager.Instance.PlayRandomSFX(explosionSFX);

        //范围爆炸

        var colliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius, enemyLayer);
        foreach (var collider in colliders)
        { 
            //collider.GetComponent<Enemy>().TakeDamage(explosionDamage);
            if (collider.TryGetComponent<Enemy>(out Enemy enemy)) {
                enemy.TakeDamage(explosionDamage);
            }
        }

    }
    IEnumerator VaribleSpeedCoutinue()
    {
        speed = lowSpeed;

        yield return waitVaribleSpeedDelay;

        speed = highSpeed;

        if (target != null)
        {
            AudioManager.Instance.PlayRandomSFX(missileSFX);
        }
    }



    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }

    
}
