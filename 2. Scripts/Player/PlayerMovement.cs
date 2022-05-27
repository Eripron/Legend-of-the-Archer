using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //[SerializeField] float moveSpeed;
    Rigidbody rigid;
    Animator anim;

    public Animator Anim => anim;

    UiController uc;

    void Start()
    {
        rigid = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();

        if (uc == null)
            uc = UiController.Instance;
    }


    private void FixedUpdate()
    {
        Move();
    }


    void Move()
    {
        Vector3 moveVel = new Vector3(JoyStick.Instance.joyStickDir.x * PlayerData.Instance.MoveSpeed,
                                      rigid.velocity.y,
                                      JoyStick.Instance.joyStickDir.y * PlayerData.Instance.MoveSpeed);
        rigid.velocity = moveVel;


        if(moveVel == Vector3.zero)     // stop
            anim.SetBool("IsRun", false);
        else  // run
        {
            anim.SetBool("IsRun", true);
            anim.SetBool("IsAttack", false);
        }
    }
    public void Rotate()    // stick 방향으로 회전
    {
        Vector3 lookPoint = new Vector3(transform.position.x + JoyStick.Instance.joyStickDir.x,
                                transform.position.y,
                                transform.position.z + JoyStick.Instance.joyStickDir.y);

        transform.LookAt(lookPoint);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("NextStage"))
        {
            StageManager.Instance.NextStage();
        }

        if (TargetingController.Instance.Enemys.Count <= 0 && other.gameObject.CompareTag("Exp"))
        {
            PlayerData.Instance.GetExp(10f);
            Destroy(other.gameObject.transform.parent.gameObject);
        }
    }

    public void DeadAnimation()
    {
        rigid.velocity = Vector3.zero;
        anim.SetTrigger("OnDead");
    }
}
