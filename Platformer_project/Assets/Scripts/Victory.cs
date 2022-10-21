using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Victory : MonoBehaviour
{
    [SerializeField] Text text;
    [SerializeField] EventSystem eventSystem;
    [SerializeField] GameObject victoryCanvas;
    [SerializeField] GameObject mainMenuButton;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            text.gameObject.SetActive(true);
            eventSystem.SetSelectedGameObject(mainMenuButton);
            Time.timeScale = 0;
            victoryCanvas.SetActive(true);
        }
    }
}
