using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

public class Move : MonoBehaviour
{
    // Velocity' variables.
    public float dx = 2f;
    public float dz = 3f;

    // Rigidbody component.
    public Rigidbody rg;

    // Scores of 2 players.
    public int score1 = 0;
    public int score2 = 0;

    // Start position of the ball.
    private Vector3 strPsn = new Vector3(10, 1, 25);


    // <summary>
    // Start is called before the first frame update.
    // </summary>
    void Start()
    {
        Debug.Log("start");

        if (score1 == score2 && score1 == 0)
        {
            // Set rigidbody component.
            rg = transform.gameObject.GetComponent<Rigidbody>();
        }

        // Set random val to indicate the directionof the ball to move.
        float rand = Random.Range(0, 2);

        if (rand < 1)
        {
            // In such case, move using initial values.
            rg.velocity = new Vector3(dx, 0, dz);
        }
        else
        {
            // Otherwise, move in the opposite direction.
            rg.velocity = new Vector3(-dx, 0, -dz);

            // Update velocity data.
            dx = -dx;
            dz = -dz;
        }
        Debug.Log(rg.position + "and, " + rg.velocity);
        Debug.Log("start - end");
    }


    // <summary>
    // OnCollisionEnter is called when this collider/rigidbody has begun touching another rigidbody/collider.
    // </summary>

    // <param name="collision"> The collision object </param>
    public void OnCollisionEnter(Collision collision)
    {
        Debug.Log("col");

        bool flag = true;

        Vector3 vel = new Vector3(dx, 0, dz);

        Debug.Log(vel);

        // Cubes set on the sides of the board.
        string[] sideCubes = { "Cube (2)", "Cube (3)" };

        // Cubes set as goals.
        string[] goalCubes = { "Cube", "Cube (1)" };

        if (sideCubes.Contains(collision.collider.name))
        {
            // A collision occured on the sides of the boards, change velocity on x-axis.
            this.dx *= -1;
            vel.x = dx;

        }

        if (goalCubes.Contains(collision.collider.name))
        {
            if (collision.collider.tag == "winWall")
            {
                // A goal is scored.

                if ((collision.collider.name == "Cube"))
                {
                    // Player 2 has scored a goal, increase his score.
                    this.score2++;
                }
                else
                {
                    // Player 1 has scored a goal, increase his score.
                    this.score1++;
                }

                // Relocate the ball.
                rg.position = new Vector3(10, 1, 25);
                strPsn = rg.position;

                // Set ball's valocity as 0 for meantime.
                rg.velocity = new Vector3(0, 0, 0);

                // Wait a bit and start over.
                countdown();
                return;
            }
            else
            {
                Debug.Log("here");
                flag = false;
                collision.collider.GetComponent<BoxCollider>().isTrigger = true;
                collision.collider.GetComponent<MeshRenderer>().enabled = false;
                vel.z = -vel.z;
                dz = vel.z;
            }

            rg.velocity = vel;
            rg.position += rg.velocity / 100;
        }
        if (collision.collider.tag == "Player")
        {
            Debug.Log(collision.transform.position.x + " - " + transform.position.x + " = " + (collision.transform.position.x - transform.position.x));
            if (Mathf.Abs(collision.transform.position.x - transform.position.x) > 0.8)
            {
                if (collision.transform.position.x > transform.position.x)
                {
                    dx = -Mathf.Abs(dx);
                }
                else
                {
                    dx = Mathf.Abs(dx);
                }
                vel.x = dx;
            }
            this.dz *= -1;
            vel.z = dz;
        }
        if (flag)
        {
            rg.velocity = vel;
            Debug.Log(collision.collider.name);
            Debug.Log(vel);
        }
        Debug.Log("col-end");


    }
    public void OnTriggerEnter(Collider collider)
    {
        if (collider.name == "Cube" || collider.name == "Cube (1)")
        {
            if ((collider.name == "Cube"))
            {
                strPsn = new Vector3(10, 1, strPsn.z - 50);
                rg.position = strPsn;
            }
            else
            {
                strPsn = new Vector3(10, 1, strPsn.z + 50);
                rg.position = strPsn;
            }

            rg.velocity = new Vector3(0, 0, 0);
            countdown();
        }
    }
    void countdown()
    {
        Invoke("Start", 3f);
    }

}
