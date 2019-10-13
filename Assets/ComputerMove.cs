using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComputerMove : MonoBehaviour
{
    // Paddle's velocity.
    public const float dx = 2.5f;
    private Vector3 center;

    // Rigidbody component.
    public Rigidbody rg;
    private Rigidbody ball;
    // Start is called before the first frame update
    void Start()
    {
        if (transform.parent.parent.GetComponent<setting>().twoPlayers)
        {
            this.enabled = false;
        }
        rg = transform.gameObject.GetComponent<Rigidbody>();
        ball = transform.parent.transform.parent.Find("Sphere").GetComponent<Rigidbody>();
        center = rg.position;
    }

    // Update is called once per frame
    void Update()
    {
        float target = ball.position.x;
        bool isCenter = false;
        if (ball.velocity.z <= 0 || (ball.position.x > center.x + 12.5) || (ball.position.x < center.x - 12.5) || ball.position.z > rg.position.z || ball.position.z - rg.position.z < -48)
        {
            rg.velocity = Vector3.zero;
            target = center.x;
            isCenter = true;
        }
        if (Mathf.Abs(target - rg.position.x) < 2.5 && isCenter)
        {
            rg.velocity = Vector3.zero;
            return;
        }
        if (target > rg.position.x)
        {
            rg.velocity = new Vector3(dx, 0, 0);
        }
        else if (target < rg.position.x)
        {
            rg.velocity = new Vector3(-dx, 0, 0);
        }
        
    }

    private float findTaget()
    {
        return 0;
    }
}
