using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyAttack : MonoBehaviour
{
    [SerializeField] EnemyBase enemy;

    private void OnTriggerEnter(Collider other)
    {
        PlayerData.Instance.PlayerDamaged((int)enemy.Damage);
        this.gameObject.SetActive(false);
    }
}
