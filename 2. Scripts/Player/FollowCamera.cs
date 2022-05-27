using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FollowCamera : Singleton<FollowCamera>
{
    [SerializeField] Transform target;
    [SerializeField] Vector3 offset;
    [SerializeField] Image fadeImage;


    Vector3 camPos;

    void Start()
    {
        camPos.x = target.position.x;
    }

    void LateUpdate()
    {
        if (camPos.x - target.position.x > 10f)
            camPos.x = target.position.x;

        camPos.y = target.position.y + offset.y;
        camPos.z = target.position.z + offset.z;

        transform.position = camPos;
    }

    public void NextStageCamera()
    {
        StartCoroutine(FadeInOut());
        camPos.x = target.position.x;
    }

    IEnumerator FadeInOut()
    {
        float a = 1;
        fadeImage.color = new Vector4(1, 1, 1, a);
        yield return new WaitForSeconds(0.3f);

        while(a >= 0)
        {
            fadeImage.color = new Vector4(1, 1, 1, a);
            a -= 0.02f;
            yield return null;
        }
        fadeImage.color = new Vector4(1, 1, 1, 0);
    }

}
