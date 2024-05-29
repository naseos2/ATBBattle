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

    public Animator monsterTransition;

    public Slider atbSlider;
    public Slider hpSlider;

    public PlayerBattle player;

    public GameObject attackEffect;
    private GameObject ae;

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

            Vector3 pPosition = player.transform.position;
            ae = Instantiate(attackEffect, pPosition, Quaternion.identity);
            Destroy(ae, 0.4f);

            StartCoroutine(WalkMotion());
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
        PlayerPrefs.SetFloat("PlayerMp", player.nowMp);
    }

    IEnumerator WalkMotion()
    {
        monsterTransition.SetBool("BattleWalk", true);

        transform.position = Vector2.MoveTowards(transform.position, new Vector2(-2, -0.09f), 1);

        yield return new WaitForSeconds(1f);

        monsterTransition.SetBool("BattleWalk", false);
        transform.position = Vector2.MoveTowards(transform.position, new Vector2(-4, -0.09f), 1);
    }

}
