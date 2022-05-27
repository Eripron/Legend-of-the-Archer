using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotMachineManager : MonoBehaviour
{
    [SerializeField] Transform[] slotLines;
    [SerializeField] Button[] slotButton;

    SkillManager sm;

    [System.Serializable]
    public class DisplayItemSlot
    {
        public List<Image> slotSprite = new List<Image>();
    }
    [SerializeField] DisplayItemSlot[] displayItemSlots;

    List<int> startList = new List<int>();
    List<int> resultIndexList = new List<int>();

    List<Vector3> slotPosition = null;

    int itemCount = 3;


    private void OnEnable()
    {
        if (sm == null)
        {
            sm = SkillManager.Instance;
            slotPosition = new List<Vector3>();
            for (int i = 0; i < slotLines.Length; i++)
                slotPosition.Add(slotLines[i].localPosition);
        }

        //UiController.Instance.OnResetJoyStick();
        InitPos();
        StartSlot();
    }

    void StartSlot()
    {
        // skill list index 추가 
        for (int i = 0; i < sm.SkillList.Count; i++)
            startList.Add(i);


        for (int i = 0; i < slotButton.Length; i++)
        {
            slotButton[i].interactable = false;
            for (int j = 0; j < itemCount; j++)
            {
                // get start list random index
                int randomIndex = Random.Range(0, startList.Count);
                if (i == 0 && j == 1 || i == 1 && j == 0 || i == 2 && j == 2)
                    resultIndexList.Add(startList[randomIndex]);

                displayItemSlots[i].slotSprite[j].sprite = sm.SkillList[startList[randomIndex]].Icon;
                if (j == 0)
                    displayItemSlots[i].slotSprite[itemCount].sprite = sm.SkillList[startList[randomIndex]].Icon;

                startList.RemoveAt(randomIndex);
            }
        }

        for (int i = 0; i < slotButton.Length; i++)
            StartCoroutine(CoRotateSlot(i));

        UiController.Instance.PasueGame();
    }

    WaitForSeconds wait20 = new WaitForSeconds(0.02f);

    WaitForSecondsRealtime realTime = new WaitForSecondsRealtime(0);

    int[] answer = { 2, 3, 1 };
    IEnumerator CoRotateSlot(int slotIndex)
    {
        for(int i=0; i< (itemCount * (3 + slotIndex * 3) + answer[slotIndex]) * 2; i++)
        {
            if (slotLines[slotIndex].transform.localPosition.y <= 0)
                slotLines[slotIndex].transform.localPosition += new Vector3(0, 750f, 0);

            slotLines[slotIndex].transform.localPosition -= new Vector3(0, 125f, 0);
            yield return realTime;
        }

        for (int i = 0; i < itemCount; i++)
            slotButton[i].interactable = true;
    }

    public void ClickButton(int index)
    {
        // 결과 
        sm.GetSkill(resultIndexList[index]);
        this.gameObject.SetActive(false);
        UiController.Instance.ResumeGame();
    }


    void InitPos()
    {
        startList.Clear();
        resultIndexList.Clear();

        if (slotPosition != null && slotPosition.Count > 0)
        {
            for (int i = 0; i < slotPosition.Count; i++)
            {
                slotLines[i].localPosition = slotPosition[i];
            }
        }
    }
}
