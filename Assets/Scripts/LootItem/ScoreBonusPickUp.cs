using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreBonusPickUp : LootItem
{
    [SerializeField] int ScoreBonus;

    protected override void PickUp()
    {
        ScoreManager.Instance.AddScore(ScoreBonus);
        base.PickUp();
    }
}
