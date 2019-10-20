﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerObj1 : NetworkBehaviour, IMoveBoardListener
{
    // Paddle's velocity.
    public const float dx = 2.5f;

    public GameObject playerPrefab1;

    public GameObject[,] players = new GameObject[3,3];

    //Current player coordinates.
    public int xCurrent = 1;
    public int yCurrent = 1;


    // <summary>
    // Start is called before the first frame update
    // </summary>
    void Start()
    {
        if (isLocalPlayer)
        {
            // Spawn the player (with authority) to the relevant user only.
            CmdSpawnPlayer1();

            GameObject.Find("Sphere").transform.GetComponent<Move>().AddListener(this);
        }
    }


    // <summary>
    // Update is called once per frame.
    // </summary>
    void Update()
    {
        if (isLocalPlayer)
        {
            if (Input.GetKey("down"))
            {
                CmdMoveDown();
            }
            else if (Input.GetKey("up"))
            {
                CmdMoveUp();
            }
        }
    }


    [Command]
    private void CmdMoveDown()
    {
        // Move down, according to user's input.
        players[xCurrent, yCurrent].transform.Translate(new Vector3(dx, 0, 0));
    }


    [Command]
    private void CmdMoveUp()
    {
        // Move up, according to user's input.
        players[xCurrent,yCurrent].transform.Translate(new Vector3(-dx, 0, 0));
    }


    // <summary>
    // A command to the server to spawn the created player.
    // </summary>
    [Command]
    void CmdSpawnPlayer1()
    {
        // Iterate over all players that the user should control.
        for (int i = 0; i < players.GetLength(0); i++)
        {
            for (int j = 0; j < players.GetLength(1); j++)
            {
                // Add a prefab to the player.
                players[i, j] = Instantiate(playerPrefab1);

                // Set the prefab parent as its board.
                players[i, j].transform.parent = GameObject.Find(string.Format("GameObject ({0},{1})", i, j)).transform;

                // Set local start position.
                players[i, j].transform.localPosition = new Vector3(10, 0, 2);

                // Spawn with authority.
                NetworkServer.SpawnWithClientAuthority(players[i, j], connectionToClient);
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
        xCurrent = (next - 1) / 3;
        yCurrent = (next - 1) % 3;
    }
}