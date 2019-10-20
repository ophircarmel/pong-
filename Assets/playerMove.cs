
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMove : MonoBehaviour, IMoveBoardListener, ballListeners
{
    // Paddle's velocity.
    public float dx = Constants.PLAYER_SPEED;

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
        transform.parent.parent.Find("ballListenersHolder").GetComponent<holdListeners>().AddListener(this);
        rg = transform.gameObject.GetComponent<Rigidbody>();
        x = rg.position.x;
        if (gameObject.name == "player1")
        {
            if (setting.local) {
                up = setting.Player1Up;
                down = setting.Player1Down;
            } else
            {
                up = "up";
                down = "down";
            }
        }
        else
        {
            if (!setting.twoPlayers)
            {
                this.enabled = false;
            }
            if (setting.local)
            {
                up = setting.Player2Up;
                down = setting.Player2Down;
            }
            else
            {
                up = "w";
                down = "s";
            }
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
            //if (Input.GetKeyUp(up) || Input.GetKeyUp(down))
            //{
              //  rg.velocity = new Vector3(0, 0, 0);
            //}
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

    public void heBorn()
    {
        transform.parent.parent.Find("Sphere").GetComponent<Move>().AddListener(this);
    }
}