using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MidBossMole : EnemyMole
{
    [SerializeField] Transform[] destinations;

    protected override void InitMonster()
    {
        maxHp = 10000;
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

        float random = Random.Range(0, 1.0f);
        if (random < 0.5f)
        {
            Attack01();
            yield return WaitSeconds.Instance[0.3f];
        }
        else
        {
            anim.SetTrigger("Hide");

            yield return WaitSeconds.Instance[0.4f];

            int ran = Random.Range(0, destinations.Length);
            model.position = destinations[ran].position;

            anim.SetTrigger("Pop");

            yield return WaitSeconds.Instance[0.6f];
            model.LookAt(Player.transform.localPosition);

            Shoot();
        }

        canAtk = false;

        yield return WaitSeconds.Instance[2f];

        nav.isStopped = false;
        currentState = State.Move;
    }

    // 십자형 공격
    void Attack01()
    {
        float rot = 0;
        for(int i=0; i<4; i++)
        {
            Shoot(rot);
            rot += 90f;
        }
    }


    void Shoot(float rot = 0f)
    {
        GameObject bullet = Instantiate(boltPrefeb, boltPosition.position, model.rotation);
        bullet.transform.rotation = Quaternion.Euler(bullet.transform.eulerAngles + new Vector3(0, rot, 0));
        bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * 8f;
    }
}
