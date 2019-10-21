﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class playerMove : NetworkBehaviour, IMoveBoardListener
{
    // Paddle's velocity.
    public const float dx = 2.5f;

    // Rigidbody component.
    public Rigidbody rg;

    // Insert values using unity api.
    public string up;
    public string down;
    private float x;


    // <summary>
    // Start is called before the first frame update.
    // </summary>
    void Start()
    {
        transform.parent.parent.Find("Sphere").GetComponent<Move>().AddListener(this);
        rg = transform.gameObject.GetComponent<Rigidbody>();
        x = rg.position.x;
        if (gameObject.name == "player1")
        {
            up = setting.Player1Up;
            down = setting.Player1Down;
        }
        else
        {
            if (!setting.twoPlayers)
            {
                this.enabled = false;
            }
            up = setting.Player2Up;
            down = setting.Player2Down;
        }
        if (!transform.parent.name.EndsWith("(1,1)"))
        {
            transform.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        }
    }


    // <summary>
    // Update is called once per frame.
    // Get input from user and move the player according to it.
    // </summary>
    void Update()
    {
        if (hasAuthority)
        {
            if (Input.GetKey(down))
            {
                // Move down, according to user's input.
                rg.velocity = new Vector3(dx, 0, 0); ;
            }
            else if (Input.GetKey(up))
            {
                // Move up, according to user's input.
                rg.velocity = new Vector3(-dx, 0, 0);
            }
            else
            {
                // Another key is not acceptable, don't move.
                rg.velocity = new Vector3(0, 0, 0);
            }
        }
    }


    // <summary>
    // Move to another board.
    // </summary>
    // <param name="prevoius"> The board to move from. </param>
    // <param name="next"> The board to move to. </param>
    public void MoveBoard(int previous, int next)
    {
        if ((transform.parent.name.Substring(11)[1] - '0') * 3 + (transform.parent.name.Substring(11)[3] - '0') + 1 == next)
        {
            // If the player is on the next board.

            // Release movement.
            transform.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ;
        }
        else if ((transform.parent.name.Substring(11)[1] - '0') * 3 + (transform.parent.name.Substring(11)[3] - '0') + 1 == previous)
        {
            // If the player is on the previous board.

            // Freeze movement.
            transform.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;

            // Restart position.
            transform.position = new Vector3(x, 0, transform.position.z);
        }
    }
}