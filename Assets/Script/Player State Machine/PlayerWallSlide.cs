using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallSlide : PlayerState
{
    public PlayerWallSlide(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        if (Input.GetKeyDown(KeyCode.Space) && xInput!=player.facingDir)
        { 
            stateMachine.ChangeState(player.wallJump);
            return;        
        }
        
        if(Input.GetKeyDown(KeyCode.LeftShift))
        {
            return;
        }
        if(xInput!=0&&player.facingDir != xInput)
        {           
                stateMachine.ChangeState(player.idleState);         
        }
        if (yInput < 0)
            rb.velocity = new Vector2(0, rb.velocity.y);
        else
            rb.velocity = new Vector2(0, rb.velocity.y * 0.7f);
        if (player.IsGroundDetected())
            stateMachine.ChangeState(player.idleState);
    }
}
