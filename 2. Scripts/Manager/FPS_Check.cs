using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPS_Check : MonoBehaviour
{
    [Range(1, 100)]
    public int fontSize;
    [Range(0, 1)]
    public float R, G, B;

    float deltaTime = 0.0f;

    private void Start()
    {
        fontSize = fontSize == 0 ? 50 : fontSize;
    }

    private void Update()
    {
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
    }

    float min = float.MaxValue, max = 0;
    List<float> l = new List<float>();

    private void OnGUI()
    {
        int w = Screen.width, h = Screen.height;

        GUIStyle style = new GUIStyle();
        Rect rect = new Rect(0, 0, w, h * 2 / 100);

        style.alignment = TextAnchor.UpperLeft;
        style.fontSize = h * 2 / fontSize;
        style.normal.textColor = new Color(R, G, B, 1f);

        float msec = deltaTime * 1000f;
        float fps = 1.0f / deltaTime;

        if (fps < min)
            min = fps;
        if (fps > max)
            max = fps;

        l.Add(fps);

        string text = $"{msec : 0.} ms ({fps : 0.}) fps";
        GUI.Label(rect, text, style);
    }

#if UNITY_EDITOR
    private void OnDisable()
    {
        Debug.Log($"--- FPS --- ");
        Debug.Log($"Min : {min} / Max : {max}");

        float sum = 0;
        for(int i=0; i<l.Count; i++)
        {
            sum += l[i];
        }

        Debug.Log($"ЦђБе : {sum / l.Count}");
    }
#endif
}
