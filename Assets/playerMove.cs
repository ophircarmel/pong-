using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMove : MonoBehaviour
{
    public float dx = 2.5f;
    public Rigidbody rg;
    public string up;
    public string down;
    // Start is called before the first frame update
    void Start()
    {
        rg = transform.gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(up+ ""))
        {
            rg.velocity = new Vector3(dx, 0, 0); ;
        }
        else { if (Input.GetKey(down+ "" ))
            {
                rg.velocity = new Vector3(-dx, 0, 0);
            }
            else {
                rg.velocity = new Vector3(0, 0, 0);
            }
        }
       
        
    }
}
