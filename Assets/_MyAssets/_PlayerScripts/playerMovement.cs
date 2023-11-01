using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMovement
{
    [SerializeField] float gravity = -20f;

    GameObject _owner;
    CharacterController _characterController;
    Vector3 _playerVelocity;

    float _rotationSpeed;

    public playerMovement(GameObject myOwner)
    {
        _owner = myOwner;
        _characterController = _owner.AddComponent<CharacterController>();
        _characterController.center = new Vector3(0, 1, 0);
        _rotationSpeed = 10;
    }

    public void MoveCharacter(Vector3 velocity, Vector3 inputDirection, Vector3 movementDir)
    {
        // Checks if there is current movement
        if (inputDirection == Vector3.zero) return;

        movementDir.y = 0;
        movementDir = movementDir.normalized;

        // Handles Moving to the direction of the camera
        _characterController.Move(_owner.transform.forward * 3 * Time.deltaTime); // 3 is current speed

        // Handles Rotating in the direction of movement
        Vector3 relativeDirection = Quaternion.LookRotation(movementDir) * inputDirection;
        Quaternion targetRotation = Quaternion.LookRotation(relativeDirection);
        _owner.transform.rotation = Quaternion.Slerp(_owner.transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
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
}
