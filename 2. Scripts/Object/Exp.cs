using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exp : MonoBehaviour
{
    RoomCondition roomCondition;
    GameObject player;


    void Start()
    {
        roomCondition = GetComponentInParent<RoomCondition>();
        this.player = PlayerData.Instance.Player;

        StartCoroutine(CheckRoom());
    }


    IEnumerator CheckRoom()
    {
        while (!roomCondition.IsRoomClear)
            yield return new WaitForSeconds(0.2f);

        yield return new WaitForSeconds(0.4f);

        while (true)
        {
            transform.position = Vector3.Lerp(transform.position, (player.transform.position + Vector3.up * 0.3f), 0.2f);
            yield return null;
        }
    }

}
