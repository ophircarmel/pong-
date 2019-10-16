using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

public class Move : MonoBehaviour, IMoveBoardListener
{
    private static readonly int BOARDWIDTH = 25;

    private static readonly int BOARDLENGTH = 50;

    // Current board.
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
    private bool isIn = false;

    // A list of move board listeners.
    List<IMoveBoardListener> listeners = new List<IMoveBoardListener>();

    // <summary>
    // Start is called before the first frame update.
    // </summary>
    void Start()
    {
        Debug.Log("start");
        if (score1 == score2 && score1 == 0)
        {
            // Add self as listener.
            this.listeners.Add(this);

            // Add camera as listener.
            this.listeners.Add((CameraManager)GameObject.Find("Main Camera").GetComponent<CameraManager>());
            // Set current board to the middle one.
            //this.currentboard = 5;
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
        //Debug.Log(rg.position + "and, " + rg.velocity);
        //Debug.Log("start - end");
    }
    public void AddListener(IMoveBoardListener l)
    {
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
        //Debug.Log("col" + "\n" + transform.position.x);

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
            //Debug.Log(collision.collider.name + ", " + collision.collider.transform.parent.name + ", " + collision.collider.transform.parent.transform.parent.name);

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
                rg.position = new Vector3(10, 0, 25);
                strPsn = rg.position;

                // Set ball's valocity as 0 for meantime.
                rg.velocity = new Vector3(0, 0, 0);
                // update camera location
                //CameraManager manager = GameObject.Find("Main Camera").GetComponent<CameraManager>();
                //manager.newCamPsn = new Vector3(10, 15, 25);
                this.NotifyAll(currentboard, 5);
                currentboard = 5;
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
        //Debug.Log("col-end");


    }
    public void OnTriggerEnter(Collider collider)
    {
        if (collider.name == "Plane" || isIn)
        {
            return;
        }
        isIn = true;
        //Debug.Log("trig-start" + "\n" + strPsn);
        //Debug.Log(collider.name + ", " + collider.transform.parent.name + ", " + collider.transform.parent.transform.parent.tag);

        int next = this.currentboard;
        Debug.Log("startZZZ: " + next);
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

            if (collider.name == "rightWall") {
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
            /*strPsn = new Vector3(x, 0, z);
            rg.position = strPsn;
            rg.velocity = new Vector3(0, 0, 0);

            // update camera location
            CameraManager manager = GameObject.Find("Main Camera").GetComponent<CameraManager>();
            manager.newCamPsn = new Vector3(x, 15, z);*/
            Debug.Log(next);
            this.NotifyAll(currentboard, next);

            //Debug.Log("trig-end");
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


    // <summary>
    // Notify all listners to move a board.
    // </summary>
    // <param name="prevoius"> The board to move from. </param>
    // <param name="next"> The board to move to. </param>
    private void NotifyAll(int previous, int next)
    {
        foreach (IMoveBoardListener listener in listeners)
        {
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

        strPsn = new Vector3(nx, ny, nz);
        //Debug.Log(strPsn);
        rg.position = strPsn;
        rg.velocity = new Vector3(0, 0, 0);

        // Update current board.
        this.currentboard = next;
    }

}