using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class playerLock : MonoBehaviour
{
    Image _crosshairImage;
    Vector3 _originalScale;
    private Transform _parentTransform;
    bool _bActive;
    bool _bInLock;


    private void Start()
    {
        _crosshairImage = transform.GetComponent<Image>();
        _originalScale = transform.localScale;
        GameObject.Find("player").GetComponent<playerScript>().onTargetLockUpdated += TargetLockUpdated;
    }

    private void TargetLockUpdated(bool state, Transform target)
    {
        _bInLock = state;
    }

    public void SetupAttachment(Transform attachParent)
    {
        _parentTransform = attachParent;
        transform.position = Camera.main.WorldToScreenPoint(_parentTransform.position);
        _bActive = true;
    }

    public void RemoveAttachment()
    {
        _bActive = false;
        _crosshairImage.color = new Color(0, 0, 0, 0);
    }

    void Update()
    {
        if (_bActive == false) return;

        transform.position = Vector3.Lerp(transform.position, Camera.main.WorldToScreenPoint(_parentTransform.position), 45 * Time.deltaTime);

        if (_bInLock == false)
        {
            _crosshairImage.color = Color.Lerp(_crosshairImage.color, Color.white, 5 * Time.deltaTime);
            transform.localScale = Vector3.Lerp(transform.localScale, _originalScale, 5 * Time.deltaTime);
        }
        else
        {
            _crosshairImage.color = Color.Lerp(_crosshairImage.color, Color.red, 5 * Time.deltaTime);
            transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(0.3f, 0.3f, 0.3f), 5 * Time.deltaTime);
        }
    }
}
