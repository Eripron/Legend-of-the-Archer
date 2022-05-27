using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RetryUI : MonoBehaviour
{
    [SerializeField] GameObject gameClearText;
    [SerializeField] GameObject gameOverText;

    private void OnEnable()
    {
        if (StageManager.Instance.IsGameClear)
            gameClearText.SetActive(true);
        else
            gameOverText.SetActive(true);
    }


    public void ClickRetryButton()
    {
        SceneManager.LoadScene(1);
    }



}
