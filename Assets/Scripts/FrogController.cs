using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogController : EnemyController
{
    private float Speed = 5f;
    private int MoveRange = 3;
    public float initX;
    private int MoveDirection = 1;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        // Move();
    }

    private void Awake() {
        transform.localScale = new Vector3(-0.7f, 0.7f, 1);
        initX = transform.position.x;
    }

    void Idel(){
        Anim.SetBool("isJumpping", false);
        Rb.velocity = new Vector2(0, 0);
    }

    void Move() {
        // idel 动画结束进入跳跃动画
        if (MoveDirection == 1) {
            // move right
            if (transform.position.x < initX + MoveRange) {
                Rb.velocity = new Vector2(Speed, 0);
                Anim.SetBool("isJumpping", true);
                // jump
            } else {
                transform.localScale = new Vector3(0.7f, 0.7f, 1);
                MoveDirection = -1;
            }
        } else {
            if (transform.position.x > initX - MoveRange) {
                Rb.velocity = new Vector2(-Speed, 0);
                Anim.SetBool("isJumpping", true);
            } else {
                transform.localScale = new Vector3(-0.7f, 0.7f, 1);
                MoveDirection = 1;
            }
        }
    }
}
