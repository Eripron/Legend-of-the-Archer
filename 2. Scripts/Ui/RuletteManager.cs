using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RuletteManager : MonoBehaviour
{
    [SerializeField] GameObject ruletteGO;
    [SerializeField] GameObject needleGO;

    [SerializeField] Image[] slotImages;

    SkillManager sm;

    const int slotCount = 6;

    Skill[] slotSkill = new Skill[6];
    List<int> selectedIndex = new List<int>();

    Transform ruletteTf;

    void OnEnable()
    {
        if (sm == null)
        {
            sm = SkillManager.Instance;
            ruletteTf = ruletteGO.transform;
        }

        InitRulette();
        //UiController.Instance.OnResetJoyStick();
        StartCoroutine(CoRotateRulette());

        UiController.Instance.PasueGame();
    }


    void InitRulette()
    {
        if (selectedIndex.Count > 0)
            selectedIndex.Clear();

        for (int i = 0; i < slotCount;)
        {
            int ranIndex = Random.Range(0, sm.SkillList.Count);
            if (!selectedIndex.Contains(ranIndex))
            {
                selectedIndex.Add(ranIndex);

                slotSkill[i] = sm.SkillList[ranIndex];
                slotImages[i].sprite = sm.SkillList[ranIndex].Icon;
                i++;
            }
        }
    }

    WaitForSecondsRealtime realTime = new WaitForSecondsRealtime(0.5f);
    IEnumerator CoRotateRulette()
    {
        float random = Random.Range(2f, 5f);
        float rotSpeed = -200f * random;

        while (true)
        {
            yield return null;
            if (rotSpeed >= -0.01f)
                break;

            rotSpeed = Mathf.Lerp(rotSpeed, 0, Time.unscaledDeltaTime * 2f);
            ruletteGO.transform.Rotate(0, 0, rotSpeed);
        }
        yield return realTime;

        RuletteResult();
    }

    void RuletteResult()
    {
        float minDistance = float.MaxValue;
        int resultIndex = -1;

        for (int i = 0; i < slotCount; i++)
        {
            float curDistance = Vector2.Distance(slotImages[i].transform.position, needleGO.transform.position);
            if (curDistance < minDistance)
            {
                minDistance = curDistance;
                resultIndex = i;
            }
        }

        StartCoroutine(ResultEffect(slotImages[resultIndex]));

        slotSkill[resultIndex].Effect();
    }
    IEnumerator ResultEffect(Image image)
    {
        float scale = 1f;
        while(scale < 1.8f)
        {
            scale += Time.unscaledDeltaTime;
            image.rectTransform.localScale = new Vector3(scale, scale, 1f);
            yield return null;
        }
        image.rectTransform.localScale = Vector3.one;

        yield return realTime;
        this.gameObject.SetActive(false);

        UiController.Instance.ResumeGame();
    }


}
