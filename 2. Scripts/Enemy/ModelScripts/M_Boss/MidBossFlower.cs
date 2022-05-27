using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MidBossFlower : EnemyFSM
{
    [SerializeField] Bullet bulletPrefab;

    [SerializeField] Transform[] destinations;
    [SerializeField] Transform attackPosition;
    [SerializeField] Transform storePos;

    List<Bullet> bulletList;


    new void Start()
    {
        base.Start();
        InitBulletList();
    }

    protected override void InitMonster()
    {
        maxHp = 20000;
        Hp = maxHp;

        nav.speed = moveSpeed;
        nav.stoppingDistance = 0;
        attackCoolTimeCacl = attackCoolTime;

        currentState = State.Move;
    }

    protected override IEnumerator Attack()
    {
        // attack state enter -> stop
        // 확률적으로 공격 

        // stop
        nav.isStopped = true;
        rb.velocity = Vector3.zero;
        model.LookAt(Player.transform.position);

        // attack
        float random = Random.Range(0, 1.0f);
        if(random < 0.5f)
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
        // 무작위 방향 
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Attack01"))
            anim.SetTrigger("Attack01");

        int count = 15, rot = 0;
        int mid = 360 / count;
        for (int i = 0; i < count; i++)
        {
            ThrowBullet(rot);
            rot += Random.Range(mid - 2, mid + 2);
        }

    }

    IEnumerator Attack02()
    {
        // 플레이어 방향 
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Attack02"))
            anim.SetTrigger("Attack02");

        int rot = -20;
        for (int i = 0; i < 8; i++)
        {
            ThrowBullet(rot);
            yield return WaitSeconds.Instance[0.1f];
            rot += 10;
        }
    }



    void ThrowBullet(int rot)
    {
        Bullet bullet = GetBullet();
        bullet.transform.rotation = Quaternion.Euler(bullet.transform.eulerAngles
                                                    + new Vector3(0, rot, 0));
        bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * 7f;
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


    // bullet pooling 
    void InitBulletList()
    {
        bulletList = new List<Bullet>();

        for (int i = 0; i < 15; i++)
            CreateBullet();
    }
    Bullet CreateBullet()
    {
        Bullet bullet = Instantiate(bulletPrefab, storePos);

        bullet.gameObject.SetActive(false);
        bulletList.Add(bullet);

        return bullet;
    }
    Bullet GetBullet()
    {
        Bullet bullet = null;

        for(int i=0; i<bulletList.Count; i++)
        {
            if(!bulletList[i].gameObject.activeSelf)
            {
                bullet = bulletList[i];
                break;
            }
        }

        if(bullet == null)
            bullet = CreateBullet();

        bullet.transform.position = attackPosition.position;
        bullet.transform.rotation = attackPosition.rotation;
        bullet.gameObject.SetActive(true);

        return bullet;
    }

}
