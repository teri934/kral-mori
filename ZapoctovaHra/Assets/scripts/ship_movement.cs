using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    public GameObject game_over_panel;
    private GameObject anchor_image;
    public GameObject kaching_sound;
    public GameObject ball;
	
	public static ship_movement objInScene;
	
    public float ball_force = 6f;
	public float speed = 10f;
    public float rot_speed = 7f;
    public float health = 1f;
    const float damage_scale = 0.1f;
    private const float shoot_add_interval = 0.25f;

    public int counter_oranges = 5;
    public int counter_coconuts = 9;

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
		SpawnEnemies(5);
    }

    // Update is called once per frame

    private void FixedUpdate()
    {

        if (anchor == false)
        {
            if (Input.GetKey(KeyCode.F))    //na skusanie, ked sa nam nechce cakat na vietor 
            {
                rb.AddForce(speed * 2 * transform.forward);
            }

            rb.AddForce(speed * transform.forward + transform.forward * speed * Mathf.Cos(angle * (Mathf.PI / 180)), ForceMode.Force);

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
        menu_handler.DeleteFiles(WorldLoader.activeMapFilename);
    }
	
	private void TakeDamage(float damage, GameObject other)
    {
        health -= damage;
        if (health > 0.0f)
        {
            camera.Shake(0.002f);
            rb.AddForce(3f * (transform.position - other.transform.position), ForceMode.Impulse);
            RefreshHealth();
        }
        else
        {
            GameOver();
            Destroy(camera.GetComponent<AudioListener>());
        }
    } 
	
	private bool EmptySpace(Vector3 direction){
		Ray ray = new Ray(transform.position, direction);
		if (Physics.Raycast(ray, out RaycastHit hit, 50))
		{
			return false;
		}
		else{
			return true;
		}
	}
	
	//TODO!!!
	public void SpawnEnemies(int count){
		for(int i = 0; i< count; i++){
			if(EmptySpace(new Vector3(10*i,0,10*i))){
				Instantiate(enemy, transform.position + new Vector3(10*i,0,10*i), Quaternion.identity);
			}
		}
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
		health_bar.SetSize(health);
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
                    counter_coconuts += 1;
                    Destroy(hit.collider.gameObject);
                }
                if (hit.collider.gameObject.tag == "orange")
                {
                    kaching_sound.GetComponent<AudioSource>().Play();
                    counter_oranges += 1;
                    Destroy(hit.collider.gameObject);
                }
            }
        }
    }

    void AddToHealth()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            if (counter_oranges >= 5 && health < 1)
            {
                counter_oranges -= 5;
                if (health <= 0.95f)
                {
                    kaching_sound.GetComponent<AudioSource>().Play();
                    health += 0.05f;
                }
                else if (health <= 1)
                {
                    kaching_sound.GetComponent<AudioSource>().Play();
                    health += 1 - health;
                }
                RefreshHealth();
            }
        }
    }
	
	static void FindObjectInScene(){
		objInScene = FindObjectOfType<ship_movement>();
	}

    void Update()
    {
		if(Time.timeScale == 0){
			return;
		}
        angle = Vector3.Angle(transform.forward, wind_generator.position);
        Anchor();
        Shooting();
        Collect();
        AddToHealth();
    }
}