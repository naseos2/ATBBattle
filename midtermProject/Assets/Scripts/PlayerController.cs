using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum PlayerState
{
    Idle,
    Walk,
    Run
}


public class PlayerController : MonoBehaviour
{
    public PlayerState PlayerState;
    private Rigidbody2D rb2d;

    private Vector2 moveDirection = Vector2.zero;
    public float speed;
    public float runSpeed;

    private Animator anim;

    private bool isEncounter;
    public Animator transition;
    public float transitionTime = 1f;

    private int score;
    public Text scoreTxt;


/*    [InitializeOnLoadMethod]
    static void RunMethod()
    {
        PlayerPrefs.SetFloat("X", 0f);
        PlayerPrefs.SetFloat("Y", -2f);
        PlayerPrefs.SetFloat("PlayerHp", 100f);
    }*/

    private void Start()
    {
        Load();

        Application.targetFrameRate = 30;

        isEncounter = false;

        rb2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        score = PlayerPrefs.GetInt("Score");
        scoreTxt.text = $"Score : {score}";
    }

    private void Update()
    {
        anim.SetFloat("MoveX", Input.GetAxis("Horizontal"));
        anim.SetFloat("MoveY", Input.GetAxis("Vertical"));
    }

    private void FixedUpdate()
    {
        if (isEncounter)
            return;

        KeybordInput();
    }

    void KeybordInput()
    {
        float xx = Input.GetAxis("Horizontal");
        float yy = Input.GetAxis("Vertical");

        if (xx != 0 || yy != 0)
        {
            moveDirection = new Vector2(xx, yy);

            rb2d.velocity = moveDirection.normalized * speed;

            PlayerState = PlayerState.Walk;
            Encounter();
            
        }
        else
        {
            rb2d.velocity = moveDirection.normalized * 0;
            PlayerState = PlayerState.Idle;
        }

    }

    void Encounter()
    {
        int random = Random.Range(1, 1000);

        if (random <= 5)
        {
            isEncounter = true;
            rb2d.velocity = moveDirection.normalized * 0;
            Debug.Log("Encounter");
            Save();
            StartCoroutine(LoadLevel());
        }
        
    }

    IEnumerator LoadLevel()
    {
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene("Battle");
    }

    void Save()
    {
        PlayerPrefs.SetFloat("X", gameObject.transform.position.x);
        PlayerPrefs.SetFloat("Y", gameObject.transform.position.y);
    }
    
    public void Load()
    {
        float x = PlayerPrefs.GetFloat("X");
        float y = PlayerPrefs.GetFloat("Y");

        gameObject.transform.position = new Vector2(x, y);
    }
}
