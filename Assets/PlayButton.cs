using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayButton : MonoBehaviour
{
    public Button button;

    // Start is called before the first frame update
    void Start()
    {
        button.onClick.AddListener(() => { GameObject.Find("Game/Sphere").GetComponent<Move>().Go(); });
        Debug.Log("Done");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
