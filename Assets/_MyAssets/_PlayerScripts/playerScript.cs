using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.VFX;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(Animator))]
public class playerScript : MonoBehaviour, IEventDispatcher
{
    [SerializeField] PlayerInputActions _playerInput;
    [SerializeField] playerMovement _movement;
    [SerializeField] playerCamera _camera;
    [SerializeField] playerSight _sight;
    [SerializeField] playerActions _actions;
    [SerializeField] playerLock _lock;
    [SerializeField] playerLock _lockRight;
    [SerializeField] playerLock _lockLeft;
    [SerializeField] playerLock _lockPrefab;
    [SerializeField] playerLock _lockRightPrefab;
    [SerializeField] playerLock _lockLeftPrefab;

    [SerializeField] AudioSource stepAudio;
    [SerializeField] AudioSource vfxAudio;

    Health healthComponet;
    [SerializeField] Image healthImage;
    [SerializeField] TextMeshProUGUI healthText;

    [SerializeField] Image staminaImage;
    private float maxStamina = 100;
    private float currentStamina = 100;


    [SerializeField] VisualEffect _slashVisualEffect;

    public delegate void OnTargetLockUpdated(bool state, Transform target);
    public event OnTargetLockUpdated onTargetLockUpdated;

    [SerializeField] Transform _targetedEnemy;
    [SerializeField] Transform _rightEnemy;
    [SerializeField] Transform _leftEnemy;
    [SerializeField] Transform _cameraYaw;
    [SerializeField] Transform _cameraPitch;

    private bool inQuickTim = false;

    private Animator playerAnimator;

    bool bInvincible;

    bool _bInLock;
    bool _bMovementStop;

    bool recentlyDodged;

    [SerializeField] Transform[] killAngle;

    void Start()
    {
        _playerInput = new PlayerInputActions();
        _movement = new playerMovement(gameObject);
        _camera = new playerCamera(gameObject, _cameraYaw, _cameraPitch);
        _sight = Camera.main.GetComponent<playerSight>();
        _actions = new playerActions(gameObject);
        _actions.onMovementStopUpdated += MovementStopUpdated;
        _actions.onDodgeUpdated += DodgeUpdated;
        _actions.onSwordVFX += createSwordVFX;
        _actions.onKillSetup += cameraKillSetup;

        if (FindObjectOfType<Canvas>() == null)
        {
            Debug.Log("No Canvas");
            return;
        }
        _lock = Instantiate(_lockPrefab, FindObjectOfType<Canvas>().transform);
        _lockRight = Instantiate(_lockRightPrefab, FindObjectOfType<Canvas>().transform);
        _lockLeft = Instantiate(_lockLeftPrefab, FindObjectOfType<Canvas>().transform);

        playerAnimator = GetComponent<Animator>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        _playerInput.PlayerSword.Enable();

        healthComponet = GetComponent<Health>();

        healthComponet.onHealthChanged += HealthChanged;
        healthComponet.onTakenDamage += TookDamage;
        healthComponet.onHealthEmpty += Death;
    }

    private void HealthChanged(float currentHealth, float amount, float maxHealth)
    {
        if (bInvincible)
            return;

        healthText.text = $"Health: {currentHealth}/{maxHealth}";
        StartCoroutine(changeHealthOverTime(currentHealth, maxHealth));
    }

    private IEnumerator changeHealthOverTime(float currentHealth, float maxHealth)
    {
        float time = 120;
        while(time > 0)
        {
            time--;
            yield return new WaitForEndOfFrame();
            healthImage.fillAmount = Mathf.Lerp(healthImage.fillAmount, (currentHealth / maxHealth), 8 * Time.deltaTime);
        }
    }

    private void TookDamage(float currentHealth, float amount, float maxHealth, GameObject instigator, string hitAnim)
    {
        if (bInvincible)
            return;

        playerAnimator.SetTrigger(hitAnim);
        _bMovementStop = true;
        _actions.HitScreenEffects();
        _movement.SetBurst(20, 4, -transform.forward);
        DamageEffects(amount);
    }

