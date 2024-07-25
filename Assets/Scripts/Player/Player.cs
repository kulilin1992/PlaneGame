using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    [SerializeField] PlayerInput playerInput;
    // Start is called before the first frame update
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float paddingX = 0.8f;
    [SerializeField] float paddingY = 0.2f;
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

    private void StopMove()
    {
        rigidbody.velocity = Vector2.zero;
        StopCoroutine(MovePositionLimitCoroutine());
    }

    private void OnMove(Vector2 moveInput)
    {
        //Vector2 moveAmount = moveInput * moveSpeed;
        rigidbody.velocity = moveInput * moveSpeed;
        StartCoroutine(MovePositionLimitCoroutine());
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
}
