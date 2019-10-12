using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    private float camSpeed = 20f;

    public Vector3 newCamPsn;

    // Start is called before the first frame update
    void Start()
    {
        newCamPsn = new Vector3(10, 10, 25);
        enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        // move camera to new position
        if (transform.position != newCamPsn)
        {
            transform.position = Vector3.MoveTowards(transform.position, newCamPsn, camSpeed * Time.deltaTime);
        }
    }
}
