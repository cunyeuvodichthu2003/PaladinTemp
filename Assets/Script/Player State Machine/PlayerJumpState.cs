using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerState
{
    public PlayerJumpState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        rb.velocity = new Vector2(rb.velocity.x, player.jumpForce);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        if (rb.velocity.y < 0)
            stateMachine.ChangeState(player.airState);
        if (player.IsWallDetected() && xInput == player.facingDir)
            stateMachine.ChangeState(player.wallSlide);
        if(xInput!=0)
            player.SetVelocity(player.moveSpeed * 1f * xInput, rb.velocity.y);
    }
}
