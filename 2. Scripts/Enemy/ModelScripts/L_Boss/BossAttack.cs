using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttack : MonoBehaviour
{
    [SerializeField] Boss boss;

    public void Attack1()
    {
        boss.Attack01();
    }

    public void Attack2()
    {
        boss.Attack02();
    }

}
