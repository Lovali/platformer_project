using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool gameIsPaused = false;
    [SerializeField] GameObject pauseMenuUI;
    [SerializeField] GameObject controlsMenuUI;
    public bool pausePressed;

    void Update()
    {
        if(pausePressed)
        {
            Paused();
        }
        if(controlsMenuUI.activeSelf)
        {
            InputSystem.onAnyButtonPress.CallOnce(ctrl => controlsMenuUI.SetActive(false));
        }
    }

    void Paused()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0;
        gameIsPaused = true;
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1;
        gameIsPaused = false;
    }
    public void LoadMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }
}
