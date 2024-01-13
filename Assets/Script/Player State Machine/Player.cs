using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
    public bool isBusy { get; private set; }
    [Header("Attack detail")]
    public Vector2[] attackMovement;
    public float counterAttackDuration = 0.2f;
    
    [Header("Move Info")]
    public float moveSpeed = 1f;
    public float jumpForce = 2f;
    public float catchSwordImpact = 0f;
    [Header("Dash Info")]
    public float dashSpeed = 5f;
    public float dashDuration = 0f;
    public float dashDir { get; private set; }

    public SkillManager skill { get; private set; }
    public GameObject sword { get; private set; }
    
    #region State
    public PlayerStateMachine stateMachine { get; private set; }
    public PlayerIddleState idleState { get; private set; }
    public PlayerMoveState moveState { get; private set; }
    public PlayerJumpState jumpState { get; private set; }
    public PlayerAirState airState { get; private set; }
    public PlayerDashState dashState { get; private set; }
    public PlayerWallSlide wallSlide { get; private set; }
    public PlayerWallJumpState wallJump { get; private set; }
    public PlayerPrimaryAttackState primaryAttack { get; private set; }
    public PlayerCounterAttackState counterAttack { get; private set; }
    public PlayerAimSwordState aimSwordState { get; private set; }
    public PlayerCatchSwordState catchSwordState { get; private set; }


  
    #endregion
    private  void Awake()
    {
        base.Awake();
        stateMachine = new PlayerStateMachine();
        idleState = new PlayerIddleState(this, stateMachine, "Idle");
        moveState = new PlayerMoveState(this, stateMachine, "Move");
        airState = new PlayerAirState(this, stateMachine, "Jump");
        jumpState = new PlayerJumpState(this, stateMachine, "Jump");
        dashState = new PlayerDashState(this, stateMachine, "Dash");
        wallSlide = new PlayerWallSlide(this, stateMachine, "WallSlide");
        wallJump = new PlayerWallJumpState(this, stateMachine, "Jump");
        primaryAttack = new PlayerPrimaryAttackState(this, stateMachine, "Attack");
        counterAttack = new PlayerCounterAttackState(this, stateMachine, "CounterAttack");
        catchSwordState = new PlayerCatchSwordState(this, stateMachine, "CatchSword");
        aimSwordState = new PlayerAimSwordState(this, stateMachine, "AimSword");
        
    }
    private void Start()
    {
        base.Start();
        stateMachine.Initialize(idleState);
        skill = SkillManager.instance;
        
    }

    private void Update()
    {
        base.Update();
        
        stateMachine.currentState.Update();
        anim.SetFloat("yVelocity", rb.velocity.y);
        CheckForDashInput(); 
    }
    public void AssignNewSword(GameObject _newSword)
    {
        sword = _newSword;
    }
    public void CatchSword()
    {
        stateMachine.ChangeState(catchSwordState);
        Destroy(sword);
    }
    IEnumerator BusyFor (float _second)
    {
        isBusy = true;
        yield return new WaitForSeconds(_second);
        isBusy = false;
    }
    public void AnimationTrigger()
        => stateMachine.currentState.AnimationFinishTrigger();
    private void CheckForDashInput()
    {
        if (IsWallDetected())
            return;
        if (Input.GetKeyDown(KeyCode.LeftShift) && SkillManager.instance.dash.CanUseSkill())
        { 
            dashDir = Input.GetAxisRaw("Horizontal");

            if (dashDir == 0)
                dashDir = facingDir;
            stateMachine.ChangeState(dashState);
        }

    }
    
    
    
}
