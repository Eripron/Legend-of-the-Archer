using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] int dmg;

    [SerializeField] bool isRemain = false;

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.CompareTag("Player"))
        {
            PlayerData.Instance.PlayerDamaged(dmg);
            Disapear();
        }
        else if(collision.transform.CompareTag("Wall"))
        {
            Disapear();
        }
    }

    void Disapear()
    {
        if (isRemain)
            this.gameObject.SetActive(false);
        else
            Destroy(this.gameObject);
    }


}
