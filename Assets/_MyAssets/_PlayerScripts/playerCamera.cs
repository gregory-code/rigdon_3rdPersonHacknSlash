using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;

[ExecuteAlways]
public class playerCamera
{
    GameObject _owner;

    Transform _followTransform;
    Transform _follow;
    Transform _lookAtTransform;
    Transform _lookAt;

    Transform _playerCam;
    Transform _cameraTrack;
    Transform _cameraArm;

    float _followDamping;
    float _cameraLength;
    float _desiredLength;
    float _defaultLength;
    float _farLength;

    float _clampMax;
    float _clampMin;
    float _horizontalRotSpeed;
    float _verticalRotSpeed;

    bool _bInLock;

    public playerCamera(GameObject myOwner)
    {
        _owner = myOwner;

        _followTransform = GameObject.Find("followTransform").GetComponent<Transform>();
        _follow = _followTransform;
        _lookAtTransform = GameObject.Find("lookAt").GetComponent<Transform>();
        _lookAt = _lookAtTransform;

        _playerCam = Camera.main.transform;
        _cameraTrack = GameObject.Find("CameraTrack").GetComponent<Transform>();
        _cameraArm = GameObject.Find("CameraArm").GetComponent<Transform>();

        _clampMax = 40;
        _clampMin = -30;
        _horizontalRotSpeed = 12f;
        _verticalRotSpeed = 9;
        _followDamping = 0.5f;
        _defaultLength = 5;
        _farLength = 8;
        _desiredLength = _defaultLength;

        _owner.GetComponent<playerScript>().onTargetLockUpdated += TargetLockUpdated;
    }

    private void TargetLockUpdated(bool state, Transform target)
    {
        _bInLock = state;

        _lookAt = (_bInLock) ? target : _followTransform;
        _follow = (_bInLock) ? _lookAtTransform : _followTransform;
    }

    public void HandleRotation(Vector2 look)
    {
        Quaternion followLerp = _followTransform.rotation;

        if(_bInLock) 
            followLerp = LockFollow();
        else 
            followLerp = RegularFollow(followLerp, look);

        FollowRotation(followLerp);

        LerpCameraLength();
        CameraLookAt();
        CameraFollow();
    }

    private Quaternion LockFollow()
    {
        Quaternion followLerp = _owner.transform.rotation;
        return followLerp;
    }

    private Quaternion RegularFollow(Quaternion followLerp, Vector2 look)
    {
        Vector3 followPlayer = _owner.transform.position;
        followPlayer.y = 1.7f;
        _followTransform.position = followPlayer;

        followLerp *= Quaternion.AngleAxis(look.x * _horizontalRotSpeed, Vector3.up);
        followLerp *= Quaternion.AngleAxis(-look.y * _verticalRotSpeed, Vector3.right);

        return followLerp;
    }

    private void FollowRotation(Quaternion followLerp)
    {
        _followTransform.rotation = Quaternion.Slerp(_followTransform.rotation, followLerp, 5 * Time.deltaTime);

        Vector3 clampedAngle = _followTransform.eulerAngles;

        if (clampedAngle.x > 180f) clampedAngle.x -= 360f;
        clampedAngle.x = Mathf.Clamp(clampedAngle.x, _clampMin, _clampMax);

        _followTransform.rotation = Quaternion.Euler(clampedAngle);
    }

    private void CameraLookAt()
    {
        Quaternion lookAt = _follow.rotation;
        lookAt.z = 0;
        lookAt.x = 0;
        _cameraTrack.rotation = Quaternion.Lerp(_cameraTrack.rotation, lookAt, 5 * Time.deltaTime);
    }

    private void CameraFollow()
    {
        _playerCam.position = _cameraArm.position - _playerCam.forward * _cameraLength;

        _cameraTrack.position = Vector3.Lerp(_cameraTrack.position, _follow.position, (1 - _followDamping) * Time.deltaTime * 20f);
    }

    private void LerpCameraLength()
    {
        float dotProduct = Vector3.Dot(_owner.transform.forward, _playerCam.transform.forward);
        _desiredLength = (dotProduct <= 0.1f) ? _farLength : _defaultLength;

        _cameraLength = Mathf.Lerp(_cameraLength, _desiredLength, 5 * Time.deltaTime);
    }


    public Vector3 GetCameraFoward()
    {
        return _playerCam.forward;
    }
}
