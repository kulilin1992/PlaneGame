using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPowerBullet : PlayerBullet
{
    [SerializeField] BulletSystem bulletSystem;
    protected override void OnEnable()
    {
        SetTarget(EnemyManager.Instance.RandomEnemy);
        transform.rotation = Quaternion.identity;
        if (target == null)
        {
            base.OnEnable();
        }
        else
        {
            //追踪敌人
            StartCoroutine(bulletSystem.TrackCoroutine(target));
        }
        //base.OnEnable();
    }
}
