using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor.Rendering;
using UnityEngine;
using static playerScript;

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

    // What?

    int _nextAttackIndex;
    bool _bRecieveAttack;

    int _inputAge;
    bool _bStoredInput;

    public delegate void OnMovementStopUpdated(bool state);
    public event OnMovementStopUpdated onMovementStopUpdated;

    public playerActions(GameObject myOwner)
    {
        _owner = myOwner;
        _animator = _owner.GetComponent<Animator>();
        _katana = GameObject.Find("Katana").GetComponent<Transform>();
        _hipHolder = GameObject.FindGameObjectWithTag("hipHolder").GetComponent<Transform>();
        _handHolder = GameObject.FindGameObjectWithTag("handHolder").GetComponent<Transform>();
        _desiredWeight = 0;
        _nextAttackIndex = 0;
    }

    public void RegularAttack()
    {
        if (_bSwordEquipped == false)
        {
            DrawSword(true);
            return;
        }

        onMovementStopUpdated?.Invoke(true);

        if (_bRecieveAttack == false && _nextAttackIndex > 0)
        {
            StoreInput(100);
            return;
        }
        ExecuteAttack();
    }

    private void ExecuteAttack()
    {
        _bRecieveAttack = false;
        _animator.SetLayerWeight(1, 0);
        _animator.SetBool("attack" + _nextAttackIndex, true);
        _nextAttackIndex++;
    }

    private void StoreInput(int setAge)
    {
        _inputAge = setAge;
        _bStoredInput = true;
    }

    public void CheckAttack()
    {
        _bRecieveAttack = true;
        if (_bStoredInput) ExecuteAttack();
    }

    public void AttackCutOff()
    {
        _bRecieveAttack = false;
    }

    public void FinishFlourish()
    {
        for(int i = 0; i < 4; i++)
        {
            _animator.SetBool("attack" + i, false);
        }
        onMovementStopUpdated?.Invoke(false);
        _bRecieveAttack = false;
        _nextAttackIndex = 0;
    }

    private void DrawSword(bool bState)
    {
        _animator.SetLayerWeight(1, 1);
        _timeDelay = 500;
        _desiredWeight = 0;
        _bSwordEquipped = bState;
        _animator.SetBool("bSwordEquipped", _bSwordEquipped);
    }

    public void UpdateSword(bool toHip)
    {
        Debug.Log(toHip);
        Transform toParent = (toHip) ? _hipHolder : _handHolder;
        _katana.transform.SetParent(toParent);
        _katana.transform.localPosition = Vector3.zero;
        _katana.transform.localEulerAngles = Vector3.zero;
    }

    public void Update()
    {
        if(_inputAge > 0)
        {
            --_inputAge;
            if (_inputAge <= 0) _bStoredInput = false;
        }

        if(_timeDelay > 0)
        {
            --_timeDelay;
            return;
        }

        float weightLerp = Mathf.Lerp(_animator.GetLayerWeight(1), _desiredWeight, 3 * Time.deltaTime);
        _animator.SetLayerWeight(1, weightLerp);
    }
}
