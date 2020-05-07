using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class panel_handler : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject PausePanel;
    public GameObject GameOverPanel;
    void Start()
    {
        Time.timeScale = 1;
        GameOverPanel.SetActive(false);
        PausePanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Time.timeScale == 1)
            {
                Time.timeScale = 0;
                ShowPaused();
            }
            else
            {
                Time.timeScale = 1;
                HidePaused();
            }
        }
    }

    public void ShowPaused()
    {
        PausePanel.SetActive(true);
    }

    public void HidePaused()
    {
        PausePanel.SetActive(false);
    }

    public void PauseControl()
    {
        if (Time.timeScale == 0)
        {
            Time.timeScale = 1;
            HidePaused();
        }
    }

    public void GameOver()
    {
        Time.timeScale = 0;
        GameOverPanel.SetActive(true);
    }
}
