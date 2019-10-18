using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ExitOnClick : MonoBehaviour
{
    // <summary>
    // Handle the case of clicking exit button.
    // </summary>
    public void ExitGame()
    {
        // Close the application.
        Application.Quit();
    }
}
