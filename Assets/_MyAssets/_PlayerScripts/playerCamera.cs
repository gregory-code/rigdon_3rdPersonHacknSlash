using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
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
    Transform _cameraYaw;
    Transform _cameraPitch;
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
    bool _bExecuteKill;

    public playerCamera(GameObject myOwner, Transform cameraYaw, Transform cameraPitch)
    {
        _owner = myOwner;

        _followTransform = GameObject.Find("followTransform").GetComponent<Transform>();
        _follow = _followTransform;
        _lookAtTransform = GameObject.Find("lookAt").GetComponent<Transform>();
        _lookAt = _lookAtTransform;

        _playerCam = Camera.main.transform;
        _cameraYaw = cameraYaw;
        _cameraPitch = cameraPitch;
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

        _bExecuteKill = false;

        SetLookAtTransform((_bInLock) ? target : _followTransform);
        SetFollowTranform((_bInLock) ? _lookAtTransform : _followTransform);
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

    public void SetFollowTranform(Transform follow)
    {
        _follow = follow;
    }

    public void SetLookAtTransform(Transform lookat)
    {
        _lookAt = lookat;
    }

    public void ExecuteKill(bool state)
    {
        _bExecuteKill = state;
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
        followLerp.z = 0;
        followLerp.y = 0;

        _followTransform.rotation = Quaternion.Slerp(_followTransform.rotation, followLerp, 5 * Time.deltaTime);

        Quaternion clampedAngle = _followTransform.rotation;

        //if (clampedAngle.x > 180f) clampedAngle.x -= 360f;
        //clampedAngle.x = Mathf.Clamp(clampedAngle.x, _clampMin, _clampMax);

        _followTransform.rotation = clampedAngle;
    }

    private void CameraLookAt()
    {
        Quaternion lookAtYaw = _follow.rotation;
        lookAtYaw.z = 0;
        lookAtYaw.x = 0;
        _cameraYaw.rotation = Quaternion.Lerp(_cameraYaw.rotation, lookAtYaw, 5 * Time.deltaTime);

        Vector3 lookAtPitch = _follow.localEulerAngles;
        lookAtPitch.z = 0;
        lookAtPitch.y = 0;
        _cameraPitch.localEulerAngles = Vector3.Lerp(_cameraPitch.localEulerAngles, lookAtPitch, 5 * Time.deltaTime);
    }

    private void CameraFollow()
    {
        _playerCam.position = _cameraArm.position - _playerCam.forward * _cameraLength;

        _cameraYaw.position = Vector3.Lerp(_cameraYaw.position, _follow.position, (1 - _followDamping) * Time.deltaTime * 20f);
    }

    private void LerpCameraLength()
    {
        if(_bExecuteKill == false)
        {
            float dotProduct = Vector3.Dot(_owner.transform.forward, _playerCam.transform.forward);
            _desiredLength = (dotProduct <= 0.1f) ? _farLength : _defaultLength;
        }
        else
        {
            _desiredLength = _defaultLength / 1.75f;
        }

        _cameraLength = Mathf.Lerp(_cameraLength, _desiredLength, 5 * Time.deltaTime);
    }


    public Vector3 GetCameraFoward()
    {
        return _playerCam.forward;
    }
}
