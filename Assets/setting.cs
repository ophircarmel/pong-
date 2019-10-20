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

    public static float HardLevel;
    internal static int score = 3;
    public static bool local = true;

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
        if (p1up != null && p1up.text.Length > 0)
            Player1Up = p1up.text;
        else
            Player1Up = "up";
    }
    public void setPlayer2Up()
    {
        if (p2up != null && p2up.text.Length > 0)
            Player2Up = p2up.text;
        else
            Player2Up = "w";
    }
    public void setPlayer1Down()
    {
        if (p1down != null && p1down.text.Length > 0)
            Player1Down = p1down.text;
        else
            Player1Down = "down";
    }
    public void setPlayer2Down()
    {
        if (p2up != null && p2down.text.Length > 0)
            Player2Down = p2down.text;
        else
            Player2Down = "s";
    }
    public void set2players(bool is2p)
    {
            twoPlayers = !is2p;
    }

    public void setHardLevel(Slider s)
    {
        if (s != null)
            HardLevel = s.value;
        else
            HardLevel = 50;
    }
    public void defaultAll()
    {
        Player1Down = "down";
        Player1Up = "up";
        Player2Down = "s";
        Player2Up = "w";
        twoPlayers = true;
        HardLevel = 50;
        score = 3;
        local = true;
    }
    public void setScore(InputField s)
    {
        score = int.Parse(s.text);
    }
    public void setLocal(bool isLocal)
    {
        local = isLocal;
    }
}
