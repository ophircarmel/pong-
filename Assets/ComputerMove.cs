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
        if (Mathf.Abs(target - rg.position.x) < 0.3 && isCenter)
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

    /*private float findTaget()
    {
        float moveInZ1 = 0;
        float moveX = (center.z - ball.position.z) / ball.velocity.z * ball.velocity.x;
        float target1 = moveX + ball.position.x;
        Vector3 psn = ball.position;
            if (target1 < center.x + 12.5f && target1 > center.x - 12.5f)
            {
                return target1;
            }
            if (ball.velocity.x > 0)
            {
                moveInZ1 += (center.x + 12.5f) / ball.velocity.x * ball.velocity.z;
                moveX = (center.z - psn.z - moveInZ1) / ball.velocity.z * (-ball.velocity.x);
                target1 = center.x + 12.5f + moveX;
            }
            else
            {
                moveInZ1 += (center.x - 12.5f) / ball.velocity.x * ball.velocity.z;
                moveX = (center.z - psn.z - moveInZ1) / ball.velocity.z * (-ball.velocity.x);
                target1 = center.x - 12.5f + moveX;
            }
        return target1;
    }*/
}
