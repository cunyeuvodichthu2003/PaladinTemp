using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonGroundedState : EnemyState
{
    protected EnemySkeleton enemy;
    protected Transform player => PlayerManager.instance.player.transform;

    public SkeletonGroundedState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, EnemySkeleton _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
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
        if ((enemy.IsPlayerDetected()|| (Mathf.Abs(player.position.x - enemy.rb.position.x) < 5 && Mathf.Abs(player.position.y - enemy.rb.position.y) < 1.4)) && enemy.IsGroundDetected())
            stateMachine.changeState(enemy.battleState);
    }
}
