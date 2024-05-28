using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum BattleState
{
    Idle,
    Hit,
    Magic,
    Item,
    Run,
    Dead,
}

public class PlayerBattle : MonoBehaviour
{
    public BattleState BattleState;

    public float atbTime;
    private float nowTime;

    public float maxHp;
    public float nowHp;
    public float maxMp;
    public float nowMp;

    public Slider atbSlider;
    public Slider hpSlider;
    public Slider mpSlider;

    public GameObject menu;
    public GameObject battleMenu;
    private bool isMenu;

    private GameObject monster;

    public Animator transition;
    public float transitionTime = 1f;

    public Button battleBtn;
    public Button attackBtn;

    public Image gameoverBg;
    public Text gameoverTxt;

    private int score;
    public Text scoreTxt;
    public Text gameoverScoreTxt;

    private void Start()
    {
        monster = GameObject.FindGameObjectWithTag("Monster");
        nowHp = PlayerPrefs.GetFloat("PlayerHp");
        nowMp = PlayerPrefs.GetFloat("PlayerMp");

        score = PlayerPrefs.GetInt("Score");
        scoreTxt.text = $"Score : {score}";

        BattleState = BattleState.Idle;
        nowTime = 0f;
        hpSlider.value = nowHp / maxHp;
        mpSlider.value = nowMp / maxMp;

    }

    private void Update()
    {
        if (!monster.activeSelf)
        {
            score = PlayerPrefs.GetInt("Score");
            scoreTxt.text = $"Score : {score}";

            StartCoroutine(BacktoMain());
            return;
        }
        nowTime += Time.deltaTime;
        atbSlider.value = nowTime / atbTime;


        if (nowTime >= atbTime && isMenu == false)
        {
            BattlePanel();
            BattleState = BattleState.Idle;
        }

        if (menu.activeSelf)
        {
           if (Input.GetKeyDown(KeyCode.Space))
           {
               battleBtn.Select();
           }
        }

        else if(battleMenu.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                attackBtn.Select();
            }
        }


        if (nowHp <= 0)
        {
            Dead();
        }
    }

    private void BattlePanel()
    {
        menu.SetActive(true);
        isMenu = true;
    }

    public void BattleMenu()
    {
        menu.SetActive(false);
        battleMenu.SetActive(true);
    }

    public void Back()
    {
        menu.SetActive(true);
        battleMenu.SetActive(false);
    }

    public void Attack()
    {
        BattleState = BattleState.Hit;
        Debug.Log("Player's Attack!");
        menu.SetActive(false);
        isMenu = false;
        battleMenu.SetActive(false);
        nowTime = 0f;

        Monster m = monster.GetComponent<Monster>();
        m.nowHp -= 20f;
        m.hpSlider.value = m.nowHp / m.maxHp;
    }

    public void Magic()
    {
        if (nowMp <= 0)
        {
            return;
        }

        BattleState = BattleState.Magic;
        Debug.Log("Player's Magic!");
        menu.SetActive(false);
        isMenu = false;
        battleMenu.SetActive(false);
        nowTime = 0f;
        nowMp -= 20f;
        mpSlider.value = nowMp / maxMp; 

        Monster m = monster.GetComponent<Monster>();
        m.nowHp -= 50f;
        m.hpSlider.value = m.nowHp / m.maxHp;
    }

    void Dead()
    {
        BattleState = BattleState.Dead;

        StartCoroutine(Gameover());

        gameoverBg.gameObject.SetActive(true);
        gameoverTxt.gameObject.SetActive(true);
        gameoverScoreTxt.text = $"Score : {score}";
        gameoverScoreTxt.gameObject.SetActive(true);
        PlayerPrefs.SetInt("Score", score);

        StartCoroutine(BacktoTitle());

    }

    IEnumerator BacktoMain()
    {
        yield return new WaitForSeconds(2f);

        transition.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene("Main");
    }

    IEnumerator Gameover()
    { 
        yield return new WaitForSeconds(5f);

    }

    IEnumerator BacktoTitle()
    {
        yield return new WaitForSeconds(5f);

        SceneManager.LoadScene("Title");
    }
}
