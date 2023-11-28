using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EagleController : EnemyController
{
    private int MoveRange = 500;
    private int MoveRight = 0;
    private int MoveLeft = 0;
    private int MoveDirection = 1;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        transform.localScale = new Vector3(-1, 1, 1);
    }

    // Update is called once per frame
    void Update()
    {
        move();
    }

    void move() {
        if (MoveDirection == 1) {
            MoveRight += 1;
            transform.position = new Vector3(transform.position.x + 0.005f, transform.position.y, transform.position.z);
            if (MoveRight == MoveRange) {
                transform.localScale = new Vector3(1, 1, 1);
                MoveDirection = -1;
                MoveRight = 0;
            }
        } else {
            MoveLeft += 1;
            transform.position = new Vector3(transform.position.x - 0.005f, transform.position.y, transform.position.z);
            if (MoveLeft == MoveRange) {
                transform.localScale = new Vector3(-1, 1, 1);
                MoveDirection = 1;
                MoveLeft = 0;
            }
        }
    }
}
