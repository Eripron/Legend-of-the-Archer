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

    // ��ƽ ���� ��ƽ�� GameObject
    [Header("Stick UI")]
    [SerializeField] GameObject stickBackground;
    [SerializeField] GameObject joyStick;

    Vector3 originPos;                              // ��ƽ�� �⺻ ��ġ
    Vector3 curStickPos;                            // ��ġ �� ��ƽ�� ��ġ 
    [HideInInspector] public Vector3 joyStickDir;   // ��ƽ ����

    float stickRadius;                              // ���̽�ƽ ���� ������


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
