using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SkillName
{
    RICOCHET            = 0,
    MULTISHOT           = 1,
    FRONTARROW          = 2,
    DIAGNALARROW        = 3,
    BOUNCYWALL          = 4,
}


public class PlayerData : Singleton<PlayerData>
{
    [Header("Object")]
    [SerializeField] GameObject player;
    [SerializeField] HpBar playerHpBar;


    [Header("Status")]
    private int curHp;
    [SerializeField] int level = 1;
    [SerializeField] int maxHp;
    [SerializeField] float power = 100;
    [SerializeField][Range(0, 10)] float attackSpeed = 1;
    [SerializeField][Range(0, 100)] float moveSpeed;

    bool isAlive = true;


    [Header("Skill")]
    [SerializeField] List<int> playerSkill = new List<int>();


    [Header("EXP")]
    [SerializeField] GameObject expPrefab;

    [SerializeField] float maxExp;
    private float curExp = 0;


    public int Hp
    {
        get
        {
            return curHp;
        }
        set
        {
            curHp = Mathf.Clamp(value, 0, maxHp);
            playerHpBar.UpdateHpBar();

            if (curHp <= 0)
            {
                if(player.TryGetComponent<PlayerMovement>(out PlayerMovement pm))
                {
                    if(isAlive)
                    {
                        isAlive = false;
                        UiController.Instance.PlayerDead();

                        pm.DeadAnimation();
                    }
                }
            }
        }
    }
    public int MaxHp
    {
        get
        {
            return maxHp;
        }
        set
        {
            maxHp = value;
        }
    }
    public float Damage
    {
        get
        {
            return power;
        }
        set
        {
            if(value <= 0)
            {
                power = 0;
                return;
            }

            power = value;
        }
    }
    public float AttackSpeed
    {
        get
        {
            return attackSpeed;
        }
        set
        {
            if (value >= 10)
                attackSpeed = 10;
            else
                attackSpeed = value;
        }
    }
    public float MoveSpeed
    {
        get
        {
            return moveSpeed;
        }
        set
        {
            if (value >= 10)
                moveSpeed = 10;
            else
                moveSpeed = value;
        }
    }


    public int Level => level;
    public float Exp => curExp;
    public float MaxExp => maxExp;


    public GameObject Player => player;
    public bool PlayerAlive => isAlive;



    void Start()
    {
        Hp = maxHp;
    }



    public int PlayerSkill(int index) => playerSkill[index];
    public GameObject InstantExp(Vector3 position)
    {
        GameObject go = Instantiate(expPrefab, position + Vector3.up * 1.4f, transform.rotation);
        return go;

    }

    public void GetExp(float exp)
    {
        curExp += exp;

        if (curExp >= maxExp)
        {
            level++;
            curExp -= maxExp;
            maxExp *= 1.2f;

            UiController.Instance.OnSlotMachineUI();
        }

        UiController.Instance.SetPlayerLevelBar();
    }
    public void PlayerDamaged(int dmg)
    {
        Hp = Mathf.Clamp(curHp - dmg, 0, maxHp);
    }

    public void GetSkill(SkillName skill)
    {
        playerSkill[(int)skill] = 1;
    }

}
