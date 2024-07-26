using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletAiming : Bullet
{
    void Awake()
    {
        target = GameObject.FindGameObjectWithTag("Player");
    }

    protected override void OnEnable()
    {
        StartCoroutine(nameof(MoveDirectionCoroutine));
        base.OnEnable();
    }

    IEnumerator MoveDirectionCoroutine()
    {
        yield return null;

        if (target.activeSelf)
        {
            direction = (target.transform.position - transform.position).normalized;
        }
    }
}
