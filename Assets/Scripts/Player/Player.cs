using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : Character
{
    [SerializeField] StatsBarHud statsBarHud;
    [SerializeField] PlayerInput playerInput;
    // Start is called before the first frame update
    [SerializeField] float moveSpeed = 7f;
    // [SerializeField] float paddingX = 0.8f;
    // [SerializeField] float paddingY = 0.2f;

    //加速
    [SerializeField] float acceleationTime = 0.2f;
    //减速
    [SerializeField] float decelerationTime = 0.2f;

    [SerializeField] float moveRotationAngle = 27f;

    private Coroutine coroutine;
    Coroutine healthGenerateCoroutine;
    new Rigidbody2D rigidbody;

    [Header("---------Fire---------")]
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] GameObject bulletDoublePrefab;
    [SerializeField] GameObject bulletTriPrefab;

    [SerializeField] GameObject bulletPowerDrivePrefab;

    [SerializeField, Range(0, 2)] int weaponLevel = 0;
    [SerializeField] Transform attackPoint;
    [SerializeField] Transform attackTopPoint;
    [SerializeField] Transform attackBottomPoint;

    [SerializeField] float attackInterval = 0.1f;

    [SerializeField] AudioData bulletLaunchSFX;
    WaitForSeconds waitForSeconds;

    [SerializeField] bool regenerateHealth = true;
    [SerializeField] float regenerateHealthTime;
    [SerializeField, Range(0f, 1f)] float regenerateHealthPercent = 0.1f;


    [Header("---------Dodge---------")]
    [SerializeField, Range(0, 100)] int dodgeEnergyCost = 25;
    [SerializeField] float maxRoll = 360f;
    [SerializeField] float rollSpeed = 360f;

    [SerializeField] AudioData dodgeSFX;

    [SerializeField] Vector3 dodgeScale = new Vector3(0.5f, 0.5f, 0.5f);


    [Header("---------PowerDirve---------")]
    [SerializeField] int powerDriveDodgeFactor = 2;
    [SerializeField] float powerDriveSpeedFactor = 2f;
    [SerializeField] float powerDriveFireFactor = 2f;


    float currentRoll;

    bool isDodging = false;

    float dodgeDuartion;

    bool isPowerDrive = false;

    float paddingX;
    float paddingY;

    WaitForSeconds waitHealthRegenerateTime;
    //WaitForSeconds waitForFireInterval;
    WaitForSeconds waitForPowerDriveFireInterval;

    WaitForSeconds waitDecelerationTime;
    new Collider2D collider2D;

    Vector2 previousVelocity;
    Quaternion previousRotation;
    WaitForFixedUpdate waitForFixedUpdate = new WaitForFixedUpdate();
    void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        collider2D = GetComponent<Collider2D>();
        dodgeDuartion = maxRoll / rollSpeed;

        var size = transform.GetChild(0).GetComponent<Renderer>().bounds.size;
        paddingX = size.x / 2f;
        paddingY = size.y / 2f;

        rigidbody.gravityScale = 0f;
        waitForSeconds = new WaitForSeconds(attackInterval);
        waitForPowerDriveFireInterval = new WaitForSeconds(attackInterval / powerDriveFireFactor);
        waitHealthRegenerateTime = new WaitForSeconds(regenerateHealthTime);
        waitDecelerationTime = new WaitForSeconds(decelerationTime);
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        playerInput.onMove += OnMove;
        playerInput.onStopMove += StopMove;
        playerInput.onAttack += OnAttack;
        playerInput.onStopAttack += OnStopAttack;
        playerInput.onDodge += OnDodge;
        playerInput.onPowerDrive += OnPowerDrive;

        PlayerPowerDrive.on += PowerDriveOn;
        PlayerPowerDrive.off += PowerDriveOff;
    }

    void OnDisable()
    {
        playerInput.onMove -= OnMove;
        playerInput.onStopMove -= StopMove;
        playerInput.onAttack -= OnAttack;
        playerInput.onStopAttack -= OnStopAttack;
        playerInput.onDodge -= OnDodge;
        playerInput.onPowerDrive -= OnPowerDrive;
        PlayerPowerDrive.on -= PowerDriveOn;
        PlayerPowerDrive.off -= PowerDriveOff;
    }

    private void OnMove(Vector2 moveInput)
    {
        if (coroutine != null) {
            StopCoroutine(coroutine);
        }
        //Vector2 moveAmount = moveInput * moveSpeed;
        //rigidbody.velocity = moveInput * moveSpeed;

        Quaternion rotation = Quaternion.AngleAxis(moveRotationAngle * moveInput.y, Vector3.right);
        coroutine = StartCoroutine(MoveCoroutine(acceleationTime, moveInput.normalized * moveSpeed, rotation));
        StopCoroutine(nameof(DecelerationCoroutine));
        StartCoroutine(nameof(MovePositionLimitCoroutine));
    }

     private void StopMove()
    {
        if (coroutine != null) {
            StopCoroutine(coroutine);
        }
        //rigidbody.velocity = Vector2.zero;
        coroutine = StartCoroutine(MoveCoroutine(decelerationTime, Vector2.zero, Quaternion.identity));
        //StopCoroutine(nameof(MovePositionLimitCoroutine));
        StartCoroutine(nameof(DecelerationCoroutine));
    }

    void Start()
    {
        // rigidbody.gravityScale = 0f;
        playerInput.EnablePlayerInput();
        // waitForSeconds = new WaitForSeconds(attackInterval);
        // waitHealthRegenerateTime = new WaitForSeconds(regenerateHealthTime);

        statsBarHud.Initialize(curHealth, maxHealth);
        //StartCoroutine(nameof(MovePositionLimitCoroutine));
        //TakeDamage(50f);
    }

    // Update is called once per frame
    void Update()
    {
        //transform.position = Viewport.Instance.PlayerLimitPosition(transform.position);
    }

    IEnumerator MovePositionLimitCoroutine()
    {
        while (true)
        {
            transform.position = Viewport.Instance.PlayerLimitPosition(transform.position, paddingX, paddingY);
            //yield return new WaitForSeconds(0.1f);
            yield return null;
        }
    }

    // IEnumerator StartMoveCoroutine(Vector2 moveAmount)
    // {
    //     float time = 0f;
    //     while (time < acceleationTime)
    //     {
    //         time += Time.fixedDeltaTime / acceleationTime;
    //         rigidbody.velocity = Vector2.Lerp(Vector2.zero, moveAmount, time / acceleationTime);
    //         yield return null;
    //     }
    // }
    // IEnumerator StopMoveCoroutine(Vector2 moveAmount)
    // {
    //     float time = 0f;
    //     while (time < decelerationTime)
    //     {
    //         time += Time.fixedDeltaTime / decelerationTime;
    //         rigidbody.velocity = Vector2.Lerp(Vector2.zero, moveAmount, time / decelerationTime);
    //         yield return null;
    //     }
    // }
    IEnumerator MoveCoroutine(float movetime, Vector2 moveAmount, Quaternion moveRotation)
    {
        float time = 0f;
        // while (time < movetime)
        // {
        //     time += Time.fixedDeltaTime / movetime;
        //     rigidbody.velocity = Vector2.Lerp(Vector2.zero, moveAmount, time / movetime);

        //     //添加飞机旋转
        //     transform.rotation = Quaternion.Lerp(transform.rotation, moveRotation, time / movetime);
        //     yield return null;
        // }
        previousVelocity = rigidbody.velocity;
        previousRotation = transform.rotation;
        while (time < 1f)
        {
            time += Time.fixedDeltaTime / movetime;
            rigidbody.velocity = Vector2.Lerp(previousVelocity, moveAmount, time);

            //添加飞机旋转
            transform.rotation = Quaternion.Lerp(previousRotation, moveRotation, time);
            yield return waitForFixedUpdate;
        }
    }

    void OnAttack()
    {
        //StartCoroutine(AttackCoroutine());
        //StartCoroutine("AttackCoroutine");
        StartCoroutine(nameof(AttackCoroutine));
        //Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
    }

    void OnStopAttack()
    {
        StopCoroutine(nameof(AttackCoroutine));
    }

    IEnumerator AttackCoroutine()
    {
        while (true)
        {
            // switch (weaponLevel)
            // {
            //     case 0:
            //         Instantiate(bulletPrefab, attackPoint.position, Quaternion.identity);
            //         break;
            //     case 1:
            //         Instantiate(bulletPrefab, attackTopPoint.position, Quaternion.identity);
            //         Instantiate(bulletPrefab, attackBottomPoint.position, Quaternion.identity);
            //         break;
            //     case 2:
            //         Instantiate(bulletPrefab, attackPoint.position, Quaternion.identity);
            //         Instantiate(bulletDoublePrefab, attackTopPoint.position, Quaternion.identity);
            //         Instantiate(bulletTriPrefab, attackBottomPoint.position, Quaternion.identity);
            //         break;
            //     default:
            //         break;
            // }
            switch (weaponLevel)
            {
                case 0:
                    PoolManager.Release(isPowerDrive? bulletPowerDrivePrefab : bulletPrefab, attackPoint.position, Quaternion.identity);
                    break;
                case 1:
                    PoolManager.Release(isPowerDrive? bulletPowerDrivePrefab : bulletPrefab, attackTopPoint.position, Quaternion.identity);
                    PoolManager.Release(isPowerDrive? bulletPowerDrivePrefab : bulletPrefab, attackBottomPoint.position, Quaternion.identity);
                    break;
                case 2:
                    PoolManager.Release(isPowerDrive? bulletPowerDrivePrefab : bulletPrefab, attackPoint.position, Quaternion.identity);
                    PoolManager.Release(isPowerDrive? bulletPowerDrivePrefab : bulletDoublePrefab, attackTopPoint.position, Quaternion.identity);
                    PoolManager.Release(isPowerDrive? bulletPowerDrivePrefab : bulletTriPrefab, attackBottomPoint.position, Quaternion.identity);
                    break;
                default:
                    break;
            }
            //Instantiate(bulletPrefab, attackPoint.position, Quaternion.identity);
            //yield return new WaitForSeconds(attackInterval);   尽量避免循环中new对象
            AudioManager.Instance.PlayRandomSFX(bulletLaunchSFX);
            //yield return waitForSeconds;
            // if (isPowerDrive)
            // {
            //     yield return waitForPowerDriveFireInterval;
            // }
            // else
            // {
            //     yield return waitForSeconds;
            // }
            yield return isPowerDrive ? waitForPowerDriveFireInterval : waitForSeconds;
        }
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);

        statsBarHud.UpdateStats(curHealth, maxHealth);

        if (gameObject.activeSelf)
        {
            if (regenerateHealth)
            {
                if (healthGenerateCoroutine != null)
                {
                    StopCoroutine(healthGenerateCoroutine);
                }
                healthGenerateCoroutine = StartCoroutine(HealthRegenCoroutine(waitHealthRegenerateTime, regenerateHealthPercent));
            }
        }
    }

    public override void RestoreHealth(float amount)
    {
        base.RestoreHealth(amount);
        statsBarHud.UpdateStats(curHealth, maxHealth);
    }

    public override void Die()
    {
        statsBarHud.UpdateStats(0f, maxHealth);
        base.Die();
        //statsBarHud.gameObject.SetActive(false);
    }

    void OnDodge()
    {
        if (isDodging || !PlayerEnergy.Instance.IsEnoughEnergy(dodgeEnergyCost))
            return;
        StartCoroutine(nameof(DodgeCoroutine));
    }

    IEnumerator DodgeCoroutine()
    {
        isDodging = true;
        AudioManager.Instance.PlayRandomSFX(dodgeSFX);
        //消耗能量
        PlayerEnergy.Instance.UseEnergy(dodgeEnergyCost);

        //让玩家无敌
        collider2D.isTrigger = true;
        var scale = transform.localScale;
        currentRoll = 0f;
        while (currentRoll < maxRoll)
        {
            currentRoll += Time.deltaTime * rollSpeed;
            transform.rotation = Quaternion.AngleAxis(currentRoll, Vector3.right);

            if (currentRoll < maxRoll / 2)
            {
                //scale -= (Time.deltaTime / dodgeDuartion) * Vector3.one;

                scale.x = Mathf.Clamp(scale.x - Time.deltaTime / dodgeDuartion, dodgeScale.x, 1f);
                scale.y = Mathf.Clamp(scale.y - Time.deltaTime / dodgeDuartion, dodgeScale.y, 1f);
                scale.z = Mathf.Clamp(scale.z - Time.deltaTime / dodgeDuartion, dodgeScale.z, 1f);

            }
            else
            {
                //scale += (Time.deltaTime / dodgeDuartion) * Vector3.one;

                scale.x = Mathf.Clamp(scale.x + Time.deltaTime / dodgeDuartion, dodgeScale.x, 1f);
                scale.y = Mathf.Clamp(scale.y + Time.deltaTime / dodgeDuartion, dodgeScale.y, 1f);
                scale.z = Mathf.Clamp(scale.z + Time.deltaTime / dodgeDuartion, dodgeScale.z, 1f);
            }
            transform.localScale = scale;

            yield return null;
        }
        collider2D.isTrigger = false;
        isDodging = false;
    }

    IEnumerator DecelerationCoroutine()
    {
        yield return waitDecelerationTime;

        StopCoroutine(nameof(MovePositionLimitCoroutine));
    }

    #region  PowerDrive
    private void OnPowerDrive()
    {
        if (!PlayerEnergy.Instance.IsEnoughEnergy(PlayerEnergy.MAX)) {
            return;
        }

        PlayerPowerDrive.on.Invoke();
    }

    private void PowerDriveOff()
    {
        isPowerDrive = false;

        dodgeEnergyCost /= powerDriveDodgeFactor;
        moveSpeed /= powerDriveSpeedFactor;
    }

    private void PowerDriveOn()
    {
        isPowerDrive = true;
        dodgeEnergyCost *= powerDriveDodgeFactor;
        moveSpeed *= powerDriveSpeedFactor;
        TimeController.Instance.BulletTime(1f, 1f);
    }
    #endregion
}
