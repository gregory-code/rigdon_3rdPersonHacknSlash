using Cinemachine.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMovement
{
    [SerializeField] float gravity = -20f;

    GameObject _owner;
    Animator _ownerAnimator;
    CharacterController _characterController;
    Vector3 _playerVelocity;
    float _rotationSpeed;

    public float Speed;
    private float _walkSpeed;
    private float _runSpeed;

    private float _animationTransitionSpeed;

    private bool _bIsSprinting;

    private bool _bInLock;
    private Transform _targetTransform;

    public playerMovement(GameObject myOwner)
    {
        _owner = myOwner;
        _ownerAnimator = _owner.GetComponent<Animator>();
        _characterController = _owner.AddComponent<CharacterController>();
        _characterController.center = new Vector3(0, 1, 0);
        _rotationSpeed = 10;
        _walkSpeed = 3;
        _runSpeed = 6;
        _animationTransitionSpeed = 5;
    }

    public void MoveCharacter(Vector3 velocity, Vector3 inputDirection, Vector3 movementDir)
    {
        if(_bInLock)
        {
            Quaternion targetLock = Quaternion.LookRotation(_targetTransform.position - _owner.transform.position);
            targetLock.x = 0;
            targetLock.z = 0;
            _owner.transform.rotation = Quaternion.Slerp(_owner.transform.rotation, targetLock, _rotationSpeed * Time.deltaTime);
        }

        // Checks if there is current movement
        if (inputDirection == Vector3.zero)
        {
            SpeedChange(0);
            return;
        }

        SpeedChange(GetDesiredSpeed());

        if (_bInLock)
        {
            Quaternion targetRotation = Quaternion.LookRotation(_targetTransform.forward);
            targetRotation.y = 0;
            targetRotation = targetRotation.normalized;
            Vector3 relativeDirection = targetRotation * inputDirection;

            UpdateAnimator(_owner.transform.TransformDirection(relativeDirection));

            _characterController.Move(_owner.transform.TransformDirection(relativeDirection) * Speed * Time.deltaTime);
        }
        else
        {
            UpdateAnimator(_owner.transform.forward);

            _characterController.Move(_owner.transform.forward * Speed * Time.deltaTime);

            // Handles Rotating in the direction of movement
            movementDir.y = 0;
            movementDir = movementDir.normalized;
            Vector3 relativeDirection = Quaternion.LookRotation(movementDir) * inputDirection;
            Quaternion targetRotation = Quaternion.LookRotation(relativeDirection);
            _owner.transform.rotation = Quaternion.Slerp(_owner.transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
        }
    }

    private void SpeedChange(float desiredSpeed)
    {
        Speed = Mathf.Lerp(Speed, desiredSpeed, _animationTransitionSpeed * Time.deltaTime);
        _ownerAnimator.SetFloat("speed", Speed);
    }

    private float GetDesiredSpeed()
    {
        float desiredSpeed = (_bIsSprinting) ? _runSpeed : _walkSpeed;
        return desiredSpeed;
    }

    public void SetTarget(bool state, Transform target)
    {
        _bInLock = state;
        if (state) _targetTransform = target;
    }

    private void UpdateAnimator(Vector3 moveDir)
    {
        float rightSpeed = Vector3.Dot(moveDir, _owner.transform.right);
        float forwardSpeed = Vector3.Dot(moveDir, _owner.transform.forward);

        float lerpRight = Mathf.Lerp(_ownerAnimator.GetFloat("leftSpeed"), rightSpeed, 4 * Time.deltaTime);
        float lerpFoward = Mathf.Lerp(_ownerAnimator.GetFloat("fowardSpeed"), forwardSpeed, 4 * Time.deltaTime);

        _ownerAnimator.SetFloat("leftSpeed", lerpRight);
        _ownerAnimator.SetFloat("fowardSpeed", lerpFoward);
    }

    public void MoveToGravity()
    {
        _playerVelocity.y += gravity * Time.deltaTime;

        if (_characterController.isGrounded && _playerVelocity.y < 0)
        {
            _playerVelocity.y = -2f;
        }

        _characterController.Move(_playerVelocity * Time.deltaTime);
    }

    public void SprintToggle()
    {
        _bIsSprinting = !_bIsSprinting;
    }
}
