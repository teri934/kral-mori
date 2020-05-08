using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class panel_handler : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject pause_panel;
    public GameObject game_over_panel;
    void Start()
    {
        Time.timeScale = 1;
        game_over_panel.SetActive(false);
        pause_panel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Time.timeScale == 1)
            {
                Time.timeScale = 0;
                pause_panel.SetActive(true);
            }
            else
            {
                Time.timeScale = 1;
                pause_panel.SetActive(false);
            }
        }
    }

    public void ResumeControl()
    {
        if (Time.timeScale == 0)
        {
            Time.timeScale = 1;
            pause_panel.SetActive(false);
        }
    }

    public void MainMenuControl()
    {
        if (Time.timeScale == 0)
        {
            pause_panel.SetActive(false);
            SceneManager.LoadScene("MainMenu");
        }
    }

    public void GameOver()
    {
        Time.timeScale = 0;
        game_over_panel.SetActive(true);
    }
}
