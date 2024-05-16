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

    public float maxHp;
    public float nowHp;

    public Slider atbSlider;
    public Slider hpSlider;
    public GameObject battleGUI;

    private GameObject monster;

    private void Start()
    {
        BattleState = BattleState.Idle;
        nowTime = 0f;
        hpSlider.value = nowHp / maxHp;
    }

    private void Update()
    {
        nowTime += Time.deltaTime;
        atbSlider.value = nowTime / atbTime;
        

        if (nowTime >= atbTime)
        {
            BattlePanel();
            BattleState = BattleState.Idle;
        }
    }

    private void BattlePanel()
    {
        battleGUI.SetActive(true);

        if (Input.anyKeyDown)
        {
            BattleState = BattleState.Hit;
            Debug.Log("Player's Attack!");
            battleGUI.SetActive(false);
            nowTime = 0f;

            monster = GameObject.FindGameObjectWithTag("Monster");
            Monster m = monster.GetComponent<Monster>();
            m.nowHp -= 20f;
            m.hpSlider.value = m.nowHp / m.maxHp;
        }

    }
}
