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
    bool _bOpenInput;

    int _inputAge;

    private enum input { None, Regular, Dodge, Kill }
    private input _storedInput;

    public delegate void OnMovementStopUpdated(bool state);
    public event OnMovementStopUpdated onMovementStopUpdated;

    public delegate void OnDodgeUpdated();
    public event OnDodgeUpdated onDodgeUpdated;

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

    public void RegularAttackInput()
    {
        if (_bSwordEquipped == false)
        {
            DrawSword(true);
            return;
        }

        onMovementStopUpdated?.Invoke(true);

        if (_bOpenInput == false && _nextAttackIndex > 0)
        {
            StoreInput(100, input.Regular);
            return;
        }
        Attack("attack");
    }

    private void Attack(string attackName)
    {
        _bOpenInput = false;
        _animator.SetLayerWeight(1, 0);
        _animator.SetBool(attackName + _nextAttackIndex, true);
        _nextAttackIndex++;
    }

    public void DodgeInput()
    {
        if (_bOpenInput == true)
        {
            StoreInput(100, input.Dodge);
            return;
        }

        Debug.Log("Left: " + _animator.GetFloat("leftSpeed"));
        Debug.Log("Foward: " + _animator.GetFloat("fowardSpeed"));

        _animator.SetTrigger("dodge");
        onMovementStopUpdated?.Invoke(true);
        onDodgeUpdated?.Invoke();
    }

    private void StoreInput(int setAge, input storedInput)
    {
        _inputAge = setAge;
        _storedInput = storedInput;
    }

    public void CheckAttack()
    {
        _bOpenInput = true;
        CheckInput();
    }

    public void AttackCutOff()
    {
        _bOpenInput = false;
    }

    public void FinishFlourish()
    {
        for(int i = 0; i < 4; i++)
        {
            _animator.SetBool("attack" + i, false);
        }
        _animator.ResetTrigger("dodge");

        onMovementStopUpdated?.Invoke(false);
        _bOpenInput = false;
        _nextAttackIndex = 0;

        if(_storedInput != input.None)
        {
            CheckInput();
        }
    }

    private void CheckInput()
    {
        switch (_storedInput)
        {
            case input.None:
                return;

            case input.Regular:
                Attack("attack");
                return;

            case input.Dodge:
                DodgeInput();
                return;

            case input.Kill:
                return;
        }
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
            if (_inputAge <= 0) _storedInput = input.None;
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
