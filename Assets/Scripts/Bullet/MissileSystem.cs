using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileSystem : MonoBehaviour
{

    [SerializeField] int missileCount = 3;

    [SerializeField] float coolDownTime = 5f;
    [SerializeField] GameObject missilePrefab;

    [SerializeField] AudioData launchSound;

    int remainCount;

    bool isReady = true;

    void Awake()
    {
        remainCount = missileCount;
    }

    void Start()
    {
        MissileDisplay.UpdateMissileCount(remainCount);
    }
    public void Launch(Transform bulletTransform)
    {
        if (remainCount == 0 || !isReady)
            return;

        isReady = false;
        PoolManager.Release(missilePrefab, bulletTransform.position);

        AudioManager.Instance.PlayRandomSFX(launchSound);

        remainCount--;
        MissileDisplay.UpdateMissileCount(remainCount);

        if (remainCount == 0)
        {
            MissileDisplay.UpdateMissileCoolDown(1f);
        }
        else
        {
            StartCoroutine(nameof(CoolDownCoroutine));
        }
    }
    IEnumerator CoolDownCoroutine()
    {
        var coolDownValue = coolDownTime;
        while (coolDownValue > 0f)
        {
            MissileDisplay.UpdateMissileCoolDown(coolDownValue / coolDownTime);
            coolDownValue = Mathf.Max(coolDownValue - Time.deltaTime, 0f);
            //coolDownTime = Mathf.Clamp(coolDownTime, 0f, coolDownValue - Time.deltaTime);
            yield return null;
        }
        //yield return new WaitForSeconds(coolDownTime);
        isReady = true;
    }
}
