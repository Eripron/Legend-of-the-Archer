using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Skill
{
    public static int count = 0;

    int index;
    int level;

    public string name;

    Sprite icon;

    System.Action DelEffect;


    public Sprite Icon => icon;


    private Skill() { }
    public Skill(string _name, int _level, Sprite _icon, System.Action effect)
    {
        index = count++;

        name = _name;
        level = _level;
        icon = _icon;
        DelEffect = effect;
    }

    public void Effect()
    {
        DelEffect?.Invoke();
    }

}
