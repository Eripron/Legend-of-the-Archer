using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPenguin : EnemyFSM
{
    LineRenderer lr;

    [SerializeField] protected LayerMask layerMask;
    [SerializeField] protected GameObject bulletPrefab;
    [SerializeField] protected GameObject shootPoint;

    bool isChasePlayer = true;

    new void Start()
    {
        base.Start();

        lr = GetComponentInChildren<LineRenderer>();
        lr.startWidth = 0.2f;
        lr.endWidth = 0.2f;
    }

    protected override IEnumerator Attack()
    {
        StartCoroutine(CoChasePlayer());
        yield return WaitSeconds.Instance[beforeAtkTime];

        Shoot();

        currentState = State.Idel;
    }


    IEnumerator CoChasePlayer()
    {
        isChasePlayer = true;
        while (true)
        {
            yield return null;
            if (!isChasePlayer)
                break;

            model.LookAt(Player.transform);
            DrawWarningLine();
        }
    }

    void DrawWarningLine()
    {
        lr.positionCount = 5;

        Vector3 point = model.position + Vector3.up * 0.2f;     // line vertex
        Vector3 dir = model.forward;
        lr.SetPosition(0, point);

        for (int i = 1; i < lr.positionCount; i++)
        {
            Physics.Raycast(point, dir, out RaycastHit hit, 30f, (int)Layer.Wall);
            point = hit.point;
            lr.SetPosition(i, point);
            dir = Vector3.Reflect(dir, hit.normal);
        }
    }
    void EraseWarningLine()
    {
        for(int i=0; i<lr.positionCount; i++)
        {
            lr.SetPosition(i, Vector3.zero);
        }
        lr.positionCount = 0;
    }

    void Shoot()
    {
        canAtk = false;
        isChasePlayer = false;

        Instantiate(bulletPrefab, shootPoint.transform.position, model.rotation);

        EraseWarningLine();
    }
}
