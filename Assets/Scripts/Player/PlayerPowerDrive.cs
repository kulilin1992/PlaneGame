using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerPowerDrive : MonoBehaviour
{
    public static UnityAction on = delegate { };
    public static UnityAction off = delegate { };

    [SerializeField] GameObject triggerVFX;
    [SerializeField] GameObject engineVFXNormal;
    [SerializeField] GameObject engineVFXPowerDrive;

    [SerializeField] AudioData onSFX;
    [SerializeField] AudioData offSFX;

    void Awake()
    {
        on += On;
        off += Off;
    }
    void OnDestroy()
    {
        on -= On;
        off -= Off;
    }

    void On()
    {
        triggerVFX.SetActive(true);
        engineVFXNormal.SetActive(false);
        engineVFXPowerDrive.SetActive(true);
        AudioManager.Instance.PlayRandomSFX(onSFX);
    }
    void Off()
    { 
        engineVFXNormal.SetActive(true);
        engineVFXPowerDrive.SetActive(false);
        AudioManager.Instance.PlayRandomSFX(offSFX); //TODO: Implement
    }
}