    private void DamageEffects(float damage)
    {
        //GameObject damagePop = Instantiate(damagepopPrefab, FindObjectOfType<Canvas>().transform);
        //damagePop.GetComponent<damagepop>().Init(indicatorSpawns, damage);

        //GameObject hit = Instantiate(hitEffect, this.transform);
        //Destroy(hit, 1);
    }

    public void SlowDownAttacked(Transform enemyAttacking)
    {

        if(inQuickTim)
        {
            inQuickTim = false;
            Time.timeScale = 1f;
        }
        else
        {
            inQuickTim = true;
            _targetedEnemy = enemyAttacking;
            _bInLock = false;
            SwitchLock();

            Time.timeScale = 0.06f;
        }

        _camera.inQuickTime(inQuickTim);
    }

    private void Death(float amount, float maxHealth)
    {
        
    }

    void Update()
    {
        if (_movement == null) return;
        if (_camera == null) return;
        if (_sight == null) return;
        if (_actions == null) return;

        currentStamina = Mathf.Lerp(currentStamina, 100, 0.075f * Time.deltaTime);

        staminaImage.fillAmount = Mathf.Lerp(staminaImage.fillAmount, (currentStamina / maxStamina), 8 * Time.deltaTime);

        Look();

        if (_bMovementStop == false) Movement();

        _movement.BurstFoward();

        ActionUpdate();


        if (_bInLock == false)
        {
            _targetedEnemy = _sight.GetClosestEnemy(false, false);
        }
        else
        {
            _rightEnemy = _sight.GetClosestEnemy(true, false);
            _leftEnemy = _sight.GetClosestEnemy(true, true);
            if(TooFarToLock() || NotAnEnemy()) SwitchLock();
        }

        AdjustAttachment(_targetedEnemy, _lock);
        AdjustAttachment(_leftEnemy, _lockLeft);
        AdjustAttachment(_rightEnemy, _lockRight);
    }

    private void AdjustAttachment(Transform enemy, playerLock lockAttachment)
    {
        if (enemy == null)
        {
            lockAttachment.RemoveAttachment();
        }
        else
        {
            lockAttachment.SetupAttachment(enemy);
        }
    }

    private bool TooFarToLock()
    {
        float dis = Vector3.Distance(transform.position, _targetedEnemy.position);
        if(dis >= 12.2f)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool NotAnEnemy()
    {
        if(_targetedEnemy.gameObject.layer != 6)
        {
            return true;
        }

        return false;
    }

    private void Look()
    {
        Vector2 lookVector = _playerInput.PlayerSword.Look.ReadValue<Vector2>();
        _camera.HandleRotation(lookVector);
    }

    private void Movement()
    {
        Vector2 inputVector = _playerInput.PlayerSword.Movement.ReadValue<Vector2>();
        Vector3 movementDir = new Vector3(inputVector.x, 0, inputVector.y);
        _movement.MoveCharacter(inputVector, movementDir, _camera.GetCameraFoward());
        _movement.MoveToGravity();
    }

    public void Sprint(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            if (_movement == null) return;
            _movement.SprintToggle();
        }
    }

