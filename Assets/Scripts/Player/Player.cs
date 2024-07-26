using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
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
    new Rigidbody2D rigidbody;

    void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    void OnEnable()
    {
        playerInput.onMove += OnMove;
        playerInput.onStopMove += StopMove;
    }

    void OnDisable()
    {
        playerInput.onMove -= OnMove;
        playerInput.onStopMove -= StopMove;
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
        while (time < movetime)
        {
            time += Time.fixedDeltaTime / movetime;
            rigidbody.velocity = Vector2.Lerp(Vector2.zero, moveAmount, time / movetime);

            //添加飞机旋转
            transform.rotation = Quaternion.Lerp(transform.rotation, moveRotation, time / movetime);
            yield return null;
        }
    }
}
