using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ComputerMove : MonoBehaviour
{
    // Paddle's velocity.
    public const float dx = 2.5f;
    private Vector3 center;

    // Rigidbody components.
    public Rigidbody rg;
    private Rigidbody ball;
    private bool hasTarget = false;
    private float target;
    private bool harder = false;
    private float difficulty;
    // <summary>
    // Start is called before the first frame update.
    // Check if game is user vs computer.
    // As well, declare some fields.
    // </summary>
    void Start()
    {
        if (setting.twoPlayers)
        {
            // Player 2 should not move using computer logic.
            this.enabled = false;
        } else
        {
            difficulty = setting.HardLevel;
        }

        // Set rigidbody component.
        rg = transform.gameObject.GetComponent<Rigidbody>();

        // Set a pointer to the ball object.
        ball = transform.parent.transform.parent.Find("Sphere").GetComponent<Rigidbody>();

        // Set the center of the computer player.
        center = rg.position;
    }


    // <summary>
    // Update is called once per frame.
    // </summary>
    void Update()
    {
        float rand = Random.Range(0, 100);
        bool isCenter = false;
        if (ball.velocity.z <= 0 || (ball.position.x > center.x + 12.5) || (ball.position.x < center.x - 12.5) || ball.position.z > rg.position.z || ball.position.z - rg.position.z < -48)
        {
            rg.velocity = Vector3.zero;
            target = center.x;
            isCenter = true;
            hasTarget = false;
            harder = false;
        }
        else if ((!hasTarget && rand < difficulty) || harder)
        {
            target = findTaget();
            //hasTarget = true;
            harder = true;
        }
        else if (!hasTarget || !harder)
        {
            target = ball.position.x;
            harder = false;
            hasTarget = true;
        }
        if (Mathf.Abs(target - rg.position.x) < 0.3 && isCenter || (harder && Mathf.Abs(target - rg.position.x) < 1.5 && !isCenter))
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
        if (ball.velocity.z == 0)
        {
            Debug.Log("Error");
            return 0;
        }
        Vector3 vel = new Vector3(ball.velocity.x, ball.velocity.y, ball.velocity.z) / 50;
        Vector3 pos = new Vector3(ball.position.x, ball.position.y, ball.position.z);
        //float target = ball.position.x;
        while (pos.z < center.z - 0.5f)
        {
            pos += vel;
            if (pos.x > center.x + 12)
            {
                vel.x = -vel.x;
                pos += (101 / 100) * vel;
            }
            if (pos.x < center.x - 12)
            {
                vel.x = -vel.x;
                pos += (101 / 100) * vel;
            }
        }
        //Debug.Log(pos.x);
        return pos.x;
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.name == "Sphere")
        {
            hasTarget = false;
            harder = false;
        }
    }
}