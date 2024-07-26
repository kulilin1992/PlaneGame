using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : Bullet
{
    TrailRenderer trail;
    void Awake()
    {
        trail = GetComponentInChildren<TrailRenderer>();
    }

    void OnDisable()
    {
        trail.Clear();
    }
}
