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

    public int redPosion;
    public int bluePosion;
    public Text redPosionTxt;
    public Text bluePosionTxt;

    public Slider atbSlider;
    public Slider hpSlider;
    public Slider mpSlider;

    public GameObject menu;
    public GameObject battleMenu;
    public GameObject itemMenu;
    private bool isMenu;

    private GameObject monster;

    public Animator transition;
    public float transitionTime = 1f;
    public Animator playerTransition;

    public Button battleBtn;
    public Button attackBtn;
    public Button redPosionBtn;

    public Image gameoverBg;
    public Text gameoverTxt;

    private int score;
    public Text scoreTxt;
    public Text gameoverScoreTxt;

    public GameObject attackEffect;
    private GameObject ae;
    public GameObject magicAttackEffect;
    private GameObject mae;

    public GameObject[] enemyList;
    private void Start()
    {
        int rdNum = Random.Range(0, 3);
        Instantiate(enemyList[rdNum]);
        monster = GameObject.FindGameObjectWithTag("Monster");
        nowHp = PlayerPrefs.GetFloat("PlayerHp");
        nowMp = PlayerPrefs.GetFloat("PlayerMp");
        redPosion = PlayerPrefs.GetInt("RedPosion");
        bluePosion = PlayerPrefs.GetInt("BluePosion");

        score = PlayerPrefs.GetInt("Score");
        scoreTxt.text = $"Score : {score}";

        redPosionTxt.text = $"Red Posion : {redPosion}";
        bluePosionTxt.text = $"Blue Posion : {bluePosion}";

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
        
        else if(itemMenu.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                redPosionBtn.Select();
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

    public void ItemMenu()
    {
        menu.SetActive(false);
        itemMenu.SetActive(true);
    }

    public void Back()
    {
        menu.SetActive(true);
        battleMenu.SetActive(false);
        itemMenu.SetActive(false);
    }

    public void Attack()
    {
        BattleState = BattleState.Hit;
        Debug.Log("Player's Attack!");
        isMenu = false;
        battleMenu.SetActive(false);
        nowTime = 0f;

        Vector3 mPosition = monster.transform.position;
        ae = Instantiate(attackEffect, mPosition, Quaternion.identity);
        Destroy(ae, 0.8f);
        StartCoroutine(WalkMotion());

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
        isMenu = false;
        battleMenu.SetActive(false);
        nowTime = 0f;
        nowMp -= 20f;
        mpSlider.value = nowMp / maxMp;

        Vector3 mPosition = monster.transform.position;
        mae = Instantiate(magicAttackEffect, mPosition, Quaternion.identity);
        Destroy(mae, 0.7f);

        StartCoroutine(WalkMotion());

        Monster m = monster.GetComponent<Monster>();
        m.nowHp -= 50f;
        m.hpSlider.value = m.nowHp / m.maxHp;
    }

    public void RedPosion()
    {
        if (redPosion <= 0 || nowHp >= 100f)
        {
            return;
        }

        Debug.Log("RedPosion!");
        isMenu = false;
        itemMenu.SetActive(false);

        redPosion--;
        redPosionTxt.text = $"Red Posion : {redPosion}";
        PlayerPrefs.SetInt("RedPosion", redPosion);

        nowTime = 0f;
        if (nowHp >= 50f)
        {
            nowHp += 100f - nowHp;
        }
        else if (nowHp < 50f)
        {
            nowHp += 50f;
        }
        hpSlider.value = nowHp / maxHp;
    }

    public void BluePosion()
    {
        if (bluePosion <= 0 || nowMp >= 100f)
        {
            return;
        }

        Debug.Log("BluePosion!");
        isMenu = false;
        itemMenu.SetActive(false);

        bluePosion--;
        bluePosionTxt.text = $"Blue Posion : {bluePosion}";
        PlayerPrefs.SetInt("BluePosion", bluePosion);

        nowTime = 0f;
        if (nowMp >= 50f)
        {
            nowMp += 100f - nowMp;
        }
        else if (nowMp < 50f)
        {
            nowMp += 50f;
        }
        
        mpSlider.value = nowMp / maxMp;
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
    IEnumerator WalkMotion()
    {
        playerTransition.SetBool("BattleWalk", true);

        transform.position = Vector2.MoveTowards(transform.position, new Vector2(2, -0.09f), 1);

        yield return new WaitForSeconds(1f);

        playerTransition.SetBool("BattleWalk", false);
        transform.position = Vector2.MoveTowards(transform.position, new Vector2(4, -0.09f), 1);
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
