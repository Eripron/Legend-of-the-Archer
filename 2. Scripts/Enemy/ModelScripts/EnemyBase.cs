using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public enum Layer
{
    None = 0,
    Player = 1 << 6,
    Wall = 1 << 8,
}

// Enemy Base
public class EnemyBase : MonoBehaviour
{
    [SerializeField] bool isBoss = false;

    // temp 
    [Header("State")]
    [SerializeField] protected float damage;                    // 데미지
    [SerializeField] protected float moveSpeed;                 // 이동 속도 
    [SerializeField] protected float attackRange;            // 공격 사정거리 
    [SerializeField] protected float attackCoolTime;         // 공격 쿨타임 
    [SerializeField] protected float beforeAtkTime = 1f;
    [SerializeField] protected int haveMaxExpCount = 4;

    protected float attackCoolTimeCacl;     // 공격 쿨타임 계산

    protected float maxHp;
    private float currentHp;

    [Header("Model")]
    [SerializeField] protected Transform model;      // monster model
    [SerializeField] protected Image hpImage;         // 일반 몬스터만
    //Canvas canvas;


    //[SerializeField] protected float playerRealizeRange;     // 인지 거리 
    //--------------

    protected bool canAtk = true;           // 공격 가능 bool check

    protected GameObject Player;
    protected RoomCondition roomCondition;
    protected NavMeshAgent nav;
    protected Animator anim;
    protected Rigidbody rb;

    protected float distance;

    public float Damage => damage;
    public float Hp
    {
        get
        {
            return currentHp;
        }
        set
        {
            currentHp = Mathf.Clamp(value, 0, maxHp);
            SetHpImage();

            if (currentHp <= 0)
                Dead();

            if(isBoss)
                UiController.Instance.SetBossHpBar(Hp, maxHp);
        }
    }


    protected void Start()
    {
        Player  = GameObject.FindGameObjectWithTag("Player");
        nav     = GetComponentInChildren<NavMeshAgent>();
        rb      = GetComponentInChildren<Rigidbody>();
        anim    = GetComponentInChildren<Animator>();
        //canvas  = GetComponentInChildren<Canvas>();

        roomCondition = transform.parent.parent.GetComponent<RoomCondition>();

        StartCoroutine(CalCoolTime());
    }

    Vector3 s;
    Vector3 e;

    // front player check
    protected bool IsCanAttcak()
    {
        Vector3 start = new Vector3(model.position.x, 1.5f, model.position.z);
        Vector3 dir = (Player.transform.position - model.position).normalized;

        int layer = (int)(Layer.Player | Layer.Wall);   // (int)Layer.Player
        Physics.Raycast(start, dir, out RaycastHit hit, 30f, layer);
        distance = Vector3.Distance(Player.transform.position, model.position);

        if (hit.transform == null)
            return false;

        if (hit.transform.CompareTag("Player") && distance <= attackRange)
            return true;
        else 
            return false;
    }

    protected virtual IEnumerator CalCoolTime()
    {
        while(true)
        {
            yield return null;
            if(!canAtk)
            {
                attackCoolTimeCacl -= Time.deltaTime;
                if(attackCoolTimeCacl <= 0)
                {
                    attackCoolTimeCacl = attackCoolTime;
                    canAtk = true;
                }
            }
        }
    }

    void SetHpImage()
    {
        if(hpImage != null)
            hpImage.fillAmount = (float)currentHp / maxHp;
    }

    public virtual void Damaged(int dmg)
    {
        Hp -= dmg;

        DmgText text = EffectSet.Instance.GetDmgText();
        text.transform.position = model.transform.position + model.transform.up * 1.3f;
        text.OnDamageText(dmg);
    }

    protected virtual void Dead()
    {
        roomCondition.enemys.Remove(model.gameObject);
        if (!isBoss)
        {
            int random = Random.Range(1, haveMaxExpCount);
            for (int i = 0; i < random; i++)
            {
                GameObject exp = PlayerData.Instance.InstantExp(model.position);
                exp.transform.SetParent(transform.parent.parent);
            }
        }

        if (TargetingController.Instance != null)
            TargetingController.Instance.TargetingEnemy();

        Destroy(this.gameObject);
    }

}
