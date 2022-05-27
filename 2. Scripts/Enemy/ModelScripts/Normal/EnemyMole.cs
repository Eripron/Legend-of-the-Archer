using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMole : EnemyFSM
{
    [SerializeField] protected GameObject dangerLine;
    [SerializeField] protected GameObject boltPrefeb;

    [SerializeField] protected Transform boltPosition;

    new void Start()
    {
        base.Start();
    }


    protected override IEnumerator Attack()
    {
        model.LookAt(Player.transform);

        MakeDangerLine();
        yield return WaitSeconds.Instance[beforeAtkTime];

        Shoot();

        canAtk = false;

        yield return WaitSeconds.Instance[2f];

        currentState = State.Idel;
    }


    void MakeDangerLine()
    {
        Vector3 startPos = model.position;
        startPos.y = 0.2f;

        Physics.Raycast(startPos, model.forward, out RaycastHit hit, 30f, (int)Layer.Wall);

        if (hit.transform == null)
            return;

        if (hit.transform.CompareTag("Wall"))
        {
            GameObject clone = Instantiate(dangerLine, startPos, model.rotation);

            if (clone.TryGetComponent<DangerLine>(out DangerLine dl))
            {
                dl.EndPosition = hit.point;
            }
        }
    }

    void Shoot()
    {
        GameObject bolt = Instantiate(boltPrefeb, boltPosition.position, model.rotation);
        bolt.GetComponent<Rigidbody>().AddForce(bolt.transform.forward, ForceMode.Impulse);

        Destroy(bolt, 2f);
    }

}
