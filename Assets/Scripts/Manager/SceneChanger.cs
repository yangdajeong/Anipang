using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public void PlayScene()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("GameScene");
    }

    public void HomeScene()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("HomeScene");
    }

    public void GameQuit()
    {
        Application.Quit();
    }
}
