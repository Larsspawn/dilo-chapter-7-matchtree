using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public UnityEvent OnGameOver;
    public UnityEvent OnTimerNearEnd;

    public void GoToScene(int index)
    {
        SceneManager.LoadScene(index);
    }

    public void PauseGame(bool state)
    {
        GameManager.instance.isPaused = state;
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("QUIT");
    }
}
