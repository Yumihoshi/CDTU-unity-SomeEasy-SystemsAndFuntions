using System;
using System.Collections.Generic;
using Managers;
using UnityEngine;

public enum PlayerAnimationStates
{
    Idle,
    Walking,
    Jumping,
}

[RequireComponent(typeof(Animator))]
public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] private float defaultAnimationSpeed = 1f; // 默认动画速度

    private PlayerAnimationStates currentAnimationState;
    private Animator animator;
    private PlayerController playerController;

    [Tooltip("Idle状态的速度倍率")]
    [SerializeField] private float IdleSpeedMultiplier = 1f; // Idle状态的速度倍率

    [Tooltip("Walking状态的速度倍率")]
    [SerializeField] private float WalkingSpeedMultiplier = 1f; // Walking状态的速度倍率

    [Tooltip("Jumping状态的速度倍率")]
    [SerializeField] private float JumpingSpeedMultiplier = 1f; // Jumping状态的速度倍率
    private Dictionary<PlayerAnimationStates, Action> stateActions; // 存储动画逻辑
    private Dictionary<PlayerAnimationStates, float> stateSpeedMultipliers; // 各状态的速度倍率

    private static readonly int IsWalking = Animator.StringToHash("isWalking");
    private static readonly int JumpTrigger = Animator.StringToHash("jumpTrigger");

    private void Awake()
    {
        animator = GetComponent<Animator>();
        playerController = GetComponent<PlayerController>();

        // 设置默认动画速度
        animator.speed = defaultAnimationSpeed;

        // 使用 Lambda 直接绑定动画
        stateActions = new Dictionary<PlayerAnimationStates, Action>
        {
            { PlayerAnimationStates.Idle,    () => PlayIdleState() },
            { PlayerAnimationStates.Walking, () => PlayWalkingState() },
            { PlayerAnimationStates.Jumping, () => PlayJumpingState()}
        };

        // 初始化各状态的速度倍率(默认都是1)
        stateSpeedMultipliers = new Dictionary<PlayerAnimationStates, float>
        {
            { PlayerAnimationStates.Idle, IdleSpeedMultiplier },
            { PlayerAnimationStates.Walking, WalkingSpeedMultiplier },
            { PlayerAnimationStates.Jumping, JumpingSpeedMultiplier },
        };
        SetAnimationSpeed(defaultAnimationSpeed);
    }

    // 新增：设置全局动画速度
    public void SetAnimationSpeed(float speed)
    {
        defaultAnimationSpeed = Mathf.Clamp(speed, 0.01f, 10f); // 限制速度范围，防止极端值
        ApplyCurrentStateSpeed();
    }

    // 新增：设置特定状态的动画速度倍率
    public void SetAnimationSpeedForState(PlayerAnimationStates state, float multiplier)
    {
        // 原始值检查
        if (multiplier <= 0 || multiplier > 10f)
        {
            Debug.LogError(multiplier <= 0 ? "速度倍率必须大于零！" : "速度倍率不能超过10!");
        }
        multiplier = Mathf.Clamp(multiplier, 0.01f, 10f); // 强制限制在有效范围内
        stateSpeedMultipliers[state] = multiplier;

        // 如果当前正处于该状态，立即应用新速度
        if (currentAnimationState == state)
        {
            ApplyCurrentStateSpeed();
        }
    }

    // 新增：应用当前状态的动画速度
    private void ApplyCurrentStateSpeed()
    {
        float stateMultiplier = stateSpeedMultipliers[currentAnimationState];
        animator.speed = defaultAnimationSpeed * stateMultiplier;
    }

    private void PlayJumpingState()
    {
        animator.SetTrigger(JumpTrigger);
    }

    private void PlayIdleState()
    {
        animator.SetBool(IsWalking, false);
    }

    private void PlayWalkingState()
    {
        animator.SetBool(IsWalking, true);
    }

    private void Start()
    {
        ChangeAnimationState(PlayerAnimationStates.Idle); // 初始状态
    }


    private void Update()
    {
        // 移动状态判断
        ChangeAnimationState(GameInput.Instance.moveDir != Vector2.zero
            ? PlayerAnimationStates.Walking
            : PlayerAnimationStates.Idle);
        if (GameInput.Instance.JumpPressed && playerController._rb2D.linearVelocityY != 0.01f)//防止意外触发
        {
            ChangeAnimationState(PlayerAnimationStates.Jumping);
        }
    }

    // 切换动画状态
    private void ChangeAnimationState(PlayerAnimationStates newState)
    {
        if (currentAnimationState == newState) return; // 避免重复播放相同动画
        currentAnimationState = newState;
        stateActions[newState]?.Invoke(); // 直接执行动画
        animator.speed = defaultAnimationSpeed * stateSpeedMultipliers[newState];
    }
}