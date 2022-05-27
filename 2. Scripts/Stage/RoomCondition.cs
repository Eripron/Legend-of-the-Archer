using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomCondition : MonoBehaviour
{
    public List<GameObject> enemys = new List<GameObject>();


    bool isPlayerInRoom = false;
    Transform tfPlayer;

    public bool IsPlayerInRoom => isPlayerInRoom;

    public Transform TFPlayer => tfPlayer;

    Transform nextRoomPosition;

    public bool IsRoomClear => enemys.Count <= 0;



    void Start()
    {
        nextRoomPosition = transform.GetChild(3);
        StartCoroutine(CheckDoorState());
    }


    WaitForSeconds wait = new WaitForSeconds(0.2f);
    IEnumerator CheckDoorState()
    {
        // �÷��̾ �ְ� ���� ���ٸ� ���� 
        while (!isPlayerInRoom)
            yield return wait;

        while (!IsRoomClear)
        {
            yield return wait;
        }


        if (StageManager.Instance != null)
            StageManager.Instance.door.SetActiveDoor(true);

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            TargetingController player;
            if(other.TryGetComponent<TargetingController>(out player))
                player.OnInitEnemyInfo(enemys);

            isPlayerInRoom = true;
            tfPlayer = player.transform;

            if (StageManager.Instance != null)
            {
                StageManager.Instance.SetDoor(nextRoomPosition);
            }
        }
        else if(other.CompareTag("Monster"))
        {
            if(!enemys.Contains(other.gameObject))
                enemys.Add(other.gameObject);

            //other.GetComponent<EnemyBase>().SetRoomCondition(this);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            isPlayerInRoom = false;
            gameObject.SetActive(false);
        }
    }

}
