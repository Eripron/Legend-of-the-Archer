using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectSet : MonoBehaviour
{
    private static EffectSet instance;

    public static EffectSet Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<EffectSet>();
                if (instance == null)
                {
                    GameObject newObject = new GameObject("EffectSetGO");
                    instance = newObject.AddComponent<EffectSet>();
                }
            }
            return instance;
        }
    }

    [Header("Dmg Text")]
    [SerializeField] DmgText damageText;
    [SerializeField] GameObject textParent;
    [SerializeField] int initCount = 10;


    Stack<DmgText> dmgTexts = new Stack<DmgText>();

    void Start()
    {
        for(int i=0; i<initCount; i++)
        {
            DmgText text = Instantiate(damageText, textParent.transform);
            text.backFun = BackText;
            dmgTexts.Push(text);
        }
    }


    public DmgText GetDmgText()
    {
        if (dmgTexts.Count <= 0)
        {
            DmgText text = Instantiate(damageText, transform);
            dmgTexts.Push(text);
        }

        return dmgTexts.Pop();
    }

    void BackText(DmgText text)
    {
        dmgTexts.Push(text);
    }

}
