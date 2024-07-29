using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : EnemyController
{
    [SerializeField] float continueFireDuration = 1.5f;

    [Header("======PLAYER DETECTION======")]

    [SerializeField] Transform playerDetection;
    [SerializeField] Vector3 playerDetectionSize;
    [SerializeField] LayerMask playerLayer;

    [Header("======BEAM======")] // 激光
    [SerializeField] float beamCoolDownTime = 12f;
    [SerializeField] AudioData launchBeamSFX;
    [SerializeField] AudioData readyBeamSFX;

    bool isBeamReady;
    WaitForSeconds waitForContinueFireInterval;
    WaitForSeconds waitForFireInterval;

    WaitForSeconds waitBeamCoolDownTime;

    List<GameObject> magazine; //弹夹

    AudioData launchSFX;
    Animator animator;

    Transform playerTransform;

    int launchBeamID = Animator.StringToHash("launchBeam");

    protected override void Awake()
    {
        base.Awake();

        animator = GetComponent<Animator>();
        waitForContinueFireInterval = new WaitForSeconds(minAttackInterval);
        waitForFireInterval = new WaitForSeconds(maxAttackInterval);
        waitBeamCoolDownTime = new WaitForSeconds(beamCoolDownTime);

        magazine = new List<GameObject>(enemyBullets.Length);

        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    protected override void OnEnable()
    {
        isBeamReady = false;
        StartCoroutine(nameof(BeamCoolDownCoroutine));
        base.OnEnable();
    }

    protected override IEnumerator EnemyAttackCoroutine()
    {
        while (isActiveAndEnabled)
        {
            if (GameManager.GameState == GameState.GameOver) yield break;
            if (isBeamReady)
            {
                BeamAttack();

                StartCoroutine(nameof(ChasingPlayerCoroutine));
                yield break;
            }
            yield return waitForFireInterval;
            yield return StartCoroutine(nameof(ContinueFireCoroutine));
        }
    }

    IEnumerator ContinueFireCoroutine()
    {
        LoadBullets();
        float continueFireTimer = 0f;
        while (continueFireTimer < continueFireDuration)
        {
            foreach(var enemyBullet in magazine)
            {
                PoolManager.Release(enemyBullet, enemyFirePoint.position);
            }
            continueFireTimer += minAttackInterval;
            AudioManager.Instance.PlayRandomSFX(launchSFX);
            yield return waitForContinueFireInterval;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(playerDetection.position, playerDetectionSize);
    }

    void LoadBullets()
    {
        magazine.Clear();
        //如果玩家在boss正前面时
        if (Physics2D.OverlapBox(playerDetection.position, playerDetectionSize, 0, playerLayer))
        {
            magazine.Add(enemyBullets[0]);
            launchSFX = bulletLaunchSFX[0];
        }
        else
        {
            if (Random.value < 0.5f)
            {
                magazine.Add(enemyBullets[1]);
                launchSFX = bulletLaunchSFX[1];
            }
            else
            {
                for (int i = 2; i < enemyBullets.Length; i++)
                {
                    magazine.Add(enemyBullets[i]);
                }
                launchSFX = bulletLaunchSFX[2];

            }
        }
    }

    void BeamAttack()
    {
        isBeamReady = false;
        animator.SetTrigger(launchBeamID);
        AudioManager.Instance.PlayRandomSFX(readyBeamSFX);
    }

    //animation event
    void AE_BeamAttackAudio()
    {
        AudioManager.Instance.PlayRandomSFX(launchBeamSFX);
    }

    void AE_BeamAttackStop()
    {
        StopCoroutine(nameof(ChasingPlayerCoroutine));
        StartCoroutine(nameof(BeamCoolDownCoroutine));
        StartCoroutine(nameof(EnemyAttackCoroutine));
    }

    IEnumerator BeamCoolDownCoroutine()
    {
        yield return waitBeamCoolDownTime;

        isBeamReady = true;
    }
    IEnumerator ChasingPlayerCoroutine()
    {
        while (isActiveAndEnabled)
        {
            targetPosition.x = Viewport.Instance.MaxX - paddingX;
            targetPosition.y = playerTransform.position.y;
            yield return null;
        }
    }
}
