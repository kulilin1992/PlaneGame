using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBeam : MonoBehaviour
{
    [SerializeField] float damage = 50f;
    [SerializeField] GameObject hitVFX;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<Player>(out Player player))
        {
            player.TakeDamage(damage);

            // var contactPoint = collision.GetContact(0);
            // PoolManager.Release(hitVFX, contactPoint.point, Quaternion.LookRotation(contactPoint.normal));

            PoolManager.Release(hitVFX, collision.GetContact(0).point, Quaternion.LookRotation(collision.GetContact(0).normal));
        }   
    }


}
