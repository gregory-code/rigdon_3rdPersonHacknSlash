using Cinemachine;
using System;
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
    Transform _lookAt;
    Camera _playerCam;
    CinemachineVirtualCamera _regularVC;
    CinemachineVirtualCamera _lockedVC;

    float _clampMax;
    float _clampMin;
    float _horizontalRotSpeed;
    float _verticalRotSpeed;

    float _closeVCDistance;
    float _farVCDistance;

    bool _bInLock;

    public playerCamera(GameObject myOwner)
    {
        _owner = myOwner;
        _followTransform = GameObject.Find("followTransform").GetComponent<Transform>();
        _lookAt = GameObject.Find("lookAt").GetComponent<Transform>();
        _playerCam = Camera.main;
        _regularVC = GameObject.Find("regularVC").GetComponent<CinemachineVirtualCamera>();
        _lockedVC = GameObject.Find("lockedVC").GetComponent<CinemachineVirtualCamera>();
        _clampMax = 40;
        _clampMin = -30;
        _horizontalRotSpeed = 12f;
        _verticalRotSpeed = 9;
        _closeVCDistance = 3;
        _farVCDistance = 4.5f;

        _owner.GetComponent<playerScript>().onTargetLockUpdated += TargetLockUpdated;
    }

    private void TargetLockUpdated(bool state, Transform target)
    {
        _bInLock = state;

        _lockedVC.LookAt = target;
        _lockedVC.Follow = (_bInLock) ? _lookAt : _followTransform;

        _regularVC.enabled = !_bInLock;
        _lockedVC.enabled = _bInLock;
    }

    public void HandleRotation(Vector2 look)
    {
        Quaternion followLerp = _followTransform.transform.rotation;

        if (_bInLock)
        {
            followLerp = _owner.transform.rotation;
        }
        else
        {
            Vector3 followPlayer = _owner.transform.position;
            followPlayer.y = 1.7f;
            _followTransform.position = followPlayer;

            followLerp *= Quaternion.AngleAxis(look.x * _horizontalRotSpeed, Vector3.up);
            followLerp *= Quaternion.AngleAxis(-look.y * _verticalRotSpeed, Vector3.right);
        }

        _followTransform.transform.rotation = Quaternion.Slerp(_followTransform.transform.rotation, followLerp, 5 * Time.deltaTime);
        
        Vector3 clampedAngle = _followTransform.transform.eulerAngles;

        if (clampedAngle.x > 180f) clampedAngle.x -= 360f;
        clampedAngle.x = Mathf.Clamp(clampedAngle.x, _clampMin, _clampMax);

        _followTransform.transform.rotation = Quaternion.Euler(clampedAngle);
    }

    public void PushBackVirtualCam()
    {
        if (_regularVC == null) return;
        float dotProduct = Vector3.Dot(_owner.transform.forward, _playerCam.transform.forward);
        float distanceLerp = (dotProduct <= 0.5f) ? _farVCDistance : _closeVCDistance;
        Cinemachine3rdPersonFollow cinemachineFollow = _regularVC.GetCinemachineComponent<Cinemachine3rdPersonFollow>();
        cinemachineFollow.CameraDistance = Mathf.Lerp(cinemachineFollow.CameraDistance, distanceLerp, 3 * Time.deltaTime);
    }

    public Vector3 GetCameraFoward()
    {
        return _playerCam.transform.forward;
    }
}
