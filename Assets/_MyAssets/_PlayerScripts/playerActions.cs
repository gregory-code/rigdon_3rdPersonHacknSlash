using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class playerActions
{
    GameObject _owner;
    Animator _animator;

    Transform _katana;
    Transform _hipHolder;
    Transform _handHolder;

    bool _bSwordEquipped;
    int _timeDelay;
    int _desiredWeight;

    public playerActions(GameObject myOwner)
    {
        _owner = myOwner;
        _animator = _owner.GetComponent<Animator>();
        _katana = GameObject.Find("Katana").GetComponent<Transform>();
        _hipHolder = GameObject.FindGameObjectWithTag("hipHolder").GetComponent<Transform>();
        _handHolder = GameObject.FindGameObjectWithTag("handHolder").GetComponent<Transform>();
        _desiredWeight = 0;
    }

    public void RegularAttack()
    {
        _animator.SetLayerWeight(1, 1);
        _timeDelay = 500;

        if (_bSwordEquipped == false)
        {
            _bSwordEquipped = true;
            _animator.SetBool("bSwordEquipped", _bSwordEquipped);
        }
        else
        {
            _animator.SetBool("attack0", true);
        }
    }

    public void UpdateSword(bool toHip)
    {
        Debug.Log(toHip);
        Transform toParent = (toHip) ? _hipHolder : _handHolder;
        _katana.transform.SetParent(toParent);
        _katana.transform.localPosition = Vector3.zero;
        _katana.transform.localEulerAngles = Vector3.zero;
    }

    public void UpdateAnimWeight()
    {
        if(_timeDelay > 0)
        {
            --_timeDelay;
            return;
        }

        float weightLerp = Mathf.Lerp(_animator.GetLayerWeight(1), _desiredWeight, 3 * Time.deltaTime);
        _animator.SetLayerWeight(1, weightLerp);
    }
}
