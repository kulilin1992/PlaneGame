using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEnergy : Singleton<PlayerEnergy>
{
    [SerializeField] EnergyBarHud energyBar;
    [SerializeField] float powerDriveInterval = 0.1f;
    public const int MAX = 100;
    public const int PERCENT = 1;

    int energy;

    bool canGetEnergy = true;

    WaitForSeconds waitForPowerDriveInterval;

    protected override void Awake()
    {
        base.Awake();
        waitForPowerDriveInterval = new WaitForSeconds(powerDriveInterval);
    }

    void OnEnable()
    {
        PlayerPowerDrive.on += PlayerPowerDriveOn;
        PlayerPowerDrive.off += PlayerPowerDriveOff;
    }

    void OnDisable()
    {
        PlayerPowerDrive.on -= PlayerPowerDriveOn;
        PlayerPowerDrive.off -= PlayerPowerDriveOff;
    }
    void Start()
    {
        energyBar.Initialize(energy, MAX);
        //SetEnergy(MAX);
    }

    public void SetEnergy(int value)
    {
        if (energy == MAX || !canGetEnergy || !gameObject.activeSelf)
            return;
        energy = Mathf.Clamp(energy + value, 0, MAX);
        energyBar.UpdateStats(energy, MAX);
    }

    public void UseEnergy(int value)
    {
        energy -= value;
        energyBar.UpdateStats(energy, MAX);

        if (energy == 0 && !canGetEnergy)
        {
            PlayerPowerDrive.off.Invoke();
        }
    }

    // public bool IsEnoughEnergy(int value)
    // {
    //     return energy >= value;
    // }

    public bool IsEnoughEnergy(int value) => energy >= value;


    private void PlayerPowerDriveOff()
    {
        canGetEnergy = true;
        StopCoroutine(nameof(KeepLostingEnergyCorontine));
    }

    private void PlayerPowerDriveOn()
    {
        canGetEnergy = false;
        StartCoroutine(nameof(KeepLostingEnergyCorontine));
    }

    IEnumerator KeepLostingEnergyCorontine()
    {
        while (gameObject.activeSelf && energy > 0)
        {
            yield return waitForPowerDriveInterval;
            UseEnergy(PERCENT);
        }
    }
}
