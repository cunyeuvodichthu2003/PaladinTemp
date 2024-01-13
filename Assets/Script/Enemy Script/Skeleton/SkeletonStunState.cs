using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonStunState : EnemyState
{
    // Start is called before the first frame update
    private EnemySkeleton enemy;

    public SkeletonStunState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName,EnemySkeleton _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

   

    public override void Enter()
    {
        base.Enter();
        stateTimer = enemy.stunDuration;
        enemy.fx.InvokeRepeating("RedColorBlink",0, 0.1f);
        rb.velocity = new Vector2 (-enemy.facingDir * enemy.stunDir.x, enemy.stunDir.y);
    }

    public override void Exit()
    {
        base.Exit();
        enemy.fx.Invoke("CancelRedBlink", 0);
    }

    public override void Update()
    {
        base.Update();
       
        if (stateTimer < 0)
            stateMachine.changeState(enemy.idleState);

    }
}
