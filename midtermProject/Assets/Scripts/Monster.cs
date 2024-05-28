using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Monster : MonoBehaviour
{
    public float atbTime;
    private float nowTime;

    public float maxHp;
    public float nowHp;

    private int score;

    public Slider atbSlider;
    public Slider hpSlider;

    public PlayerBattle player;

    private void Start()
    {
        nowTime = 0f;
        hpSlider.value = nowHp / maxHp;
        score = PlayerPrefs.GetInt("Score");
    }

    private void Update()
    {
        nowTime += Time.deltaTime;
        atbSlider.value = nowTime / atbTime;

        if (nowTime >= atbTime)
        {
            Debug.Log("Monster's Attack!");
            player.nowHp -= 10;
            player.hpSlider.value = player.nowHp / player.maxHp;
            nowTime = 0f;
        }

        if (nowHp <= 0)
        {
            MonsterDead();
        }
    }

    public void MonsterDead()
    {
        gameObject.SetActive(false);
        Destroy(atbSlider.gameObject);
        Destroy(player.atbSlider.gameObject);
        score++;

        PlayerPrefs.SetInt("Score", score);
        PlayerPrefs.SetFloat("PlayerHp", player.nowHp);
    }

}
