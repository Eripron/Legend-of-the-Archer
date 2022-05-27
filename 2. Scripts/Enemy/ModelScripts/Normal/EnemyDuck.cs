using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDuck : EnemyFSM
{
    [SerializeField] GameObject dangerLine;
    [SerializeField] GameObject playerCheckCol;

    private new void Start()
    {
        base.Start();
    }

    WaitForSeconds Delay500 = new WaitForSeconds(0.5f);
    WaitForSeconds Delay1500 = new WaitForSeconds(1.5f);
    protected override IEnumerator Attack()
    {
        nav.isStopped = true;
        nav.stoppingDistance = 0f;
        rb.velocity = Vector3.zero;

        MakeDangerLine();

        yield return WaitSeconds.Instance[beforeAtkTime];

        playerCheckCol.SetActive(true);

        canAtk = false;
        nav.isStopped = false;
        nav.speed = 50f;
        nav.acceleration = 25f;

        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
            anim.SetTrigger("Attack");

        AtkEffect();

        yield return Delay500;

        nav.acceleration = 6f;
        nav.speed = moveSpeed;
        nav.stoppingDistance = attackRange;

        yield return Delay1500;

        playerCheckCol.SetActive(false);
        currentState = State.Idel;
    }

    void MakeDangerLine()
    {
        Vector3 startPos = model.position;
        startPos.y = 0.2f;

        Vector3 destination = startPos + model.transform.forward * attackRange;

        GameObject clone = Instantiate(dangerLine, startPos, model.rotation);
        if (clone.TryGetComponent<DangerLine>(out DangerLine dl))
            dl.EndPosition = destination;

        nav.SetDestination(destination);
    }

}
