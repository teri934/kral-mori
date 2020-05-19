using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

public class menu_handler : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject main_menu_panel;
    public GameObject continue_game_panel;
    public GameObject new_game_panel;
    public GameObject delete_world_panel;
    public InputField input_field;
    public Text choose_world;
    public string world_name;
    public Text invalid;
    public Text generate_name;
    int index;
    string[] files = Directory.GetFiles("Saves","*.world");
    const string no_worlds = "No worlds created yet";
    void Start()
    {
        main_menu_panel.SetActive(true);
        continue_game_panel.SetActive(false);
        new_game_panel.SetActive(false);
        delete_world_panel.SetActive(false);

        ControlFiles();
    }
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void ControlFiles()
    {
        if (files.Length != 0)
        {
            index = 0;
            choose_world.text = Path.GetFileNameWithoutExtension(files[0]);
        }
        else
        {
            choose_world.text = no_worlds;
        }
    }

    public void BackControl()
    {
        main_menu_panel.SetActive(true);
        continue_game_panel.SetActive(false);
        new_game_panel.SetActive(false);
    }

    public void DeleteWorldControl()
    {
        if (choose_world.text != no_worlds)
        {
            delete_world_panel.SetActive(true);
        }
    }

    public void NoControl()
    {
        delete_world_panel.SetActive(false);
    }

    public void YesControl()
    {
        File.Delete("Saves/" + choose_world.text + ".world");
		File.Delete("Saves/" + choose_world.text + ".state");
        files = Directory.GetFiles("Saves","*.world");
        ControlFiles();
        delete_world_panel.SetActive(false);
    }

    public void ContinueGameControl()
    {
        main_menu_panel.SetActive(false);
        continue_game_panel.SetActive(true);
    }

    public void ArrowControl()
    {
        if (files.Length != 0)
        {
            index = (index + 1) % files.Length;
            choose_world.text = Path.GetFileNameWithoutExtension(files[index]);
        }
    }

    public void ChooseWorldControl()
    {
        if (choose_world.text != no_worlds)
        {
            world_name = choose_world.text;
            SceneManager.LoadScene("Game");
        }
    }

    public void NewGameControl()
    {
        main_menu_panel.SetActive(false);
        new_game_panel.SetActive(true);
    }

    public void GenerateWorld()
    {
        world_name = input_field.text;
        if (!string.IsNullOrEmpty(world_name) && world_name.IndexOfAny(Path.GetInvalidFileNameChars()) < 0)
        {
            SceneManager.LoadScene("Game");
        }
        else
        {
            invalid.gameObject.SetActive(true);
        }
    }

    public void HideInvalid()
    {
        invalid.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
    }
}
