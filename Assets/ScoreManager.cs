using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour, ballListeners
{
    public Text score1Text;

    public Text score2Text;
    public Text Winner;
    private Move script = null;
    private int score;

    public void heBorn()
    {
        script = GameObject.Find("Sphere").GetComponent<Move>();
    }

    public void MoveBoard(int previous, int next)
    {
    }

    // Start is called before the first frame update
    void Start()
    {
        Winner.enabled = false;
        score1Text.transform.position = new Vector3(10f / (float)Screen.width + 180, Screen.height - 120 - 10f / (float)Screen.height, 0f);
        score2Text.transform.position = new Vector3(Screen.width - 100f, Screen.height - 120 - 10f / (float)Screen.height, 0f);
        transform.parent.Find("ballListenersHolder").GetComponent<holdListeners>().AddListener(this);
        //script = GameObject.Find("Sphere").GetComponent<Move>();
        if (setting.local)
        {
            score = setting.score;
        }
        else
        {
            score = 3;
        }
        score1Text.text = "Player 1: 0";
        score2Text.text = "Player 2: 0";
    }

    // Update is called once per frame
    void Update()
    {
        score1Text.text = "Player 1: " + script.score1;
        score2Text.text = "Player 2: " + script.score2;
        if (script.score1 == score || script.score2 == score)
        {
            Winner.transform.position = new Vector3(Screen.width / 2 - 25, (2f / 3f) * Screen.height, 0);
            if (script.score1 == score)
            {
                Winner.text = "The Winner Is: Player1";
            } else
            {
                Winner.text = "The Winner Is: Player2";
            }
            Winner.enabled = true;
            if (GameObject.Find("Sphere") != null)
              GameObject.Find("Sphere").SetActive(false);
        }
    }
}
