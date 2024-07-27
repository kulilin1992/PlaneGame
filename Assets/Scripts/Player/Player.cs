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
    [SerializeField] float paddingX = 0.8f;
    [SerializeField] float paddingY = 0.2f;

    //加速
    [SerializeField] float acceleationTime = 0.2f;
    //减速
    [SerializeField] float decelerationTime = 0.2f;

    [SerializeField] float moveRotationAngle = 27f;

    private Coroutine coroutine;
    Coroutine healthGenerateCoroutine;
    new Rigidbody2D rigidbody;

    [SerializeField] GameObject bulletPrefab;
    [SerializeField] GameObject bulletDoublePrefab;
    [SerializeField] GameObject bulletTriPrefab;

    [SerializeField, Range(0, 2)] int weaponLevel = 0;
    [SerializeField] Transform attackPoint;
    [SerializeField] Transform attackTopPoint;
    [SerializeField] Transform attackBottomPoint;

    [SerializeField] float attackInterval = 0.1f;
    WaitForSeconds waitForSeconds;

    [SerializeField] bool regenerateHealth = true;
    [SerializeField] float regenerateHealthTime;
    [SerializeField, Range(0f, 1f)] float regenerateHealthPercent = 0.1f;


    [Header("---------Dodge---------")]
    [SerializeField, Range(0, 100)] int dodgeEnergyCost = 25;
    [SerializeField] float maxRoll = 720f;
    [SerializeField] float rollSpeed = 360f;

    [SerializeField] Vector3 dodgeScale = new Vector3(0.5f, 0.5f, 0.5f);

    float currentRoll;

    bool isDodging = false;

    float dodgeDuartion;

    WaitForSeconds waitHealthRegenerateTime;
    
    new Collider2D collider2D;
    void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        collider2D = GetComponent<Collider2D>();
        dodgeDuartion = maxRoll / rollSpeed;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        playerInput.onMove += OnMove;
        playerInput.onStopMove += StopMove;
        playerInput.onAttack += OnAttack;
        playerInput.onStopAttack += OnStopAttack;
        playerInput.onDodge += OnDodge;
    }


    void OnDisable()
    {
        playerInput.onMove -= OnMove;
        playerInput.onStopMove -= StopMove;
        playerInput.onAttack -= OnAttack;
        playerInput.onStopAttack -= OnStopAttack;
        playerInput.onDodge -= OnDodge;
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
        StartCoroutine(MovePositionLimitCoroutine());
    }

     private void StopMove()
    {
        if (coroutine != null) {
            StopCoroutine(coroutine);
        }
        //rigidbody.velocity = Vector2.zero;
        coroutine = StartCoroutine(MoveCoroutine(decelerationTime, Vector2.zero, Quaternion.identity));
        StopCoroutine(MovePositionLimitCoroutine());
    }

    void Start()
    {
        rigidbody.gravityScale = 0f;
        playerInput.EnablePlayerInput();
        waitForSeconds = new WaitForSeconds(attackInterval);
        waitHealthRegenerateTime = new WaitForSeconds(regenerateHealthTime);

        statsBarHud.Initialize(curHealth, maxHealth);

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
        while (time < 1f)
        {
            time += Time.fixedDeltaTime / movetime;
            rigidbody.velocity = Vector2.Lerp(Vector2.zero, moveAmount, time);

            //添加飞机旋转
            transform.rotation = Quaternion.Lerp(transform.rotation, moveRotation, time);
            yield return null;
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
                    PoolManager.Release(bulletPrefab, attackPoint.position, Quaternion.identity);
                    break;
                case 1:
                    PoolManager.Release(bulletPrefab, attackTopPoint.position, Quaternion.identity);
                    PoolManager.Release(bulletPrefab, attackBottomPoint.position, Quaternion.identity);
                    break;
                case 2:
                    PoolManager.Release(bulletPrefab, attackPoint.position, Quaternion.identity);
                    PoolManager.Release(bulletDoublePrefab, attackTopPoint.position, Quaternion.identity);
                    PoolManager.Release(bulletTriPrefab, attackBottomPoint.position, Quaternion.identity);
                    break;
                default:
                    break;
            }
            //Instantiate(bulletPrefab, attackPoint.position, Quaternion.identity);
            //yield return new WaitForSeconds(attackInterval);   尽量避免循环中new对象
            yield return waitForSeconds;
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
}
