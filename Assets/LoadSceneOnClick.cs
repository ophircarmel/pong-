using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LoadSceneOnClick : MonoBehaviour
{
    // <summary>
    // Load a scene.
    // </summary>
    public void LoadScene(string sceneName)
    {
        // Load next scene.
        SceneManager.LoadScene(sceneName);
    }
}
