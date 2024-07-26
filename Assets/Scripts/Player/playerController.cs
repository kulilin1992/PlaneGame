using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{
    [SerializeField] PlayerInput playerInput;
    // Start is called before the first frame update

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
        throw new NotImplementedException();
    }

    private void OnMove(Vector2 arg0)
    {
        throw new NotImplementedException();
    }
}
