using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootSpawner : MonoBehaviour
{
    [SerializeField] LootSetting[] lootSettings;

    public void SpawnCreate(Vector2 position)
    {
        foreach (var item in lootSettings)
        {
            item.SpawnLoot(position + Random.insideUnitCircle);
        }
    }
}
