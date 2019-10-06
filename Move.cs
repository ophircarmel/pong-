using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    public float dx = 2f;
    public float dy = 3f;
    public Rigidbody rg;
    public int score1 = 0;
    public int score2 = 0;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("start");
        if (score1 == score2 && score1 == 0)
        {
            rg = transform.gameObject.GetComponent<Rigidbody>();
        }
        rg.position = new Vector3(10, 1, 25);
        float rand = Random.Range(0, 2);
        if (rand < 1)
        {
            rg.velocity = new Vector3(dx, 0, dy);
        }
        else
        {
            rg.velocity = new Vector3(-dx, 0, -dy);
            dx = -dx;
            dy = -dy;
        }
        Debug.Log(rg.position + "and, " + rg.velocity);
        Debug.Log("start - end");
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void OnCollisionEnter(Collision collision)
    {
        Debug.Log("col");
        bool flag = true;
        Vector3 vel = new Vector3(dx, 0, dy);
        Debug.Log(vel);
        if (collision.collider.name == "Cube (2)" || collision.collider.name == "Cube (3)")
        {
            this.dx *= -1;
            vel.x = dx;

        }
        if (collision.collider.name == "Cube" || collision.collider.name == "Cube (1)")
        {
            if ((collision.collider.name == "Cube"))
            {
                this.score2++;
            } else {
                this.score1++;
            }
            Debug.Log("here");
            flag = false;
            rg.position = new Vector3(10, 1, 25);
            rg.velocity = new Vector3(0, 0, 0);
            countdown();
            return;
        }
        if (collision.collider.tag == "Player") {
            Debug.Log(collision.transform.position.x + " - " + transform.position.x + " = " + (collision.transform.position.x - transform.position.x));
            if (Mathf.Abs(collision.transform.position.x - transform.position.x) > 0.8) {
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
            this.dy *= -1;
            vel.z = dy;
        }
        if (flag)
        {
            rg.velocity = vel;
            Debug.Log(collision.collider.name);
            Debug.Log(vel);
        }
        Debug.Log("col-end");


    }
    void countdown()
    {
        Invoke("Start", 3f);
    }

}
