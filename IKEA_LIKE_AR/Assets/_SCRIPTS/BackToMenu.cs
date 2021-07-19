using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToMenu : MonoBehaviour
{
    public void OpenScene(string _scene)
    {
        SceneManager.LoadScene(_scene);
    }

    public void ExitApplication()
    {
        Application.Quit();
    }
}
