using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class holdListeners : MonoBehaviour
{
    public List<ballListeners> list = new List<ballListeners>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void AddListener(ballListeners go)
    {
        list.Add(go);
        Debug.Log(go.ToString());
    }
}
