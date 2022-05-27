using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MidBossPen : EnemyPenguin
{
    [SerializeField] Transform[] destinations;
    [SerializeField] [Range(0, 1)]float range;

    protected override void InitMonster()
    {
        maxHp = 15000;
        Hp = maxHp;

        nav.speed = moveSpeed;
        nav.stoppingDistance = 0;
        attackCoolTimeCacl = attackCoolTime;

        currentState = State.Move;
    }
    protected override IEnumerator Move()
    {
        int ranIdx = Random.Range(0, destinations.Length);
        nav.SetDestination(destinations[ranIdx].position);

        while (Vector3.SqrMagnitude(destinations[ranIdx].position - model.position) > 0.15f)
        {
            if (IsCanAttcak() && canAtk)
            {
                currentState = State.Attack;
                yield break;
            }

            yield return WaitSeconds.Instance[0.2f];
        }

        if (currentState != State.Attack)
            currentState = State.Idel;
    }
    protected override void Dead()
    {
        roomCondition.enemys.Remove(model.gameObject);
        for (int i = 0; i < haveMaxExpCount; i++)
        {
            GameObject exp = PlayerData.Instance.InstantExp(model.position);
            exp.transform.SetParent(transform.parent.parent);
        }

        Destroy(this.gameObject);
    }

    protected override IEnumerator Attack()
    {
        nav.isStopped = true;
        rb.velocity = Vector3.zero;
        model.LookAt(Player.transform.localPosition);

        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
            anim.SetTrigger("Attack");

        float random = Random.Range(0, 1.0f);
        if (random < range)
        {
            Attack01();
        }
        else
        {
            StartCoroutine(Attack02());
        }

        canAtk = false;

        yield return WaitSeconds.Instance[2f];

        nav.isStopped = false;
        currentState = State.Move;
    }

    void Attack01()
    {
        int rot = 0;
        for (int i = 0; i < 3; i++)
        {
            Shoot(rot);
            rot += 120;
        }
    }

    IEnumerator Attack02()
    {
        for(int i=0; i<3; i++)
        {
            model.transform.LookAt(Player.transform);
            Shoot(0);

            yield return WaitSeconds.Instance[0.2f];
        }
    }

    void Shoot(int rot)
    {
        GameObject bullet = Instantiate(bulletPrefab, shootPoint.transform.position, model.rotation);
        bullet.transform.rotation = Quaternion.Euler(bullet.transform.eulerAngles + new Vector3(0, rot, 0));
    }
}
