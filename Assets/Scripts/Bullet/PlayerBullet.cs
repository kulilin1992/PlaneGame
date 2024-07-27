using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : Bullet
{
    TrailRenderer trail;
    void Awake()
    {
        trail = GetComponentInChildren<TrailRenderer>();
        if (direction != Vector2.right)
        {
            //transform.rotation = Quaternion.FromToRotation(Vector2.right, direction);
            transform.GetChild(0).rotation = Quaternion.FromToRotation(Vector2.right, direction);
        }
    }

    void OnDisable()
    {
        trail.Clear();
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);
        PlayerEnergy.Instance.SetEnergy(PlayerEnergy.PERCENT);
    }
}
