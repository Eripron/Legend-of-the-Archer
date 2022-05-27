using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFlower : EnemyFSM
{
    [SerializeField] GameObject flowerBall;
    [SerializeField] Transform attackPoint;

    new void Start()
    {
        base.Start();
    }

    protected override IEnumerator Attack()
    {
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
            anim.SetTrigger("Attack");

        yield return WaitSeconds.Instance[0.2f];

        StartCoroutine(ThrowBullet());

        currentState = State.Idel;
    }


    private IEnumerator ThrowBullet()
    {
        canAtk = false;

        model.LookAt(Player.transform);
        int bulletCount = 3;

        for(int i=0; i<bulletCount; i++)
        {
            GameObject bullet = Instantiate(flowerBall, attackPoint.position, model.rotation);
            int random = Random.Range(-20, 20);
            bullet.transform.rotation = Quaternion.Euler(bullet.transform.eulerAngles 
                                                         + new Vector3(0, random, 0));

            bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * 7f;

            yield return WaitSeconds.Instance[0.2f];
        }
    }


}
