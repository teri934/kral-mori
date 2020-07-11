using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ship_movement : MonoBehaviour
{
	static GameObject enemy;
	
    private Rigidbody rb;
    private float angle;
    private bool anchor = false;
    private bool shoot = true;
    private float shoot_bar_size = 1f;
    private healthbar health_bar;
    private shootbar shoot_bar;
    private camera_movement camera;
    private int score;

    public Text scoreboard;
    public Text coco_text;
    public Text orange_text;
    public GameObject game_over_panel;
    private GameObject anchor_image;
    public GameObject kaching_sound;
    public GameObject ball;
	
	public static ship_movement objInScene;
	
    public float ball_force = 6f;
	public float speed = 10f;
    public float rot_speed = 7f;
    public int health = 10;
    const int damage_scale = 1;
    private const float shoot_add_interval = 0.25f;

    private int coconuts = 9;
    private int oranges = 5;

    public int Score
    {
        get { return score; }
		set { 
		if(value>score){
			score = value;
			scoreboard.text = "Score: " + score;
		}
	}
    }

    public int counter_coconuts
    {
        get { return coconuts; }
        set { coconuts = value;
            UpdateCounters(); }
    }
    public int counter_oranges
    {
        get { return oranges; }
        set { oranges = value;
            UpdateCounters(); }
    }


    Ray ray;
    RaycastHit hit;
	
	void Awake(){
		FindObjectInScene();
	}
    // Start is called before the first frame update
    void Start()
    {
		enemy = Resources.Load("prefabs/enemy") as GameObject;
        rb = GetComponent<Rigidbody>();
        health_bar = FindObjectOfType<healthbar>();
        shoot_bar = FindObjectOfType<shootbar>();
		camera = FindObjectOfType<camera_movement>();
        anchor_image = GameObject.Find("anchor");
        anchor_image.SetActive(false);

		FindObjectInScene();

        game_over_panel.SetActive(false);

        UpdateCounters();

    }

    // Update is called once per frame
    private void FixedUpdate()
    {

        if (anchor == false)
        {
            rb.AddForce(speed * transform.forward * Mathf.Cos(0.5f * angle * (Mathf.PI / 180)));

            if (Input.GetKey(KeyCode.LeftArrow))
            {
                rb.AddTorque(0f, -rot_speed, 0f);
            }
            if (Input.GetKey(KeyCode.RightArrow))
            {
                rb.AddTorque(0f, rot_speed, 0f);
            }
        }
    }

    private void GameOver()
    {
        Time.timeScale = 0;
        game_over_panel.SetActive(true);
        game_over_panel.transform.Find("endscore").GetComponent<Text>().text = "Your score was " + Score;
        menu_handler.DeleteFiles(WorldLoader.activeMapFilename);
    }

    private void UpdateCounters()
    {
        orange_text.text = "" + counter_oranges;
        coco_text.text = "" + counter_coconuts;
    }
	
	private void TakeDamage(int damage, GameObject other)
    {
        health -= damage;
        if (health > 0)
        {
            camera.Shake(0.002f);
            rb.AddForce(3f * (transform.position - other.transform.position), ForceMode.Impulse);
            RefreshHealth();
        }
        else
        {
            GameOver();
        }
    } 
	
	public void SpawnEnemy(int num)
    {
		List<Vector3> emptySpaces = WhereCanISpawn();
        System.Random rand = new System.Random();
        while (num > 0) 
        {
            Instantiate (enemy, transform.position + emptySpaces[rand.Next(0, emptySpaces.Count)], Quaternion.identity);
            num--;
        }

    }
	
    //Ensures that the enemies are not spawned underneath the islands.
	private List<Vector3> WhereCanISpawn()
    {
        List<Vector3> result = new List<Vector3>();
        int[] coords = WorldLoader.CoordsToMatrix(transform.position);
        string log = "";
        for (int y = 2; y >= -2; y--)
        {
            for (int x = -2; x <= 2; x++)
            {
                log += WorldLoader.ReadFromMap (coords[1] + y, coords[0] + x);
                if (y == 0 && x == 0)
                {
                    continue;
                }
                if (WorldLoader.ReadFromMap (coords[1] + y, coords[0] + x) == 0) 
                {
                    result.Add (new Vector3 (x * 10, -6, y * 10));
                }
            }
            log += '\n';
        }
        return result;
    }

    private void OnCollisionEnter(Collision collision)
    {
        TakeDamage(damage_scale, collision.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "ball")
        {
            if (other.gameObject.GetComponent<ball_collision>().enemy_ball)
            {
				TakeDamage(damage_scale, other.gameObject);
            }
        }
    }
	
	public void RefreshHealth()
    {
		health_bar.SetSize((float)(health / 10.0f));
	}

    IEnumerator Shoot()
    {
        shoot_bar_size = 0f;
        shoot_bar.SetSize(shoot_bar_size);
        shoot = false;
		camera.Shake(0.001f);
        for (int i = -1; i < 2; i++)
        {
            GameObject new_ball = Instantiate(ball, transform.position + transform.forward * 2*i, Quaternion.identity);
            new_ball.GetComponent<Rigidbody>().AddForce((transform.right + transform.up) * ball_force, ForceMode.Impulse);
        }
        counter_coconuts -= 3;
        yield return new WaitForSeconds(4);
        shoot = true;
    }

    void Shooting()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (shoot == true && counter_coconuts >= 3)
            {
                gameObject.GetComponent<AudioSource>().Play();
                StartCoroutine(Shoot());
            }
        }

        if (shoot_bar_size < 1)
        {
            shoot_bar_size += Time.deltaTime * shoot_add_interval;
            shoot_bar.SetSize(shoot_bar_size);
        }
    }

    void Anchor()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            anchor = !anchor;
            if (anchor)
            {
                anchor_image.SetActive(true);
            }
            else
            {
                anchor_image.SetActive(false);
            }
        }
    }

    void Collect()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject.tag == "coconut")
                {
                    kaching_sound.GetComponent<AudioSource>().Play();
                    counter_coconuts++;
                    Score += 5;
                    Destroy(hit.collider.gameObject);
                }
                if (hit.collider.gameObject.tag == "orange")
                {
                    kaching_sound.GetComponent<AudioSource>().Play();
                    counter_oranges++;
                    Score += 5;
                    Destroy(hit.collider.gameObject);
                }
            }
        }
    }

    void AddToHealth()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            if (counter_oranges >= 5 && health < 10)
            {
                counter_oranges -= 5;
				health++;
                RefreshHealth();
            }
        }
    }
	
	static void FindObjectInScene()
    {
		objInScene = FindObjectOfType<ship_movement>();
	}

    void Update()
    {
		if (Time.timeScale == 0)
        {
			return;
		}
		
		if (Input.GetKeyDown(KeyCode.L))
        {
            SpawnEnemy(1);
		}
		
        angle = Vector3.Angle(transform.forward, wind_generator.position);
        Anchor();
        Shooting();
        Collect();
        AddToHealth();
    }
}