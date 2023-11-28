using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    public Transform Cam;
    public float MoveRate;
    private float StartPositionX;
    // Start is called before the first frame update
    void Start()
    {
        StartPositionX = transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector2(StartPositionX + Cam.position.x * MoveRate, transform.position.y);
    }
}
