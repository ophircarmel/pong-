using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class putAtTopLeft : MonoBehaviour
{
    public float inX = 80;
    public float inY = 30;
    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(Screen.width - inX, Screen.height - inY, 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
