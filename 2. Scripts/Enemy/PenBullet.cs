using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PenBullet : MonoBehaviour
{
    [SerializeField] int bounceCount = 3;
    [SerializeField] int dmg = 50;
    [SerializeField] Rigidbody rigid;

    Vector3 dir;
    [SerializeField] int speed = 0;


    void Start()
    {
        dir = transform.forward;
        rigid.velocity = dir * speed;
    }


    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Wall"))
        {
            if (bounceCount > 0)
            {
                bounceCount -= 1;

                dir = Vector3.Reflect(dir, collision.contacts[0].normal);
                rigid.velocity = dir * 10f;
            }
            else
                Destroy(gameObject, 0.1f);

        }
        else if(collision.gameObject.CompareTag("Player"))
        {
            PlayerData.Instance.PlayerDamaged(dmg);
            Destroy(gameObject);
        }

    }


}
