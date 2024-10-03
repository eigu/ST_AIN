using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationManager : MonoBehaviour
{
    [Header("Animation Parameters")]
    [SerializeField] private bool hasAnimator = false;
    //[ConditionalHide("hasAnimator", true)] //this is how to use conditional hide
    [SerializeField] private Animator _animator;
    private float _animationBlend;
    
    private int _animIDPlayerSpeed;
    private int _animIDIsGrounded;
    private int _animIDIsJumping;
    private int _animIDIsFreeFalling;
    
    public bool HasAnimator
    {
        get => hasAnimator;
        set => hasAnimator = value;
    }

    public Animator PlayerAnim
    {
        get => _animator;
        set => _animator = value;
    }

    public float AnimationBlend
    {
        get => _animationBlend;
        set => _animationBlend = value;
    }

    public int AnimIDPlayerSpeed
    {
        get => _animIDPlayerSpeed;
        set => _animIDPlayerSpeed = value;
    }

    public int AnimIDIsGrounded
    {
        get => _animIDIsGrounded;
        set => _animIDIsGrounded = value;
    }

    public int AnimIDIsJumping
    {
        get => _animIDIsJumping;
        set => _animIDIsJumping = value;
    }

    public int AnimIDIsFreeFalling
    {
        get => _animIDIsFreeFalling;
        set => _animIDIsFreeFalling = value;
    }

    public int AnimIDIsCrouching
    {
        get => _animIDIsCrouching;
        set => _animIDIsCrouching = value;
    }

    private int _animIDIsCrouching;
    
    void Start()
    {
        AssignAnimationIDs();
    }

    private void AssignAnimationIDs()
    {
        _animIDPlayerSpeed = Animator.StringToHash("PlayerSpeed");
        _animIDIsGrounded = Animator.StringToHash("IsGrounded");
        _animIDIsJumping = Animator.StringToHash("IsJumping");
        _animIDIsFreeFalling = Animator.StringToHash("IsFreeFalling");
        _animIDIsCrouching = Animator.StringToHash("IsCrouching");
    }
}