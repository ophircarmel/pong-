using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyOnLoad : MonoBehaviour
{
    // <summary>
    // Start is called before the first frame update
    // </summary>
    void Start()
    {
        // Don't destroy when scene change.
        DontDestroyOnLoad(transform);
    }
}
