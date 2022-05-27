using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class JoyStick : MonoBehaviour
{
    private static JoyStick instance;

    public static JoyStick Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<JoyStick>();
                if(instance == null)
                {
                    GameObject instanceContainer = new GameObject("JoyStick");
                    instance = instanceContainer.AddComponent<JoyStick>();
                }
            }
            return instance;
        }
    }

    [SerializeField] PlayerMovement player;

    // 스틱 배경과 스틱의 GameObject
    [Header("Stick UI")]
    [SerializeField] GameObject stickBackground;
    [SerializeField] GameObject joyStick;

    Vector3 originPos;                              // 스틱의 기본 위치
    Vector3 curStickPos;                            // 터치 후 스틱의 위치 
    [HideInInspector] public Vector3 joyStickDir;   // 스틱 방향

    float stickRadius;                              // 조이스틱 범위 반지름


    void Start()
    {
        originPos = stickBackground.transform.position;
        stickRadius = stickBackground.gameObject.GetComponent<RectTransform>().sizeDelta.y / 2;
    }


    public void PointDown()
    {
        if (UiController.IsGamePause)
            return;

        stickBackground.transform.position = Input.mousePosition;
        curStickPos = Input.mousePosition;
    }

    public void Drag(BaseEventData baseEventData)
    {
        if (UiController.IsGamePause)
        {
            EndDrag();
            return;
        }

        PointerEventData pointerEventData = baseEventData as PointerEventData;

        Vector3 DragPosition = pointerEventData.position;
        joyStickDir = (DragPosition - curStickPos).normalized;

        float distance = Vector3.Distance(DragPosition, curStickPos);

        if(distance < stickRadius)
            joyStick.transform.position = curStickPos + joyStickDir * distance;
        else
        {
            joyStick.transform.position = curStickPos + joyStickDir * stickRadius;
            distance = stickRadius;
        }

        float ratio = distance / stickRadius;
        joyStickDir *= ratio;

        player.Rotate();
    }

    public void EndDrag()
    {
        stickBackground.transform.position = originPos;
        joyStick.transform.position = originPos;

        joyStickDir = Vector3.zero;

        if (TargetingController.Instance == null)
            Debug.Log("null tergating controller");
        else
            TargetingController.Instance.TargetingEnemy();
    }

    private void OnDisable()
    {
        EndDrag();
    }
}
