
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMove : MonoBehaviour, IMoveBoardListener
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
        transform.parent.parent.FindChild("Sphere").GetComponent<Move>().AddListener(this);
        rg = transform.gameObject.GetComponent<Rigidbody>();
        if (gameObject.name == "player1")
        {
            up = transform.parent.parent.GetComponent<setting>().Player1Up;
            down = transform.parent.parent.GetComponent<setting>().Player1Down;
        }
        else
        {
            if (!transform.parent.parent.GetComponent<setting>().twoPlayers)
            {
                this.enabled = false;
            }
            up = transform.parent.parent.GetComponent<setting>().Player2Up;
            down = transform.parent.parent.GetComponent<setting>().Player2Down;
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
    public void MoveBoard(int prevoius, int next)
    {
        Debug.Log("caculate: " + ((transform.parent.name.Substring(11)[1] - '0') * 3 + (transform.parent.name.Substring(11)[3] - '0') + 1));
        if ((transform.parent.name.Substring(11)[1] - '0')*3 + (transform.parent.name.Substring(11)[3] - '0') + 1 == next)
        {
            transform.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ;
        }
        else
        {
            transform.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        }
    }
}