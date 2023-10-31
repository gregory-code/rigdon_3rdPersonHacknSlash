using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMovement
{
    [SerializeField] float gravity = -20f;

    GameObject _owner;
    CharacterController _characterController;
    Vector3 playerVelocity;

    public playerMovement(GameObject myOwner)
    {
        _owner = myOwner;
        _characterController = _owner.AddComponent<CharacterController>();
        _characterController.center = new Vector3(0, 1, 0);
    }

    public void MoveCharacter(Vector3 velocity, Vector3 movementDir)
    {
        _characterController.Move(_owner.transform.TransformDirection(movementDir) * 3 * Time.deltaTime); // 3 is current speed
    }

    public void MoveToGravity()
    {
        playerVelocity.y += gravity * Time.deltaTime;

        if (_characterController.isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = -2f;
        }

        _characterController.Move(playerVelocity * Time.deltaTime);
    }

}
