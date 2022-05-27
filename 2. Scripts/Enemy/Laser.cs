using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] BoxCollider col;
    int damage;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            PlayerData.Instance.PlayerDamaged(damage);
    }


    public void SetPosition(float size, int damage)
    {
        this.damage = damage;

        float _size = size * 10;
        transform.localScale = new Vector3(0.1f, 0.1f, _size);
        transform.localPosition = new Vector3(0f, 3.5f, _size / 2);
    }

}
