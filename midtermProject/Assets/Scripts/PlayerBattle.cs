using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

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

    public GameObject menu;
    public GameObject battleMenu;
    private bool isMenu;

    private GameObject monster;

    public Animator transition;
    public float transitionTime = 1f;

    public Button battleBtn;
    public Button attackBtn;


    private void Start()
    {
        monster = GameObject.FindGameObjectWithTag("Monster");

        BattleState = BattleState.Idle;
        nowTime = 0f;
        hpSlider.value = nowHp / maxHp;

    }

    private void Update()
    {
        if (!monster.activeSelf)
        {
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
        BattleState = BattleState.Magic;
        Debug.Log("Player's Magic!");
        menu.SetActive(false);
        isMenu = false;
        battleMenu.SetActive(false);
        nowTime = 0f;

        Monster m = monster.GetComponent<Monster>();
        m.nowHp -= 30f;
        m.hpSlider.value = m.nowHp / m.maxHp;
    }

    IEnumerator BacktoMain()
    {
        yield return new WaitForSeconds(2f);

        transition.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene("Main");
    }
}
