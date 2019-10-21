using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;

public class NetworkCustom : NetworkManager
{
    public GameObject playerObj1;
    public GameObject playerObj2;

    public GameObject spherePrefab;

    public GameObject player1;
    public GameObject player2;

    public short onlinePlayers = 0;


    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
    {
        Debug.Log("start client");
        GameObject playerObj;

        if (onlinePlayers == 0)
        {
            // Inastiate player 1.
            playerObj = Instantiate(playerObj1);

            // Inastiate the ball.
            GameObject sphere = Instantiate(spherePrefab);

            // Set game as its parent.
            sphere.transform.parent = GameObject.Find("Game").transform;

            NetworkServer.Spawn(playerObj);

            NetworkServer.Spawn(sphere);
        }
        else if (onlinePlayers == 1)
        {
            // Inastiate player 2.
            playerObj = Instantiate(playerObj2);

            NetworkServer.Spawn(playerObj);
        }
        else
        {
            playerObj = null;
        }

        Debug.Log(playerObj);

        NetworkServer.AddPlayerForConnection(conn, playerObj, playerControllerId);

        onlinePlayers++;
        Debug.Log("finish client");
    }



    public override void OnClientConnect(NetworkConnection conn)
    {
        Debug.Log(onlinePlayers);
        ClientScene.AddPlayer(conn, onlinePlayers);
    }
}
