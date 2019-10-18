using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

public class Move : MonoBehaviour, IMoveBoardListener
{
    private static readonly int BOARDWIDTH = 25;

    private static readonly int BOARDLENGTH = 50;

    // Current board. At first it is the middle board.
    private int currentboard = 5;

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

    // Is a collision/ trigger occuring.
    private bool isIn = false;

    // A list of move board listeners.
    List<IMoveBoardListener> listeners = new List<IMoveBoardListener>();

    // A list of destroyed walls.
    List<Collider> destroyed = new List<Collider>();

    // <summary>
    // Start is called before the first frame update.
    // </summary>
    void Start()
    {
        if (score1 == score2 && score1 == 0)
        {
            // Add self as listener.
            this.listeners.Add(this);

            // Add camera as listener.
            this.listeners.Add((CameraManager)GameObject.Find("Main Camera").GetComponent<CameraManager>());

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


        // <summary>
        // Add a listener to the moving board event.
        // </summary>
    }
    public void AddListener(IMoveBoardListener l)
    {
        // Add the listenr to the listeners' list.
        this.listeners.Add(l);
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

        bool flag = true;

        Vector3 vel = new Vector3(dx, 0, dz);

        // Cubes set on the sides of the board.
        string[] sideCubes = { "Cube (2)", "Cube (3)" };

        // Cubes set as goals.
        string[] goalCubes = { "leftWall", "centerWall", "rightWall" };

        if (sideCubes.Contains(collision.collider.name))
        {
            // A collision occured on the sides of the boards, change velocity on x-axis.
            this.dx *= -1;
            vel.x = dx;

        }

        if (goalCubes.Contains(collision.collider.name))
        {
            // A goal is scored.

            if ((collision.collider.transform.parent.name == "wall1" && dz > 0) || (collision.collider.transform.parent.name == "wall2" && dz < 0))
            {
                // Collision has occured from the other side (the ball comes from another board).
                return;
            }
            else
            {
                flag = false;

                // Make the collsion object uncolliadable anymore.
                collision.collider.GetComponent<BoxCollider>().isTrigger = true;

                // Make the collision object unvisible anymore.
                collision.collider.GetComponent<MeshRenderer>().enabled = false;

                // Add this collision object to the destroyed walls' list.
                destroyed.Add(collision.collider);


                vel.z = -vel.z;
                dz = vel.z;
            }

            rg.velocity = vel;
            rg.position += rg.velocity / 100;
        }
        if (collision.collider.tag == "Player")
        {
            // If collision occured with a player

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


    }


    // <summary>
    // Handle the case of trigger is starting.
    // </summary>
    // <param name="collision"> The collision object </param>
    public void OnTriggerEnter(Collider collider)
    {
        if (collider.name == "Plane" || isIn)
        {
            return;
        }

        // A trigger starts.
        isIn = true;

        if (collider.tag == "winWall")
        {
            // A goal is scored.
            if ((collider.transform.parent.name == "wall1"))
            {
                // Player 2 has scored a goal, increase his score.
                this.score2++;

            }
            else
            {
                // Player 1 has scored a goal, increase his score.
                this.score1++;
            }

            foreach (Collider c in destroyed)
            {
                // Make the wall cllidable.
                c.GetComponent<BoxCollider>().isTrigger = false;

                // Make it visible.
                c.GetComponent<MeshRenderer>().enabled = true;
            }

            // Relocate the ball.
            rg.position = new Vector3(10, 0, 25);
            strPsn = rg.position;

            // Set ball's valocity as 0 for meantime.
            rg.velocity = Vector3.zero;

            // After a win, moving to the middle board.
            this.NotifyAll(currentboard, 5);
            currentboard = 5;

            // Wait a bit and start over.
            countdown();
            return;
        }

        // Else

        int next = this.currentboard;

        if (collider.transform.parent.name == "wall1" || collider.transform.parent.name == "wall2")
        {
            if ((collider.transform.parent.name == "wall1"))
            {
                next += 3;
            }
            else
            {
                next -= 3;
            }

            if (collider.name == "rightWall")
            {
                if (collider.transform.parent.parent.tag == "rightBoard")
                {
                    next -= 2;
                }
                else
                {
                    next += 1;
                }
            }
            else if (collider.name == "leftWall")
            {
                if (collider.transform.parent.parent.tag == "leftBoard")
                {
                    next += 2;
                }
                else
                {
                    next -= 1;
                }
            }

            // Let all listeners know that we arre moving board.
            this.NotifyAll(currentboard, next);

            //Countdown and renew the game.
            countdown();
        }
    }


    // <summary>
    // Handle the event of collision is finished.
    // </summary>
    // <param name="collision"> The collision object </param>
    void OnCollisionExit(Collision collision)
    {
        // Collision isn't occuring anymore.
        isIn = false;
    }


    // <summary>
    // Handle the event of trigger is finished.
    // </summary>
    // <param name="collision"> The collision object </param>
    void OnTriggerExit(Collider other)
    {
        // Trigger isn't occuring anymore.
        isIn = false;
    }


    // <summary>
    // Countdown and renew the game.
    // </summary>
    void countdown()
    {
        float seconds = 3f;
        Invoke("Start", seconds);
    }


    // <summary>
    // Notify all listners to move a board.
    // </summary>
    // <param name="prevoius"> The board to move from. </param>
    // <param name="next"> The board to move to. </param>
    private void NotifyAll(int previous, int next)
    {
        foreach (IMoveBoardListener listener in listeners)
        {
            // For each listener, notify to move a board.
            listener.MoveBoard(previous, next);
        }
    }


    // <summary>
    // Move to another board.
    // </summary>
    // <param name="prevoius"> The board to move from. </param>
    // <param name="next"> The board to move to. </param>
    public void MoveBoard(int previous, int next)
    {
        // Note: given a board indexed as n, the board is GameObject((n-1)/3, (n-1)%3).
        int boardz = (next - 1) / 3;
        int boardx = (next - 1) % 3;

        // Calculate next position of the camera.
        float nx = (boardx - 1) * (BOARDWIDTH + 1) + BOARDWIDTH / 2;
        int ny = 0;
        float nz = (1 - boardz) * (BOARDLENGTH + 1) + BOARDLENGTH / 2;

        // Change start position according to the calculations.
        strPsn = new Vector3(nx, ny, nz);

        // Set rigidbody position in the same position.
        rg.position = strPsn;

        // Set velocity to zero.
        rg.velocity = Vector3.zero;

        // Update current board.
        this.currentboard = next;
    }

}