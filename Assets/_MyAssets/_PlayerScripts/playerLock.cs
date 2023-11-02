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
    }

    public void SetupAttachment(Transform attachParent)
    {
        _parentTransform = attachParent;
        _bActive = true;
    }

    public void RemoveAttachment()
    {
        _bActive = false;
        _crosshairImage.color = new Color(0, 0, 0, 0);
    }

    public void SetLockState(bool state)
    {
        _bInLock = state;
    }

    void Update()
    {
        if (_bActive == false) return;

        transform.position = Camera.main.WorldToScreenPoint(_parentTransform.position);

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
