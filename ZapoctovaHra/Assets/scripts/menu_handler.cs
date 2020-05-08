using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class menu_handler : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject main_menu_panel;
    public GameObject continue_game_panel;
    public GameObject new_game_panel;
    public InputField input_field;
    public string world_name;
    void Start()
    {
        main_menu_panel.SetActive(true);
        continue_game_panel.SetActive(false);
        new_game_panel.SetActive(false);
    }
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void ContinueGameControl()
    {
        main_menu_panel.SetActive(false);
        continue_game_panel.SetActive(true);
    }

    public void NewGameControl()
    {
        main_menu_panel.SetActive(false);
        new_game_panel.SetActive(true);
    }

    public void GenerateWorld()
    {
        world_name = input_field.text;
        SceneManager.LoadScene("Game");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
