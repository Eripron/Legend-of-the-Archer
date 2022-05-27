using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpBarMonster : MonoBehaviour
{
    [SerializeField] Transform pos;

    void Update()
    {
        transform.position = pos.position;     
    }
}
