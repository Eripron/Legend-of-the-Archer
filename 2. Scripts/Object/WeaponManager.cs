using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [SerializeField] Weapon weaponPrefab;
    [SerializeField] int initCount = 30;

    Stack<Weapon> foolObject;

    void Start()
    {
        foolObject = new Stack<Weapon>();
        Init();
    }

    void Init()
    {
        for(int i=0; i<initCount; i++)
            CreateWeapon();
    }


    void CreateWeapon()
    {
        Weapon weapon = Instantiate(weaponPrefab, this.transform);
        weapon.gameObject.SetActive(false);
        weapon.SetDelFun(OnBackToStack);

        foolObject.Push(weapon);
    }

    public Weapon GetFoolObject(Vector3 position, Quaternion rotation)
    {
        if (foolObject.Count <= 0)
            CreateWeapon();

        foolObject.Peek().transform.position = position;
        foolObject.Peek().transform.rotation = rotation;
        foolObject.Peek().gameObject.SetActive(true);

        return foolObject.Pop();
    }

    void OnBackToStack(Weapon weapon)
    {
        if(!foolObject.Contains(weapon))
        {
            weapon.transform.position = transform.position;
            weapon.gameObject.SetActive(false);
            foolObject.Push(weapon);
        }
    }

}
