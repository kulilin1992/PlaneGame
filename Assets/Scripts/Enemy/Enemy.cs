using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    [SerializeField] int getDeathEmenyEnergy = 3;
    public override void Die()
    {
        PlayerEnergy.Instance.SetEnergy(getDeathEmenyEnergy);
        base.Die();
    }
}
