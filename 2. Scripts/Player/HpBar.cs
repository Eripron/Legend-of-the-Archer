using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpBar : MonoBehaviour
{
    [SerializeField] Transform hpBarPostion;


    [SerializeField] Slider slider;
    [SerializeField] GameObject[] hpLines;
    [SerializeField] Text hpText;


    HorizontalLayoutGroup layoutGroup;
    Transform _transform;


    const float HP_UNIT = 1000;
    bool isStart = false;


    private void Update()
    {
        _transform.position = hpBarPostion.position;
    }


    public void UpdateHpBar()
    {
        if(!isStart)
        {
            isStart = true;
            layoutGroup = GetComponentInChildren<HorizontalLayoutGroup>();
            _transform = this.transform;
        }


        slider.value = (float)PlayerData.Instance.Hp / PlayerData.Instance.MaxHp;        // temp

        float scale = HP_UNIT / PlayerData.Instance.MaxHp;

        for (int i = 0; i < hpLines.Length; i++)
        {
            layoutGroup.gameObject.SetActive(false);
            hpLines[i].transform.localScale = new Vector3(scale, 1f, 1f);
            layoutGroup.gameObject.SetActive(true);
        }

        hpText.text = PlayerData.Instance.Hp.ToString();
    }

}
