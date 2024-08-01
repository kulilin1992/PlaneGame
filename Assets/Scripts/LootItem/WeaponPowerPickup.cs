using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPowerPickup : LootItem
{
    [SerializeField] AudioData fullPowerPickUpSFX;
    [SerializeField] int fullPowerScoreBonus = 100;
    protected override void PickUp()
    {
        if (player.IsFullPower)
        {
            pickUpSFX = fullPowerPickUpSFX;
            lootMessage.text = $"SCORE + {fullPowerScoreBonus}";
            ScoreManager.Instance.AddScore(fullPowerScoreBonus);
        }
        else
        {
            pickUpSFX = defaultPickUpSFX;
            lootMessage.text = $"POWER UP";
            player.WeaponPowerLevelUp();
        }
        base.PickUp();
    }
}
