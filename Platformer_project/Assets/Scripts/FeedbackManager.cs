using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FeedbackManager : MonoBehaviour
{
    public static FeedbackManager _instance;
    public bool feedbackActivated = true;
    public bool jumpSliderActivated = true;
    public bool redColorWhenDamageActivated = true;
    public bool dustActivated = true;
    public bool healthPointsActivated = true;
    public bool startTextActivated = true;
    public bool victoryActivated = true;
    public bool gameOverActivated = true;
    [SerializeField] private Canvas canvas;
    [SerializeField] private GameObject healthCanvas;
    [SerializeField] private GameObject startText;

    private void Awake()
    {
        FeedbackManager._instance = this;
    }

    private void Update()
    {
        if (feedbackActivated)
        {
            canvas.gameObject.SetActive(true);

            if (healthPointsActivated)
            {
                healthCanvas.SetActive(true);
            }
            else
            {
                healthCanvas.SetActive(false);
            }

            if (startTextActivated)
            {
                startText.SetActive(true);
            }
            else
            {
                startText.SetActive(false);
            }
        }
        else
        {
            canvas.gameObject.SetActive(false);
        }
    }
}
