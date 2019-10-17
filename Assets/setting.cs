using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class setting : MonoBehaviour
{
    public static string Player1Down = "down";
    public static string Player1Up = "up";
    public static string Player2Down = "s";
    public static string Player2Up = "w";
    public static bool twoPlayers = true;
    public InputField p1up;
    public InputField p1down;
    public InputField p2up;
    public InputField p2down;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void setPlayer1Up()
    {
        Player1Up = p1up.text;
    }
    public void setPlayer2Up()
    {
        Player2Up = p2up.text;
    }
    public void setPlayer1Down()
    {
        Player1Down = p1down.text;
    }
    public void setPlayer2Down()
    {
        Player2Down = p2down.text;
    }
    public void set2players(bool is2p)
    {
        twoPlayers = is2p;
    }
}
