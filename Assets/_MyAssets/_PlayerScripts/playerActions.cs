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
    bool _bReadyForNextInput;

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
        _bReadyForNextInput = true;
    }

    public void RegularAttackInput()
    {
        if (_bSwordEquipped == false)
        {
            DrawSword(true);
            return;
        }

        StoreInput(100, input.Regular);
        if(_nextAttackIndex == 0) CheckInput();
    }

    private void Attack(string attackName)
    {
        onMovementStopUpdated?.Invoke(true);

        _animator.SetLayerWeight(1, 0);
        _animator.SetBool(attackName + _nextAttackIndex, true);
        _nextAttackIndex++;
    }

    public void DodgeInput()
    {
        StoreInput(100, input.Dodge);

        if (_bReadyForNextInput == true) Dodge();
    }

    private void Dodge()
    {
        _animator.SetTrigger("dodge");
        onMovementStopUpdated?.Invoke(true);
        onDodgeUpdated?.Invoke();
    }

    #region Anim Events

    public void CheckAttack()
    {
        CheckInput();
    }

    public void FinishFlourish()
    {
        for(int i = 0; i < 4; i++)
        {
            _animator.SetBool("attack" + i, false);
        }
        _animator.ResetTrigger("dodge");

        onMovementStopUpdated?.Invoke(false);
        _nextAttackIndex = 0;

        CheckInput();
    }

    #endregion

    private void CheckInput()
    {
        _bReadyForNextInput = false;

        switch (_storedInput)
        {
            case input.None:
                _bReadyForNextInput = true;
                _inputAge = 1;
                break;

            case input.Regular:
                Attack("attack");
                break;

            case input.Dodge:
                Dodge();
                break;

            case input.Kill:
                break;
        }

        _storedInput = input.None;
    }

    private void StoreInput(int setAge, input storedInput)
    {
        _storedInput = storedInput;
        _inputAge = setAge;
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
            if (_inputAge <= 0)
            {
                _storedInput = input.None;
            }
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
