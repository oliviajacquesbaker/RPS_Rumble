using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public void loadScene(int index)
    {
        SceneManager.LoadScene(index);
    }

    public void loadScene(string name)
    {
        SceneManager.LoadScene(name);
    }

    public void reloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void quitGame()
    {
        Application.Quit();
    }
}
