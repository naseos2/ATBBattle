using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum BattleState
{
    Idle,
    Hit,
    Magic,
    Item,
    Run,
}

public class PlayerBattle : MonoBehaviour
{
    public BattleState BattleState;

    public float atbTime;
    private float nowTime;

    public Slider atbSlider;
    public GameObject battleGUI;

    private void Start()
    {
        BattleState = BattleState.Idle;
        nowTime = 0f;
    }

    private void Update()
    {
        nowTime += Time.deltaTime;
        atbSlider.value = nowTime / atbTime;

        if (nowTime >= atbTime)
        {
            BattlePanel();

        }
    }

    private void BattlePanel()
    {
        battleGUI.SetActive(true);

        if (Input.anyKeyDown)
        {
            Debug.Log("Player's Attack!");
            battleGUI.SetActive(false);
            nowTime = 0f;
        }

    }
}
