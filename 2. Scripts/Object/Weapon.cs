using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] int maxDamage;
    [SerializeField] float speed;

    System.Action<Weapon> delBack;

    Rigidbody rigid;

    float damage;

    int bouncCount = 2;
    int reflectCount = 2;

    Vector3 newDir;

    private void OnEnable()
    {
        if(rigid == null)
            rigid = GetComponent<Rigidbody>();

        rigid.velocity = transform.forward * 30;
        newDir = transform.forward;
        damage = Random.Range(PlayerData.Instance.Damage - 10, PlayerData.Instance.Damage + 10);

        reflectCount = 2;
        bouncCount = 2;
    }

    int beforeIndex = -1;
    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.CompareTag("Monster"))
        {
            if (other.transform.parent.TryGetComponent<EnemyBase>(out EnemyBase baseEnemy))
                baseEnemy.Damaged((int)damage);

            if (PlayerData.Instance.PlayerSkill((int)SkillName.RICOCHET) > 0 && TargetingController.Instance.Enemys.Count >= 2)
            {
                int myIndex = TargetingController.Instance.Enemys.IndexOf(other.gameObject);

                if(bouncCount > 0)
                {
                    bouncCount--;
                    speed *= 1.2f;
                    newDir = NextMonsterDir(myIndex) * speed;
                    rigid.velocity = newDir;
                    return;
                }
            }
            rigid.velocity = Vector3.zero;

            delBack?.Invoke(this);
        }

    }

    Vector3 NextMonsterDir(int omissionIndex)
    {
        int index = -1;
        float minDistance = float.MaxValue;

        for(int i=0; i<TargetingController.Instance.Enemys.Count; i++)
        {
            if (i == omissionIndex || i == beforeIndex) continue; 

            float curDis = Vector3.Distance(transform.position, TargetingController.Instance.Enemys[i].transform.position);

            if (curDis > 5f) 
                continue;

            if (curDis < minDistance)
            {
                index = i;
                minDistance = curDis;
            }
        }

        if(index == -1)
        {
            delBack?.Invoke(this);
            return Vector3.zero;
        }

        beforeIndex = omissionIndex;
        return (TargetingController.Instance.Enemys[index].transform.position - transform.position).normalized;
    }

    public void SetDelFun(System.Action<Weapon> func)
    {
        delBack = func;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Wall"))
        {
            if (PlayerData.Instance.PlayerSkill((int)SkillName.BOUNCYWALL) > 0)
            {
                if (reflectCount > 0)
                {
                    reflectCount--;
                    newDir = Vector3.Reflect(newDir, collision.contacts[0].normal);
                    rigid.velocity = newDir * speed;
                    return;
                }
            }
            rigid.velocity = Vector3.zero;

            delBack?.Invoke(this);
        }
    }
}
