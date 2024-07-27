using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEnergy : Singleton<PlayerEnergy>
{
    [SerializeField] EnergyBarHud energyBar;
    public const int MAX = 100;
    public const int PERCENT = 1;

    int energy;
    void Start()
    {
        energyBar.Initialize(energy, MAX);
        //SetEnergy(MAX);
    }

    public void SetEnergy(int value)
    {
        if (energy == MAX)
            return;
        energy = Mathf.Clamp(energy + value, 0, MAX);
        energyBar.UpdateStats(energy, MAX);
    }

    public void UseEnergy(int value)
    {
        energy -= value;
        energyBar.UpdateStats(energy, MAX);
    }

    // public bool IsEnoughEnergy(int value)
    // {
    //     return energy >= value;
    // }

    public bool IsEnoughEnergy(int value) => energy >= value;
}
