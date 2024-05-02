using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private void Start()
    {
        Application.targetFrameRate = 30;

        rb2d = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
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
        }
      /*  else
        {
            rb2d.velocity = moveDirection.normalized * 0;
            PlayerState = PlayerState.Idle;
        }*/

    }
}
