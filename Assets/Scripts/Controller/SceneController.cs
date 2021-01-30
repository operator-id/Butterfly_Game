using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneController : MonoBehaviour
{

    public void ResetScene(int sceneNumber)
    {
        Application.LoadLevel(sceneNumber);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

}
