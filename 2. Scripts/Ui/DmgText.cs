using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DmgText : MonoBehaviour
{
    [SerializeField] TextMesh tm;

    public System.Action<DmgText> backFun;

     void Start()
    {
        gameObject.SetActive(false);    
    }

    void Update()
    {
        transform.Translate(Vector3.up * Time.deltaTime * 2f);
    }

    public void OnDamageText(float dmg)
    {
        tm.text = $"-{dmg}";
        gameObject.SetActive(true);
        Invoke("OffText", 0.5f);
    }

    void OffText()
    {
        backFun?.Invoke(this);
        gameObject.SetActive(false);
    }

}
