using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenuUI;
    [SerializeField] private GameObject controlsMenuUI;
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
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1;
    }

    public void ShowControlsMenu()
    {
        controlsMenuUI.SetActive(true);
    }
    public void LoadMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }
}
