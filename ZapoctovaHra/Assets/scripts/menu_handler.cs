using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

public class menu_handler : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject click_sound;
    public GameObject main_menu_panel;
    public GameObject continue_game_panel;
    public GameObject new_game_panel;
    public GameObject delete_world_panel;
    public InputField input_field;
    public Text choose_world;
	
    public string world_name;
	public int newworld_size;
	private string[] files;
	
    public Text invalid;
    public Text generate_name;
	public Text size_slider_text;
    int index;
    const string no_worlds = "No worlds created yet";
	
    void Start()
    {
        main_menu_panel.SetActive(true);
        continue_game_panel.SetActive(false);
        new_game_panel.SetActive(false);
        delete_world_panel.SetActive(false);
		if(Directory.Exists("Saves")){
			files = Directory.GetFiles("Saves","*.world");
		}
		else{
			Directory.CreateDirectory("Saves");
			files = new string[0];
		}
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
        click_sound.GetComponent<AudioSource>().Play();
        main_menu_panel.SetActive(true);
        continue_game_panel.SetActive(false);
        new_game_panel.SetActive(false);
    }

    public void DeleteWorldControl()
    {
        click_sound.GetComponent<AudioSource>().Play();
        if (choose_world.text != no_worlds)
        {
            delete_world_panel.SetActive(true);
        }
    }

    public void NoControl()
    {
        click_sound.GetComponent<AudioSource>().Play();
        delete_world_panel.SetActive(false);
    }

    public void YesControl()
    {
        click_sound.GetComponent<AudioSource>().Play();
        DeleteFiles("Saves/" + choose_world.text);
        files = Directory.GetFiles("Saves", "*.world");
        ControlFiles();
        delete_world_panel.SetActive(false);
    }

    public static void DeleteFiles(string name)
    {
        File.Delete(name + ".world");
        File.Delete(name + ".state");
    }

    public void ContinueGameControl()
    {
        click_sound.GetComponent<AudioSource>().Play();
        main_menu_panel.SetActive(false);
        continue_game_panel.SetActive(true);
    }

    public void Exit() {
        Application.Quit(0);
    }

    public void ArrowControl()
    {
        click_sound.GetComponent<AudioSource>().Play();
        if (files.Length != 0)
        {
            index = (index + 1) % files.Length;
            choose_world.text = Path.GetFileNameWithoutExtension(files[index]);
        }
    }

    public void ChooseWorldControl()
    {
        click_sound.GetComponent<AudioSource>().Play();
        if (choose_world.text != no_worlds)
        {
            world_name = choose_world.text;
            SceneManager.LoadScene("Game");
        }
    }

    public void NewGameControl()
    {
        click_sound.GetComponent<AudioSource>().Play();
        main_menu_panel.SetActive(false);
        new_game_panel.SetActive(true);
    }
	
	public void NewWorldSlider(System.Single size){
		size_slider_text.text = "World size:    " + size + "     (approx." + (int)(Mathf.Pow((2 << (int)size - 1), 2) / 1000) + "kB.)";
		newworld_size = 2 << ((int) size - 1);
	}

    public void GenerateWorld()
    {
        click_sound.GetComponent<AudioSource>().Play();
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
