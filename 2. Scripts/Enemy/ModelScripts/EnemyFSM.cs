using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyFSM : EnemyBase
{

    public enum State
    {
        Idel,
        Move,
        Attack
    }

    protected State currentState = State.Idel;


    protected new void Start()
    {
        base.Start();

        StartCoroutine(FSM());
    }

    protected virtual void InitMonster() 
    {
        int stageNum = StageManager.Instance.CurStageNum;

        maxHp = 500 + stageNum * 50;
        Hp = maxHp;

        nav.stoppingDistance = attackRange;
        nav.speed = moveSpeed;

        attackCoolTimeCacl = attackCoolTime;
    }

    protected virtual IEnumerator FSM()
    {
        while (!roomCondition.IsPlayerInRoom)
            yield return WaitSeconds.Instance[0.5f];

        InitMonster();

        while(Hp > 0)
        {
            yield return StartCoroutine(currentState.ToString());
        }
    }

    protected virtual IEnumerator Idel()
    {
        yield return null;

        model.LookAt(Player.transform);
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Idel"))
            anim.SetTrigger("Idel");


        if(IsCanAttcak())
        {
            if (canAtk)
                currentState = State.Attack;
            else
            {
                currentState = State.Idel;
            }
        }
        else
        {
            currentState = State.Move;
        }
    }

    protected virtual void AtkEffect() { }

    protected virtual IEnumerator Attack()
    {
        yield return null;
    }

    protected virtual IEnumerator Move()
    {
        yield return null;

        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Walk"))
            anim.SetTrigger("Walk");

        model.LookAt(Player.transform.position);

        if (IsCanAttcak() && canAtk)
            currentState = State.Attack;
        else
            nav.SetDestination(Player.transform.position);
    }


}
