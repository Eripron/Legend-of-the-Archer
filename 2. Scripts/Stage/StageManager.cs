using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : Singleton<StageManager>
{
    [System.Serializable]
    public class StartPointArray
    {
        public List<Transform> startPoint = new List<Transform>();
    }

    [SerializeField] Transform startPoint;
    [SerializeField] StartPointArray[] startPointNormal;
    [SerializeField] List<Transform> startPointAngle = new List<Transform>();
    [SerializeField] List<Transform> startPointMidBoss = new List<Transform>();
    [SerializeField] Transform startPointLastBoss;

    public Door door;

    GameObject player;

    const int LAST_STAGE = 20;
    int currentStage = 0;

    public int CurStageNum => currentStage;

    public bool IsGameClear => currentStage > LAST_STAGE;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        // test 차단 
        if (player != null)
            player.transform.position = startPoint.position;

        UiController.Instance.CheckBossRoom(false);
    }

    public void NextStage()
    {
        if (++currentStage > LAST_STAGE)
        {
            UiController.Instance.GameEnd();
            return;
        }

        Transform nextPoint;

        if(currentStage % 5 != 0)       //  normal room
        {
            int curArrIndex = currentStage / 10;
            int randomIndex = Random.Range(0, startPointNormal[curArrIndex].startPoint.Count);
            nextPoint = startPointNormal[curArrIndex].startPoint[randomIndex];
            startPointNormal[curArrIndex].startPoint.RemoveAt(randomIndex);
        }
        else  // boss or angle
        {
            if(currentStage % 10 == 5)  // angle
            {
                int randomIndex = Random.Range(0, startPointAngle.Count);
                nextPoint = startPointAngle[randomIndex];
            }
            else  // boss
            {
                if(currentStage == LAST_STAGE)
                {
                    nextPoint = startPointLastBoss;
                }
                else
                {
                    int randomIndex = Random.Range(0, startPointMidBoss.Count);
                    nextPoint = startPointMidBoss[randomIndex];
                    startPointMidBoss.RemoveAt(randomIndex);
                }
            }
        }

        if (nextPoint != null)
        {
            nextPoint.parent.gameObject.SetActive(true);
            player.transform.position = nextPoint.position;
            TargetingController.Instance.TargetingEnemy();
        }

        if (currentStage == LAST_STAGE)
            UiController.Instance.CheckBossRoom(true);
        else
            UiController.Instance.CheckBossRoom(false);

        // camera 위치 
        FollowCamera.Instance.NextStageCamera();
    }

    public void SetDoor(Transform doorPos)
    {
        if (doorPos == null)
            return;

        door.transform.position = doorPos.position;
        door.SetActiveDoor(false);
    }



}
