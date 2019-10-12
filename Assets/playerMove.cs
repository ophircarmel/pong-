
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMove : MonoBehaviour
{
    // Paddle's velocity.
    public const float dx = 2.5f;

    // Rigidbody component.
    public Rigidbody rg;

    // Insert values using unity api.
    public string up;
    public string down;


    // <summary>
    // Start is called before the first frame update.
    // </summary>
    void Start()
    {
        rg = transform.gameObject.GetComponent<Rigidbody>();
        if (gameObject.name == "player1")
        {
            up = transform.parent.parent.GetComponent<setting>().Player1Up;
            down = transform.parent.parent.GetComponent<setting>().Player1Down;
        }
        else
        {
            up = transform.parent.parent.GetComponent<setting>().Player2Up;
            down = transform.parent.parent.GetComponent<setting>().Player2Down;
        }
    }


    // <summary>
    // Update is called once per frame.
    // Get input from user and move the player according to it.
    // </summary>
    void Update()
    {
        if (transform.parent.parent.GetComponent<setting>().twoPlayers || transform.name == "player1")
        {
            if (Input.GetKey(down))
            {
                // Move right, according to user's input.
                rg.velocity = new Vector3(dx, 0, 0); ;
            }
            else if (Input.GetKey(up))
            {
                // Move left, according to user's input.
                rg.velocity = new Vector3(-dx, 0, 0);
            }
            else
            {
                // Another key is not acceptable, don't move.
                rg.velocity = new Vector3(0, 0, 0);
            }
        }
    }
}