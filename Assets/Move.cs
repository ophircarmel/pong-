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
    private bool isIn = false;

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
            strPsn = transform.position;
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
        if (isIn)
        {
            return;
        }
        Debug.Log("col");

        bool flag = true;

        Vector3 vel = new Vector3(dx, 0, dz);
        
        // Cubes set on the sides of the board.
        string[] sideCubes = { "Cube (2)", "Cube (3)" };

        // Cubes set as goals.
        string[] goalCubes = { "leftWall", "centerWall", "rightWall"};

        if (sideCubes.Contains(collision.collider.name))
        {
            // A collision occured on the sides of the boards, change velocity on x-axis.
            this.dx *= -1;
            vel.x = dx;

        }

        if (goalCubes.Contains(collision.collider.name))
        {
            Debug.Log(collision.collider.name + ", " + collision.collider.transform.parent.name + ", " + collision.collider.transform.parent.transform.parent.name);

            if (collision.collider.tag == "winWall")
            {
                // A goal is scored.
                if ((collision.collider.transform.parent.name == "wall1"))
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
            else if ((collision.collider.transform.parent.name == "wall1" && dz > 0) || (collision.collider.transform.parent.name == "wall2" && dz < 0))
            {
                return;
            } else
            {
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
        }
        Debug.Log("col-end");


    }
    public void OnTriggerEnter(Collider collider)
    {
        if (collider.name == "Plane" || isIn)
        {
            return;
        }
        isIn = true;
        Debug.Log("trig-start" + "\n" + strPsn);
        Debug.Log(collider.name + ", " + collider.transform.parent.name + ", " + collider.transform.parent.transform.parent.tag);
        float x;
        float z;
        if (collider.transform.parent.name == "wall1" || collider.transform.parent.name == "wall2")
        {
            if ((collider.transform.parent.name == "wall1"))
            {
                z = strPsn.z - 50;
            }
            else
            {
                z = strPsn.z + 50;
            }
            if (collider.name == "rightWall") {
                if ((collider.transform.parent.transform.parent.tag == "rightBoard"))
                {
                    x = strPsn.x - 50;
                }
                else
                {
                    x = strPsn.x + 25;
                }
            }
            else if (collider.name == "leftWall")
            {
                if ((collider.transform.parent.transform.parent.tag == "leftBoard"))
                {
                    x = strPsn.x + 50;
                }
                else
                {
                   x = strPsn.x - 25;
                }
            } else
            {
                x = strPsn.x;
            }
            strPsn = new Vector3(x, 1, z);
            rg.position = strPsn;
            rg.velocity = new Vector3(0, 0, 0);

            // update camera location
            CameraManager manager = GameObject.Find("Main Camera").GetComponent<CameraManager>();
            manager.newCamPsn = new Vector3(x, 10, z);

            Debug.Log("trig-end");
            countdown();
        }
    }
    void OnCollisionExit(Collision collision)
    {
        isIn = false;
    }
    void OnTriggerExit(Collider other)
    {
        isIn = false;
    }
    void countdown()
    {
        Invoke("Start", 3f);
    }

}