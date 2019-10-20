using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour, IMoveBoardListener
{
    private static readonly int CAMERAHEIGHT = 15;

    private static readonly int BOARDWIDTH = 25;

    private static readonly int BOARDLENGTH = 50;

    private float camSpeed = 20f;

    public Vector3 newCamPsn;

    // Start is called before the first frame update
    void Start()
    {
        newCamPsn = new Vector3(10, 15, 25);
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


    // <summary>
    // Move to another board.
    // </summary>
    // <param name="prevoius"> The board to move from. </param>
    // <param name="next"> The board to move to. </param>
    public void MoveBoard(int prevoius, int next)
    {
        // Note: given a board indexed as n, the board is GameObject((n-1)/3, (n-1)%3).
        int boardz = (next - 1) / 3;
        int boardx = (next - 1) % 3;

        // Calculate next position of the camera.
        float nx = (boardx - 1) * (BOARDWIDTH + 1) + BOARDWIDTH / 2;
        int ny = CAMERAHEIGHT;
        float nz = (1 - boardz) * (BOARDLENGTH + 1) + BOARDLENGTH / 2;

        this.newCamPsn = new Vector3(nx, ny, nz);
    }
}
