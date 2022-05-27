using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSkiilBox : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            BoxCollider col = GetComponent<BoxCollider>();
            col.isTrigger = true;

            UiController.Instance.OnRuletteUI();
        }
    }


}
