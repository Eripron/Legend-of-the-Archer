using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DangerLine : MonoBehaviour
{
    /*
     ���߿� danager line�� �����ϴ� ���� �ƴ� ���� ���� �ɷ� change 
     monster ���� trail �ð� ������ ����ϸ� �� 
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
