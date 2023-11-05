using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(Animator))]
public class playerScript : MonoBehaviour
{
    [SerializeField] PlayerInputActions _playerInput;
    [SerializeField] playerMovement _movement;
    [SerializeField] playerCamera _camera;
    [SerializeField] playerSight _sight;
    [SerializeField] playerActions _actions;
    [SerializeField] playerLock _lock;
    [SerializeField] playerLock _lockPrefab;

    public delegate void OnTargetLockUpdated(bool state, Transform target);
    public event OnTargetLockUpdated onTargetLockUpdated;

    Transform _closestEnemy;
    bool _bInLock;

    void Start()
    {
        _playerInput = new PlayerInputActions();
        _movement = new playerMovement(gameObject);
        _camera = new playerCamera(gameObject);
        _sight = Camera.main.GetComponent<playerSight>();
        _actions = new playerActions(gameObject);

        if (FindObjectOfType<Canvas>() == null)
        {
            Debug.Log("No Canvas");
            return;
        }
        _lock = Instantiate(_lockPrefab, FindObjectOfType<Canvas>().transform);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        _playerInput.PlayerSword.Enable();
    }

    void Update()
    {
        if (_movement == null) return;
        if (_camera == null) return;
        if (_sight == null) return;
        if (_actions == null) return;


        Look();
        Movement();
        UpdateWeight();


        if(_bInLock == false)
        {
            _closestEnemy = _sight.GetClosestEnemy();
        }

        if(_closestEnemy == null)
        {
            _lock.RemoveAttachment();
        }
        else
        {
            _lock.SetupAttachment(_closestEnemy);
        }
    }

    private void Look()
    {
        Vector2 lookVector = _playerInput.PlayerSword.Look.ReadValue<Vector2>();
        _camera.HandleRotation(lookVector);
        _camera.PushBackVirtualCam();
    }

    private void Movement()
    {
        Vector2 inputVector = _playerInput.PlayerSword.Movement.ReadValue<Vector2>();
        Vector3 movementDir = new Vector3(inputVector.x, 0, inputVector.y);
        _movement.MoveCharacter(inputVector, movementDir, _camera.GetCameraFoward());
        _movement.MoveToGravity();
    }

    public void Sprint(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            if (_movement == null) return;
            _movement.SprintToggle();
        }
    }

    public void RegularAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (_actions == null) return;
            _actions.RegularAttack();
        }
    }

    public void LockOn(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (_closestEnemy == null) return;


            _bInLock = !_bInLock;

            Transform target = (_bInLock) ? _closestEnemy : gameObject.transform.Find("lookAt").transform;

            onTargetLockUpdated?.Invoke(_bInLock, target);
        }
    }

    private void UpdateWeight()
    {
        _actions.UpdateAnimWeight();
    }

    private void GrabSword()
    {
        if (_actions == null) return;

        _actions.UpdateSword(false);
    }

    private void StoreSword()
    {
        if (_actions == null) return;

        _actions.UpdateSword(true);
    }
}
