using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : EnemyFSM
{
    public GameObject BossLock;
    public Transform AttackPoint;
    [SerializeField] float lockSpeed;
    [SerializeField] GameObject playerOnly;

    WaitForSeconds Delay500 = new WaitForSeconds(0.5f);
    WaitForSeconds Delay1000 = new WaitForSeconds(1.0f);

    bool isAttack3 = false;

    new void Start()
    {
        base.Start();

        attackCoolTime = 3f;
        attackCoolTimeCacl = attackCoolTime;

        //playerRealizeRange = 13f;
        attackRange = 20f;
        moveSpeed = 1f;
        nav.stoppingDistance = 4f;
    }

    protected override void InitMonster()
    {
        maxHp = 100000f;
        Hp = maxHp;

        nav.stoppingDistance = attackRange;
        nav.speed = moveSpeed;
        attackCoolTimeCacl = attackCoolTime;

        damage = 100f;

    }

    protected override IEnumerator Attack()
    {
        yield return null;

        int randomAction = Random.Range(0, 3);

        nav.isStopped = true;
        model.LookAt(Player.transform.position);

        switch(randomAction)
        {
            case 0:
                if(!anim.GetCurrentAnimatorStateInfo(0).IsName("Attack01"))
                {
                    anim.SetTrigger("Attack01");
                }
                break;
            case 1:
                if(!anim.GetCurrentAnimatorStateInfo(0).IsName("Attack02"))
                {
                    anim.SetTrigger("Attack02");
                }
                break;
            case 2:
                isAttack3 = true;
                playerOnly.SetActive(true);

                if (!anim.GetCurrentAnimatorStateInfo(0).IsName("GetHit"))
                {
                    anim.SetTrigger("GetHit");
                }
                nav.stoppingDistance = 0f;

                Vector3 pos = model.transform.position;
                pos.y = 0.2f;
                if(Physics.Raycast(pos, model.transform.forward, out RaycastHit hit, 30f, 1<<8))
                    nav.SetDestination(hit.collider.transform.position);

                nav.isStopped = false;
                nav.speed = 200f;
                yield return WaitSeconds.Instance[2.5f];
                playerOnly.SetActive(false);
                break;
        }
        canAtk = false;
        yield return Delay500;

        nav.speed = moveSpeed;
        nav.stoppingDistance = attackRange;


        currentState = State.Idel;

        isAttack3 = false;
    }

    public void Attack01()
    {
        GameObject[] locks = new GameObject[3];
        for(int i=0; i<3; i++)
            locks[i] = Instantiate(BossLock, AttackPoint.position, model.rotation);

        locks[0].transform.rotation = Quaternion.Euler(model.eulerAngles + new Vector3(0, -35f, 0));
        locks[2].transform.rotation = Quaternion.Euler(model.eulerAngles + new Vector3(0, 35f, 0));

        for (int i = 0; i < 3; i++)
            locks[i].GetComponent<Rigidbody>().velocity = locks[i].transform.forward * lockSpeed;
    }
    public void Attack02()
    {
        GameObject[] locks = new GameObject[4];
        for (int i = 0; i < 4; i++)
            locks[i] = Instantiate(BossLock, AttackPoint.position, transform.rotation);

        locks[1].transform.rotation = Quaternion.Euler(model.eulerAngles + new Vector3(0, -15f, 0));
        locks[2].transform.rotation = Quaternion.Euler(model.eulerAngles + new Vector3(0, 15f, 0));
        locks[3].transform.rotation = Quaternion.Euler(model.eulerAngles + new Vector3(0, 30f, 0));
        locks[0].transform.rotation = Quaternion.Euler(model.eulerAngles + new Vector3(0, -30f, 0));

        for (int i = 0; i < 4; i++)
            locks[i].GetComponent<Rigidbody>().velocity = locks[i].transform.forward * lockSpeed;
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.CompareTag("Player"))
        {
            if (isAttack3)
            {
                PlayerData.Instance.PlayerDamaged(100);
            }
        }

    }

    protected override void Dead()
    {
        base.Dead();

        if(!anim.GetCurrentAnimatorStateInfo(0).IsName("Dead"))
            anim.SetTrigger("Dead");

        Destroy(gameObject, 3f);
    }
}
