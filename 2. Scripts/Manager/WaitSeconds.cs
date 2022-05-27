using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitSeconds : Singleton<WaitSeconds>
{
    List<WaitForSeconds> delay;

    [SerializeField] int maxTime = 5;

    WaitForSeconds defaultValue = new WaitForSeconds(0f);

    private new void Awake()
    {
        base.Awake();
        delay = new List<WaitForSeconds>();

        float time = 0.1f;
        for (int i = 0; i <= maxTime * 10; i++)
            delay.Add(new WaitForSeconds(time * i));

    }

    public WaitForSeconds this[float time]
    {
        get
        {
            int index = (int)(time * 10);

            if(delay[index] != null)
                return delay[index];
            else
                return defaultValue;
        }
        private set { }
    }


}
