using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DangerLine : MonoBehaviour
{
    /*
     나중에 danager line을 생성하는 것이 아닌 꺼내 쓰는 걸로 change 
     monster 마다 trail 시간 설정도 고려하면 굳 
     */


    TrailRenderer tr;
    Vector3 endPosition;

    public Vector3 EndPosition
    {
        set
        {
            endPosition = value;
            StartCoroutine(MoveDengerLine());
        }
    }

    private void OnEnable()
    {
        tr = GetComponent<TrailRenderer>();

        tr.startColor = new Color(1, 0, 0, 0.7f);
        tr.endColor = new Color(1, 0, 0, 0.7f);

        tr.time = 1.5f;
    }

    IEnumerator MoveDengerLine()
    {
        while (Vector3.SqrMagnitude(endPosition - transform.position) > 0.1f)
        {
            yield return null;
            transform.position = Vector3.Lerp(transform.position, endPosition, Time.deltaTime * 3.5f);
        }

        Destroy(gameObject, tr.time);
    }

}
