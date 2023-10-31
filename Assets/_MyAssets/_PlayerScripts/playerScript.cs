using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class playerScript : MonoBehaviour
{
    [SerializeField] PlayerInputActions _playerInput;
    [SerializeField] playerMovement _movement;
    [SerializeField] playerCamera _camera;

    void Start()
    {
        _playerInput = new PlayerInputActions();
        _movement = new playerMovement(gameObject);
        _camera = new playerCamera(gameObject);

        _playerInput.PlayerSword.Enable();
    }

    void Update()
    {
        Look();
        Movement();
    }

    private void Look()
    {
        if (_camera == null) return;
        Vector2 lookVector = _playerInput.PlayerSword.Look.ReadValue<Vector2>();
        _camera.HandleRotation(lookVector);
    }

    private void Movement()
    {
        if (_movement == null) return;
        Vector2 inputVector = _playerInput.PlayerSword.Movement.ReadValue<Vector2>();
        Vector3 movementDir = new Vector3(inputVector.x, 0, inputVector.y);
        _movement.MoveCharacter(inputVector, movementDir);
        _movement.MoveToGravity();
    }
}
