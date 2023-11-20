using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.VFX;

public class playerActions
{
    GameObject _owner;
    Animator _animator;

    Transform _katana;
    KatanaHit _katanaHit;
    Transform _hipHolder;
    Transform _handHolder;

    Transform _vfxPlacement;

    bool _bSwordEquipped;
    int _timeDelay;
    int _desiredWeight;

    // What?

    int _nextAttackIndex;
    bool _bReadyForNextInput;

    int _inputAge;

    bool _bInLock;
    bool _bFreshBlood;
    Transform _target;

    private enum input { None, Regular, Dodge, Kill }
    private input _storedInput;

    public delegate void OnMovementStopUpdated(bool state);
    public event OnMovementStopUpdated onMovementStopUpdated;

    public delegate void OnDodgeUpdated();
    public event OnDodgeUpdated onDodgeUpdated;

    public delegate void OnKillSetup(int which);
    public event OnKillSetup onKillSetup;

    public delegate void OnSwordVFX(Transform spawnLocation);
    public event OnSwordVFX onSwordVFX;

    public playerActions(GameObject myOwner)
    {
        _owner = myOwner;
        _animator = _owner.GetComponent<Animator>();
        _katana = GameObject.Find("Katana").GetComponent<Transform>();
        _katanaHit = _katana.GetComponent<KatanaHit>();
        _hipHolder = GameObject.FindGameObjectWithTag("hipHolder").GetComponent<Transform>();
        _handHolder = GameObject.FindGameObjectWithTag("handHolder").GetComponent<Transform>();
        _vfxPlacement = _handHolder.GetChild(0).GetComponent<Transform>();

        _owner.GetComponent<playerScript>().onTargetLockUpdated += TargetLockUpdated;

        _desiredWeight = 0;
        _nextAttackIndex = 0;
        _bReadyForNextInput = true;
    }

    private void TargetLockUpdated(bool state, Transform target)
    {
        _bInLock = state;
        _target = target;
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

        if(CanKill())
        {
            KillSetup();
            return;
        }

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
    private void KillSetup()
    {
        System.Random ranNum = new System.Random();
        int randomKill = ranNum.Next(0, 2);

        onKillSetup?.Invoke(ranNum.Next(0, 3));
        _target.GetComponent<enemyBase>().KillSetup(randomKill);
        _animator.SetTrigger("execute" + randomKill);
    }

    private bool CanKill()
    {
        if(_nextAttackIndex == 3 && _bInLock && _bFreshBlood)
        {
            return true;
        }

        return false;
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
        _bFreshBlood = false;

        onMovementStopUpdated?.Invoke(false);
        _nextAttackIndex = 0;

        CheckInput();
    }

    public void AttackHit(string hitAnim)
    {
        List<GameObject> enemies = _katanaHit.GetHitEnemies();

        foreach (GameObject enemy in enemies)
        {
            HitScreenEffects();
            SetFreshBlood(enemy);
            Health enemyHealth = enemy.GetComponent<Health>();
            enemyHealth.ChangeHealth(-5, _owner.transform.gameObject, hitAnim);
        }
    }

    public void HitScreenEffects()
    {
        GameObject.FindObjectOfType<ScreenVFX>().StartShake();
        GameObject.FindObjectOfType<FreezeFrame>().StartFreezeFrame();
    }

    private void SetFreshBlood(GameObject enemy)
    {
        if (enemy.transform == _target && _bInLock)
        {
            _bFreshBlood = true;
        }
    }

    public void StartSwingEffect()
    {
        onSwordVFX?.Invoke(_vfxPlacement);
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
