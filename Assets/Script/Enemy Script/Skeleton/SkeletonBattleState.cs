using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonBattleState : EnemyState
{
    private EnemySkeleton enemy;
    private Transform player;
    private int moveDir;
    public SkeletonBattleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, EnemySkeleton _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        player = PlayerManager.instance.player.transform;
    }

    public override void Exit()
    {
        base.Exit();
    }
    private bool CanAttack()
    {
        if (Time.time >= enemy.lastTimeAttacked + enemy.attackCoolDown)
            return true;
        else return false;
    }

    public override void Update()
    {
        base.Update();
        if (!enemy.IsGroundDetected())
            stateMachine.changeState(enemy.idleState);
        else
        {
            if (enemy.IsPlayerDetected())
        {
                stateTimer = enemy.battleTime;
                if (enemy.IsPlayerDetected().distance < enemy.attackDistance && enemy.IsGroundDetected())
                {
                    if (CanAttack()) 
                        stateMachine.changeState(enemy.attackState);

                    return;
                }
            }
           else if (stateTimer < 0 || Vector2.Distance(player.transform.position, enemy.transform.position) > 2)

                stateMachine.changeState(enemy.idleState);

            if (player.position.x > enemy.transform.position.x)
                moveDir = 1;

            else if (player.position.x < enemy.transform.position.x)
                moveDir = -1;
            enemy.SetVelocity(enemy.moveSpeed * enemy.battleBuff * moveDir, rb.velocity.y);
        }
    }
}
