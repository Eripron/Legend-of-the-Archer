using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillManager : MonoBehaviour
{
    private static SkillManager instance;
    public static SkillManager Instance
    {
        get
        {
            if(instance == null)
            {
                instance = FindObjectOfType<SkillManager>();
                if(instance == null)
                {
                    GameObject go = new GameObject("SkillManager");
                    instance = go.AddComponent<SkillManager>();
                }
            }
            return instance;
        }
    }


    [SerializeField] Sprite[] skillIcons;

    List<Skill> skillList = new List<Skill>();


    public List<Skill> SkillList
    {
        get
        {
            return skillList;
        }
    }

    void Start()
    {
        Generate();
    }

    // create skill data
    void Generate()
    {
        //skillList.Add(new Skill("", 0, skillIcons[], )); 

        skillList.Add(new Skill("Ricochet",          0, skillIcons[4],  Ricochet));      // 0 
        skillList.Add(new Skill("Multishot",         0, skillIcons[1],  MultiShot));     // 1
        skillList.Add(new Skill("Front Arrow",       0, skillIcons[2],  FrontArrow));    // 2
        skillList.Add(new Skill("Diagnal Arrows",    0, skillIcons[24], DiagnalArrow)); // 3
        skillList.Add(new Skill("Bouncy Walls",      0, skillIcons[25], BouncyWall));   // 4

        skillList.Add(new Skill("Power Up",         0, skillIcons[14], PowerUp));   
        skillList.Add(new Skill("Attack Speed Up",  0, skillIcons[16], AttackSpeedUp));   
        skillList.Add(new Skill("Speed Up",         0, skillIcons[7],  SpeedUp));   
        skillList.Add(new Skill("MaxHp Up",         0, skillIcons[27], MaxHpUp));   
        skillList.Add(new Skill("Recove Hp",        0, skillIcons[40], RecoveHp));  
    }


    public void GetSkill(int index)
    {
        skillList[index].Effect();
    }

    #region Skill Effect

    // skill effect
    void MultiShot()
    {
        PlayerData.Instance.GetSkill(SkillName.MULTISHOT);
    }
    void Ricochet()
    {
        PlayerData.Instance.GetSkill(SkillName.RICOCHET);
    }
    void DiagnalArrow()
    {
        PlayerData.Instance.GetSkill(SkillName.DIAGNALARROW);
    }
    void BouncyWall()
    {
        PlayerData.Instance.GetSkill(SkillName.BOUNCYWALL);
    }
    void FrontArrow()
    {
        PlayerData.Instance.GetSkill(SkillName.FRONTARROW);
    }


    void PowerUp()
    {
        PlayerData.Instance.Damage *= 1.5f;
    }
    void AttackSpeedUp()
    {
        PlayerData.Instance.AttackSpeed *= 1.5f;
    }
    void SpeedUp()
    {
        PlayerData.Instance.MoveSpeed *= 1.5f;
    }
    void RecoveHp()
    {
        PlayerData.Instance.Hp += (PlayerData.Instance.MaxHp / 3);
    }
    void MaxHpUp()
    {
        PlayerData.Instance.Hp += (PlayerData.Instance.MaxHp / 3);
        PlayerData.Instance.MaxHp += (int)(PlayerData.Instance.MaxHp * 1.3f);
    }
    #endregion

}
