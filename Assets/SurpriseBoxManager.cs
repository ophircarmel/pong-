using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SurpriseBoxManager : MonoBehaviour, IMoveBoardListener
{
    // text to display
    public Text effectText;

    // is an effect taking place
    private bool isEffect;

    // time to fade
    private float timeToFade = 1f;

    // time for cube effects
    private float effectTime;

    // time for cube generation
    private float time;

    // height of cube
    private float height = 0.5f;

    // current board
    private int currBoard;

    // 0 means neutral, 1 means good for p1, 2 means good for p2.
    private int neutrality;

    // Start is called before the first frame update
    void Start()
    {
        neutrality = 0;

        time = Constants.SURPRISE_BOX_SPAWN_TIME;

        transform.position = GenerateSurpriseCubeLocation();

        currBoard = 5;

        effectText.transform.position = new Vector3(Screen.width / 2, Screen.height - 20 - 10f / (float)Screen.height, 0f);

    }

    // Update is called once per frame
    void Update()
    {
        HandleEffectTime();
        HandleSpawnTime();
        transform.Rotate(new Vector3(1, 1, 1));
    }

    // handle spawning time
    public void HandleSpawnTime()
    {
        time -= Time.deltaTime;

        // failed attempt at doing fade in and out which resulted in the box turning purple
        // |
        // |
        // V

        
        if (time >= 4)
        {
            
            Color alphaColor = GetComponent<MeshRenderer>().material.color;
            alphaColor.a = 1f;

            GetComponent<MeshRenderer>().material.color = 
                Color.Lerp(GetComponent<MeshRenderer>().material.color, alphaColor, timeToFade * Time.deltaTime);
            
        } else if (time <= 1 && time >= 0)
        {
            Color alphaColor = GetComponent<MeshRenderer>().material.color;
            alphaColor.a = 0f;

            GetComponent<MeshRenderer>().material.color =
                Color.Lerp(GetComponent<MeshRenderer>().material.color, alphaColor, timeToFade * Time.deltaTime);
            
        } else
        


        // if we should spawn new one
        if (time <= -Constants.INTERVAL_TIME)
        {
            time = Constants.SURPRISE_BOX_SPAWN_TIME;

            // random bool
            int bit = Random.Range(0, 2);

            if (bit == 0)
            {
                // neutral box
                // GetComponent<MeshRenderer>().material.shader = Shader.Find("Yellow");
                //GetComponent<MeshRenderer>().material.shader = null;
                neutrality = 0;
            }
            else
            {
                // oriented box
                //GetComponent<MeshRenderer>().material.shader = Shader.Find("Green");
                //GetComponent<MeshRenderer>().material.shader = null;
                neutrality = 1;
            }
            transform.GetComponent<Renderer>().enabled = true;
            transform.FindChild("box").GetComponent<Renderer>().enabled = true;
            // generate the cube
            transform.position = GenerateSurpriseCubeLocation();
        }
    }

    // handle the time of the effects
    public void HandleEffectTime()
    {

        if (isEffect)
        {
            effectTime -= Time.deltaTime;

            // if effect time is up
            if (effectTime <= 0)
            {
                // text is nothing
                effectText.text = "";

                // return board to normal
                NormalizeBoard();
                isEffect = false;

            }

        }


        // if effect time is up
        if (effectTime <= 0)
        {
            // text is nothing
            effectText.text = "";
            if (isEffect)
            {

                // return board to normal
                NormalizeBoard();
                isEffect = false;
            }

        }
    }

    // removes effects
    public void NormalizeBoard()
    {
        Debug.Log("normal?");
        // return sphere scale to normal
        GameObject.Find("Sphere").transform.localScale = Vector3.one;

        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                GameObject board = GameObject.Find("GameObject (" + i + "," + j + ")");
                GameObject player1 = board.transform.Find("player1").gameObject;
                GameObject player2 = board.transform.Find("player2").gameObject;
                Vector3 temp;

                // normalize scale of players
                temp = player1.transform.localScale;
                temp.x = 5;
                player1.transform.localScale = temp;

                temp = player2.transform.localScale;
                temp.x = 5;
                player2.transform.localScale = temp;


                // normalize speeds
                //temp = player1.GetComponent<Rigidbody>().velocity;
                temp.x = Constants.PLAYER_SPEED;
                player1.GetComponent<playerMove>().dx = temp.x;
                if (setting.twoPlayers)
                    player2.GetComponent<playerMove>().dx = temp.x;
                else
                    player2.GetComponent<ComputerMove>().dx = temp.x;
                /*temp = player2.GetComponent<Rigidbody>().velocity;
                temp.x = Constants.PLAYER_SPEED;
                player2.GetComponent<Rigidbody>().velocity = temp;*/

                // normalize buttons
                player1.GetComponent<playerMove>().up = setting.Player1Up;
                player1.GetComponent<playerMove>().down = setting.Player1Down;

                player2.GetComponent<playerMove>().up = setting.Player2Up;
                player2.GetComponent<playerMove>().down = setting.Player2Down;
            }
        }
    }

    // when the cube is hit
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject != GameObject.Find("Sphere") || !transform.GetComponent<Renderer>().isVisible)
        {
            return;
        }

        // if collided with ball

        int boardx = (currBoard - 1) / 3;
        int boardz = (currBoard - 1) % 3;

        // make effect
        effectTime = Constants.EFFECT_TIME;
        isEffect = true;

        GameObject sphere = GameObject.Find("Sphere");
        GameObject board = GameObject.Find("GameObject (" + boardx + "," + boardz + ")");
        GameObject player1 = board.transform.Find("player1").gameObject;
        GameObject player2 = board.transform.Find("player2").gameObject;
        Vector3 p1Scale = player1.transform.localScale;
        Vector3 p2Scale = player2.transform.localScale;


        if (neutrality == 1)
        {
            // finding velocity
            Vector3 velocity = sphere.GetComponent<Rigidbody>().velocity;
            if (velocity.z > 0)
            {
                // helps player 1
                neutrality = 1;
            }
            else
            {
                // helps player 2
                neutrality = 2;
            }
        }

        switch (neutrality)
        {
            // neutral box
            case 0:
                int rand0 = Random.Range(0, 7);

                Vector3 scale = sphere.transform.localScale;
                switch (rand0)
                {
                    case 0:
                        // ball grows
                        Debug.Log("Ball grows");

                        effectText.text = "Ball grows";
                        sphere.transform.localScale = scale * 4f;
                        break;

                    case 1:
                        // ball shrinks
                        Debug.Log("Ball shrinks");

                        effectText.text = "Ball shrinks";
                        sphere.transform.localScale = scale * 0.25f;
                        break;

                    case 2:
                        // paddles grow
                        Debug.Log("Paddles grow");

                        effectText.text = "Paddles grow";
                        p1Scale.x *= 2;
                        p2Scale.x *= 2;

                        player1.transform.localScale = p1Scale;
                        player2.transform.localScale = p2Scale;

                        break;

                    case 3:
                        // paddles shrink
                        Debug.Log("Paddles shrink");

                        effectText.text = "Paddles shrink";
                        p1Scale.x *= 0.5f;
                        p2Scale.x *= 0.5f;

                        player1.transform.localScale = p1Scale;
                        player2.transform.localScale = p2Scale;

                        break;

                    case 4:
                        // paddles quicker
                        Debug.Log("Paddles quicker");

                        effectText.text = "Paddles quicker";
                        player1.GetComponent<playerMove>().dx *= 2;
                        if (setting.twoPlayers)
                            player2.GetComponent<playerMove>().dx *= 2;
                        else
                            player2.GetComponent<ComputerMove>().dx *= 2;
                        break;

                    case 5:
                        // paddles slower
                        Debug.Log("Paddles slower");

                        effectText.text = "Paddles slower";
                        player1.GetComponent<playerMove>().dx *= 0.5f;
                        if (setting.twoPlayers)
                            player2.GetComponent<playerMove>().dx *= 0.5f;
                        else
                            player2.GetComponent<ComputerMove>().dx *= 0.5f;

                        break;

                    case 6:
                        // buttons change
                        Debug.Log("Buttons change");

                        effectText.text = "Buttons change";
                        string temp;
                        temp = player1.GetComponent<playerMove>().up;
                        player1.GetComponent<playerMove>().up = player1.GetComponent<playerMove>().down;
                        player1.GetComponent<playerMove>().down = temp;

                        temp = player2.GetComponent<playerMove>().up;
                        player2.GetComponent<playerMove>().up = player2.GetComponent<playerMove>().down;
                        player2.GetComponent<playerMove>().down = temp;
                        break;
                }
                break;

            case 1:

                int rand1 = Random.Range(0, 5);

                // helps player 1
                switch (rand1)
                {
                    case 0:
                        Debug.Log("P1 grows");
                        // p1 grows
                        effectText.text = "P1 grows";
                        p1Scale.x *= 2f;

                        player1.transform.localScale = p1Scale;

                        break;
                    case 1:
                        Debug.Log("P2 shrinks");
                        // p2 shrinks
                        effectText.text = "P2 shrinks";
                        p2Scale.x *= 0.5f;

                        player2.transform.localScale = p2Scale;

                        break;
                    case 2:
                        Debug.Log("P1 quicker");
                        // p1 faster
                        effectText.text = "P1 quicker";
                        player1.GetComponent<playerMove>().dx *= 2f;

                        break;
                    case 3:
                        Debug.Log("P2 slower");
                        // p2 slower
                        effectText.text = "P2 slower";
                        if (setting.twoPlayers)
                            player2.GetComponent<playerMove>().dx *= 0.5f;
                        else
                            player2.GetComponent<ComputerMove>().dx *= 0.5f;


                        break;
                    case 4:
                        Debug.Log("P2 buttons change");
                        // p2 changes buttons
                        effectText.text = "P2 buttons change";
                        string temp;
                        temp = player2.GetComponent<playerMove>().up;
                        player2.GetComponent<playerMove>().up = player2.GetComponent<playerMove>().down;
                        player2.GetComponent<playerMove>().down = temp;

                        break;
                }
                break;

            case 2:
                int rand2 = Random.Range(0, 5);

                // helps player 2
                switch (rand2)
                {
                    case 0:
                        Debug.Log("P2 grows");
                        // p2 grows
                        effectText.text = "P2 grows";
                        p2Scale.x *= 2f;

                        player2.transform.localScale = p2Scale;
                        break;
                    case 1:
                        Debug.Log("P1 shrinks");
                        // p1 shrinks
                        effectText.text = "P1 shrinks";
                        p1Scale.x *= 0.5f;

                        player1.transform.localScale = p1Scale;
                        break;
                    case 2:
                        Debug.Log("P2 quicker");
                        // p2 faster
                        effectText.text = "P2 quicker";
                        if (setting.twoPlayers)
                            player2.GetComponent<playerMove>().dx *= 2f;
                        else
                            player2.GetComponent<ComputerMove>().dx *= 2f;


                        break;
                    case 3:
                        Debug.Log("P1 slower");
                        // p1 slower
                        effectText.text = "P1 slower";
                        player1.GetComponent<playerMove>().dx *= 0.5f;

                        break;
                    case 4:
                        Debug.Log("P1 buttons change");
                        // p1 changes buttons
                        effectText.text = "P1 buttons change";
                        string temp;
                        temp = player1.GetComponent<playerMove>().up;
                        player1.GetComponent<playerMove>().up = player1.GetComponent<playerMove>().down;
                        player1.GetComponent<playerMove>().down = temp;

                        break;
                }
                break;
            default:
                throw new System.Exception("Fatal Error - Neutrality not initialized");
        }
        transform.GetComponent<Renderer>().enabled = false;
        transform.FindChild("box").GetComponent<Renderer>().enabled = false;

    }

    // <summary>
    // Generate location on current board.
    public Vector3 GenerateSurpriseCubeLocation()
    {

        int boardz = (currBoard - 1) / 3;
        int boardx = (currBoard - 1) % 3;

        float nx = (boardx - 1) * (Constants.BOARDWIDTH + 1) + Constants.BOARDWIDTH / 2;

        float nz = (1 - boardz) * (Constants.BOARDLENGTH + 1) + Constants.BOARDLENGTH / 2;

        //Debug.Log("nx and nz are " + nx + " " + nz);

        float xMin = nx - (Constants.BOARDWIDTH * 0.5f);
        xMin += transform.localScale.x;
        xMin += GameObject.Find("Cube (3)").transform.localScale.x;

        float xMax = nx + (Constants.BOARDWIDTH * 0.5f);
        xMax -= transform.localScale.x;
        xMax -= GameObject.Find("Cube (3)").transform.localScale.x;

        float zMin = nz - (Constants.BOARDLENGTH * 0.5f);
        zMin += transform.localScale.z;
        zMin += GameObject.Find("wall2").transform.localScale.z;

        float zMax = nz + (Constants.BOARDLENGTH * 0.5f);
        zMax -= transform.localScale.z;
        zMax -= GameObject.Find("wall1").transform.localScale.z;

        // random in board
        float randx = Random.Range(xMin + 3, xMax - 3);

      //  Debug.Log(nx + " " + nz);

       // Debug.Log("xrange - " + xMin + " " + xMax + " " + randx);

        // random in board z
        float randz = Random.Range(zMin + 3, zMax - 3);

       // Debug.Log("zrange - " + zMin + " " + zMax + " " + randz);

        Vector3 location = new Vector3(randx, height, randz);

        //Debug.Log(location);

        return location;

    }

    // <summary>
    // Move to another board.
    // </summary>
    // <param name="previous"> The board to move from. </param>
    // <param name="next"> The board to move to. </param>
    public void MoveBoard(int previous, int next)
    {
        NormalizeBoard();
        currBoard = next;
        effectTime = 0f;
        isEffect = true;
        time = 0f;
    }
}
