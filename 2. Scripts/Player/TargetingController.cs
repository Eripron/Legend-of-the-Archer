using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetingController : Singleton<TargetingController>
{
    List<GameObject> enemys = new List<GameObject>();

    [SerializeField] WeaponManager wm;
    [Header("Attack")]
    [SerializeField] Transform attackPosition;
    [SerializeField] LayerMask targetMask;

    Transform playerPos;
    Transform targetPos;

    PlayerMovement pMovement;
    PlayerData data;

    public List<GameObject> Enemys => enemys;

    void Start()
    {
        playerPos = transform;
        pMovement = GetComponent<PlayerMovement>();
        data = PlayerData.Instance;
    }

    Weapon Potato()
    {
        return wm.GetFoolObject(attackPosition.position, attackPosition.rotation);
    }
    Weapon Potato(Quaternion rotation)
    {
        return wm.GetFoolObject(attackPosition.position, rotation);
    }
    void DoublePotato()
    {
        Weapon potatoL = Potato();
        potatoL.transform.position += (potatoL.transform.right * -0.1f);
        Weapon potatoR = Potato();
        potatoR.transform.position += (potatoR.transform.right * 0.1f);
    }


    public void OnInitEnemyInfo(List<GameObject> enemys)
    {
        if (this.enemys.Count > 0)
            this.enemys.Clear();

        this.enemys = enemys;
        TargetingEnemy();
    }

    public void TargetingEnemy()
    {
        if (enemys.Count > 0)
        {
            pMovement.Anim.SetBool("IsAttack", true);
            pMovement.Anim.SetFloat("AttackSpeed", PlayerData.Instance.AttackSpeed);

            // index [0] non-wall / [1] all
            if (enemys.Count > 1)
            {
                Transform[] nearTargets = new Transform[2];
                float[] minDistances = { float.MaxValue, float.MaxValue };

                for (int i = 0; i < enemys.Count; i++)
                {
                    Transform target = enemys[i].transform;
                    Vector3 startPos = transform.position + Vector3.up * 0.5f;

                    float distance = Vector3.Distance(startPos, target.position);
                    Vector3 dir = target.position - startPos;

                    RaycastHit hit;
                    if (Physics.Raycast(startPos, dir, out hit, float.MaxValue, targetMask))
                    {
                        if (hit.collider.CompareTag("Monster"))
                        {
                            if (distance < minDistances[0])
                            {
                                minDistances[0] = distance;
                                nearTargets[0] = target;
                            }
                        }

                        if (distance < minDistances[1])
                        {
                            minDistances[1] = distance;
                            nearTargets[1] = target;
                        }
                    }
                }

                targetPos = nearTargets[0] ?? nearTargets[1];
            }
            else
                targetPos = enemys[0].transform;

            Vector3 lookPoint = new Vector3(targetPos.position.x,
                                            playerPos.position.y,
                                            targetPos.position.z);
            playerPos.LookAt(lookPoint);
        }
        else
            pMovement.Anim.SetBool("IsAttack", false);
    }

    public void Attack()
    {
        if (enemys.Count <= 0)
        {
            pMovement.Anim.SetBool("IsAttack", false);
            return;
        }

        if (PlayerData.Instance.PlayerSkill((int)SkillName.FRONTARROW) > 0)
            DoublePotato();
        else
        {
            Potato();
        }


        if (PlayerData.Instance.PlayerSkill((int)SkillName.MULTISHOT) > 0)
            Invoke("MultiShot", 0.2f);

        if (PlayerData.Instance.PlayerSkill((int)SkillName.DIAGNALARROW) > 0)
        {
            DiagonalPotato();
        }
    }


    void MultiShot()
    {
        if(PlayerData.Instance.PlayerSkill((int)SkillName.FRONTARROW) > 0)
        {
            DoublePotato();
        }
        else
        {
            Weapon potato = Potato();
        }

        //Instantiate(PlayerData.Instance.weaponPrefabs[PlayerData.Instance.PlayerSkill(2)], attackPosition.position, attackPosition.rotation);

        //if (PlayerData.Instance.PlayerSkill(3) > 0)
        //{
        //    DiagonalPotato();
        //}
    }

    void DiagonalPotato()
    {
        Quaternion rotation;

        rotation = Quaternion.Euler(attackPosition.transform.eulerAngles + new Vector3(0, -30f, 0));
        Weapon potatoL = Potato(rotation);

        rotation = Quaternion.Euler(attackPosition.transform.eulerAngles + new Vector3(0, 30f, 0));
        Weapon potatoR = Potato(rotation);
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        for (int i = 0; i < enemys.Count; i++)
        {
            GameObject go = enemys[i];
            Gizmos.DrawLine(transform.position, go.transform.position);
        }

        if (targetPos != null)
            Gizmos.DrawWireSphere(targetPos.position, 1f);
    }
#endif
}
