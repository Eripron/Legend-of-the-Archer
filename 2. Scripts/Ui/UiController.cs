using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UiController : Singleton<UiController>
{
    private static bool isPause = false;
    public static bool IsGamePause => isPause;


    [Header("GameObject")]
    [SerializeField] GameObject joyStickUI;
    [SerializeField] GameObject retryUI;
    [SerializeField] GameObject optionUI;

    [SerializeField] GameObject ruletteUI;
    [SerializeField] GameObject slotMachineUI;


    [Header("Level")]
    [SerializeField] Slider levelBar;
    [SerializeField] Text levelText;


    [Header("Boss Hp Bar")]
    [SerializeField] Slider bossHpBarBack;
    [SerializeField] Slider bossHpBar;


    public float MaxBossHp;
    public float CurBossHp;

    [SerializeField] bool isBossRoom = false;

    void Update()
    {
        if (isBossRoom)
            SetBossHpBar();
        else
            SetPlayerLevelBar();
    }

    public void SetPlayerLevelBar()
    {
        levelText.text = $"Lv. {PlayerData.Instance.Level}";
        levelBar.value = PlayerData.Instance.Exp / PlayerData.Instance.MaxExp;
    }

    private void SetBossHpBar()
    {
        bossHpBar.value = Mathf.Lerp(bossHpBar.value, CurBossHp / MaxBossHp, Time.deltaTime * 10f);

        if (bossHpBar.value != bossHpBarBack.value)
        {
            bossHpBarBack.value = Mathf.Lerp(bossHpBarBack.value, bossHpBar.value, Time.deltaTime * 5f);
        }
    }

    public void CheckBossRoom(bool isBossRoom)
    {
        this.isBossRoom = isBossRoom;

        levelBar.gameObject.SetActive(!isBossRoom);
        bossHpBar.gameObject.SetActive(isBossRoom);
        bossHpBarBack.gameObject.SetActive(isBossRoom);
    }

    

    public void PlayerDead()
    {
        joyStickUI.SetActive(false);
        GameEnd();
    }

    public void GameEnd()
    {
        retryUI.SetActive(true);
    }


    // Random Rulette & Slot
    public void OnRuletteUI()
    {
        ruletteUI.SetActive(true);
    }
    public void OnSlotMachineUI()
    {
        slotMachineUI.SetActive(true);
    }


    public void SetBossHpBar(float curHp, float maxHp)
    {
        CurBossHp = curHp;
        MaxBossHp = maxHp;
    }

    public void PasueGame()
    {
        //Debug.Log("Pause");

        isPause = true;
        Time.timeScale = 0;
    }
    public void ResumeGame()
    {
        //Debug.Log("Resume");

        isPause = false;
        Time.timeScale = 1f;
    }

    public void RestartGame()
    {
        ResumeGame();
        SceneManager.LoadScene(1);
    }
    public void ExitGame()
    {
        Application.Quit();
    }

    public void OnOptionWindow()
    {
        bool state = !optionUI.activeSelf;
        if (state)
            PasueGame();
        else
            ResumeGame();

        optionUI.SetActive(state);
    }
}