    public void RegularAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (_actions == null) return;
            _actions.RegularAttackInput();
        }
    }

    public void DodgeRoll(InputAction.CallbackContext context)
    {

        if(context.performed && inQuickTim)
        {
            if (_actions == null) return;
            if (CheckDodgeValue() == false)
                return;
            recentlyDodged = true;
            //SlowDownAttacked(_targetedEnemy);
            _actions.DodgeInput(true);
        }

        if (context.performed && _bMovementStop == false)
        {
            if (_actions == null) return;
            if (CheckDodgeValue() == false)
                return;
            _actions.DodgeInput(false);
        }
    }

    private bool CheckDodgeValue()
    {
        if (currentStamina >= 20)
        {
            currentStamina -= 20;
            return true;
        }
        else
        {
            return false;
        }
    }

    public void LockOn(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            SwitchLock();
        }
    }

    private void SwitchLock()
    {
        if (_targetedEnemy == null) return;

        _bInLock = !_bInLock;

        if (_bInLock == false)
        {
            _rightEnemy = null;
            _leftEnemy = null;
        }

        Transform target = (_bInLock) ? _targetedEnemy : gameObject.transform.Find("lookAt").transform;

        onTargetLockUpdated?.Invoke(_bInLock, target);
    }

    public void RightFocus(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (_rightEnemy == null || _bInLock == false) return;

            Transform target = _rightEnemy;
            _targetedEnemy = _rightEnemy;
            onTargetLockUpdated?.Invoke(_bInLock, target);
        }
    }

    public void LeftFocus(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (_leftEnemy == null || _bInLock == false) return;

            Transform target = _leftEnemy;
            _targetedEnemy = _leftEnemy;
            onTargetLockUpdated?.Invoke(_bInLock, target);
        }
    }

    private void ActionUpdate()
    {
        _actions.Update();
    }

    private void MovementStopUpdated(bool state)
    {
        _bMovementStop = state;
    }

    private void DodgeUpdated()
    {
        if (_actions == null) return;



        Vector2 inputVector = _playerInput.PlayerSword.Movement.ReadValue<Vector2>();
        Vector3 movementDir = transform.forward;

        if (_bInLock && inputVector != Vector2.zero)
        {
            movementDir = new Vector3(inputVector.x, 0, inputVector.y);
            Vector3 relativeDirection = _movement.GetLockedDirection(movementDir);
            movementDir = transform.TransformDirection(relativeDirection);
        }

        movementDir = movementDir.normalized;
        float rightSpeed = Vector3.Dot(movementDir, transform.right);
        float forwardSpeed = Vector3.Dot(movementDir, transform.forward);

        float speed = 12;
        float duration = 220;

        if (inQuickTim)
            speed = 16;

        if (inQuickTim)
            duration = 240;

        _movement.SetBurst(duration, speed, movementDir);

        GetComponent<Animator>().SetFloat("leftSpeed", rightSpeed);
        GetComponent<Animator>().SetFloat("fowardSpeed", forwardSpeed);
    }

    private void cameraKillSetup(int which)
    {
        if (_camera == null) return;

        _camera.ExecuteKill(true);
        _camera.SetFollowTranform(killAngle[which]);
        _camera.SetLookAtTransform(transform);
    }

    private void createSwordVFX(Transform spawnLocation)
    {
        VisualEffect swordVFX = Instantiate(_slashVisualEffect, spawnLocation.transform.position, spawnLocation.transform.rotation);
        //swordVFX.transform.SetParent(spawnLocation);
        Destroy(swordVFX.gameObject, 2);
    }

    public void SendEvent(AnimEvent animEvent)
    {
        if(animEvent.soundEfx != null)
        {
            if(animEvent.bIsStep)
            {
                playAudio(stepAudio, animEvent.soundEfx);
            }
            else
            {
                playAudio(vfxAudio, animEvent.soundEfx);
            }
        }

        switch(animEvent.functionName)
        {
            case "Attack":
                _actions.AttackHit(animEvent.hitAnim);
                break;

            case "StepFoward":
                _movement.SetBurst(animEvent.time, animEvent.speed, transform.forward);
                break;

            case "StartSwing":
                _actions.StartSwingEffect();
                break;

            case "HitScreenEffects":
                _actions.HitScreenEffects();
                break;

            case "FinishFlourish":
                bInvincible = false;
                recentlyDodged = false;
                _actions.FinishFlourish();
                break;

            case "CheckAttack":
                _actions.CheckAttack();
                break;

            case "GrabSword":
                _actions.UpdateSword(false);
                break;

            case "StoreSword":
                _actions.UpdateSword(true);
                break;

            case "ResumeMove":
                _bMovementStop = false;
                break;

            case "StartRoll":
                bInvincible = true;
                break;

            case "FinishRoll":
                if(recentlyDodged)
                {
                    _actions.inQuickTime = true;
                    _actions.Attack("Kill em");
                }
                break;
        }
    }

    private void playAudio(AudioSource player, AudioClip clip)
    {
        player.clip = clip;
        player.Play();
    }
}
