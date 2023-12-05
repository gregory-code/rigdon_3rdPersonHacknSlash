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

    float _pitch;
    float _yaw;

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
        _clampMin = 1;
        _horizontalRotSpeed = 45f;
        _verticalRotSpeed = 30;
        _followDamping = 0.5f;
        _defaultLength = 3;
        _farLength = 4;
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
        SetFollowTransformToPlayer();

        if (_bInLock)
        {
            LockFollow();
        }
        else
        {
            CameraLookAt(look);
        }

        RotatePitch(look.y);
        LerpCameraLength();
        CameraFollow();
    }

    private void SetFollowTransformToPlayer()
    {
        Vector3 followPlayer = _owner.transform.position;
        followPlayer.y = 1.7f;
        _followTransform.position = followPlayer;
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

    private void LockFollow()
    {
        Quaternion followLerp = _owner.transform.rotation;

        followLerp.z = 0;

        _followTransform.rotation = Quaternion.Slerp(_followTransform.rotation, followLerp, 5 * Time.deltaTime);

        _cameraYaw.rotation = Quaternion.Lerp(_cameraYaw.rotation, _follow.rotation, 5 * Time.deltaTime);

        _yaw = _cameraYaw.localEulerAngles.y;
    }
    
    private void FollowRotation(Quaternion followLerp)
    {
        followLerp.z = 0;

        _followTransform.rotation = Quaternion.Slerp(_followTransform.rotation, followLerp, 5 * Time.deltaTime);
    }

    private void CameraLookAt(Vector2 lookInput)
    {
        RotateYaw(lookInput.x);
    }

    private void RotateYaw(float lookInputX)
    {
        Vector3 cameraYaw = _cameraYaw.localEulerAngles;
        _yaw += lookInputX * Time.deltaTime * _horizontalRotSpeed;
        cameraYaw.y = _yaw;
        _cameraYaw.localEulerAngles = cameraYaw;
    }

    private void RotatePitch(float lookInputY)
    {
        Vector3 cameraPitch = _cameraPitch.localEulerAngles;
        _pitch -= lookInputY * Time.deltaTime * _verticalRotSpeed;
        _pitch = Mathf.Clamp(_pitch, -10, 40);
        cameraPitch.x = _pitch;
        _cameraPitch.localEulerAngles = cameraPitch;
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
            _desiredLength = _defaultLength / 6f;
        }

        _cameraLength = Mathf.Lerp(_cameraLength, _desiredLength, 5 * Time.deltaTime);
    }


    public Vector3 GetCameraFoward()
    {
        return _playerCam.forward;
    }
}
