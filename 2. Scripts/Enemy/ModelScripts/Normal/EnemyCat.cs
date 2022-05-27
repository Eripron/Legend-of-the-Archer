using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCat : EnemyFSM
{
    [SerializeField] Laser laserGO;

    [SerializeField] float laserTime = 3.5f;



    LineRenderer[] lineRenders;



    bool isChase = true;


    new void Start()
    {
        base.Start();

        lineRenders = laserGO.GetComponentsInChildren<LineRenderer>();
        laserGO.gameObject.SetActive(false);
    }

    protected override IEnumerator Attack()
    {
        laserGO.gameObject.SetActive(true);
        isChase = true;
        StartCoroutine(CoChasePlayer());

        yield return WaitSeconds.Instance[beforeAtkTime];

        FireLaser();

        yield return WaitSeconds.Instance[laserTime];
        currentState = State.Idel;
    }


    IEnumerator CoChasePlayer()
    {
        while(isChase)
        {
            yield return null;
            model.LookAt(Player.transform);
            DrawLaserLine();
        }
    }

    //float laserSize = 0f;
    void DrawLaserLine()
    {
        Vector3 startPoint = model.position + Vector3.up * 0.5f;
        Vector3 dir = model.forward;

        Physics.Raycast(startPoint, dir, out RaycastHit hit, 30f, 1<<8);

        for (int i = 0; i < lineRenders.Length; i++)
        {
            LineRenderer lr = lineRenders[i];
            lr.positionCount = 2;
            lr.SetPosition(0, startPoint);
            lr.SetPosition(1, hit.point);
        }

    }


    void FireLaser()
    {
        isChase = false;
        canAtk = false;

        float size = Vector3.Distance(lineRenders[0].GetPosition(0), lineRenders[0].GetPosition(1));
        laserGO.SetPosition(size, (int)damage);

        //StartCoroutine(PlayerCheckInLaser());
        StartCoroutine(LaserEffectOff());
    }


    // CPU 30 ~ 40 % 차지 
    IEnumerator PlayerCheckInLaser()
    {
        yield return WaitSeconds.Instance[0.3f];

        Vector3 start = model.position + Vector3.up * 0.5f;
        Vector3 end = model.position + Vector3.up * 0.5f + model.forward * 20;

        float time = laserTime;
        while (time > 0f)
        {
            Debug.Log("Laset Check");
            if (Physics.Linecast(start, end, (int)Layer.Player))
            {
                Debug.Log("player in laset");
                PlayerData.Instance.PlayerDamaged((int)damage);
            }
            // 문제점 
            //if (Physics.Raycast(model.position + Vector3.up * 0.5f, model.forward, out RaycastHit hit, 10f, (int)Layer.Player))
            //{
            //    if (hit.transform.CompareTag("Player"))
            //    {
            //        PlayerData.Instance.PlayerDamaged((int)damage);
            //    }
            //}
            yield return WaitSeconds.Instance[0.4f];
            time -= 0.4f;
        }
    }

    IEnumerator LaserEffectOff()
    {
        while (true)
        {
            yield return null;
            if (canAtk)
            {
                laserGO.gameObject.SetActive(false);
                break;
            }
        }
    }
}
