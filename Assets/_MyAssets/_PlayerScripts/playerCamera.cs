using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;

[ExecuteAlways]
public class playerCamera
{
    GameObject _owner;
    Transform _followTransform;
    Camera _playerCam;

    float _clampMax;
    float _clampMin;
    float _horizontalRotSpeed;
    float _verticalRotSpeed;

    public playerCamera(GameObject myOwner)
    {
        _owner = myOwner;
        _followTransform = GameObject.Find("followTransform").GetComponent<Transform>();
        _playerCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        _horizontalRotSpeed = 1;
        _verticalRotSpeed = 1;
        _clampMin = -30;
        _clampMax = 40;
    }

    public void HandleRotation(Vector2 look)
    {
        Vector3 followPlayer = _owner.transform.position;
        followPlayer.y = 1.7f;
        _followTransform.position = followPlayer;

        Quaternion followLerp = _followTransform.transform.rotation;
        followLerp *= Quaternion.AngleAxis(look.x * _horizontalRotSpeed, Vector3.up);
        followLerp *= Quaternion.AngleAxis(-look.y * _verticalRotSpeed, Vector3.right);

        _followTransform.transform.rotation = Quaternion.Slerp(_followTransform.transform.rotation, followLerp, 3 * Time.deltaTime);
        
        Vector3 clampedAngle = _followTransform.transform.eulerAngles;

        if (clampedAngle.x > 180f) clampedAngle.x -= 360f;
        clampedAngle.x = Mathf.Clamp(clampedAngle.x, _clampMin, _clampMax);

        _followTransform.transform.rotation = Quaternion.Euler(clampedAngle);
    }

    public Vector3 GetCameraFoward()
    {
        return _playerCam.transform.forward;
    }
}
