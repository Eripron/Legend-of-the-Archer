using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Door : MonoBehaviour
{
    [SerializeField] GameObject lightEffect;

    [SerializeField] GameObject closeDoor;
    [SerializeField] GameObject openDoor;

    [SerializeField] Text stageNumText;

    private void Start()
    {
        SetActiveDoor(true);
    }

    public void SetActiveDoor(bool value)
    {
        lightEffect.SetActive(value);
        openDoor.SetActive(value);
        closeDoor.SetActive(!value);

        if(StageManager.Instance.CurStageNum > 0)
        {
            stageNumText.text = StageManager.Instance.CurStageNum.ToString();
        }
    }

}
