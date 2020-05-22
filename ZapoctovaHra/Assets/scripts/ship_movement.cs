using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ship_movement : MonoBehaviour
{
    private Rigidbody rb;
    private float angle;
    private bool anchor = false;
    private bool shoot = true;
    private float shoot_bar_size = 1f;
    private healthbar health_bar;
    private shootbar shoot_bar;
    private camera_movement camera;

    private GameObject anchor_image;
    public GameObject kaching_sound;
    public GameObject ball;
	public static ship_movement objInScene;
    public float ball_force = 1500f;
	public float speed = 10f;
    public float rot_speed = 7f;
    public float health = 1f;
    const float damage_scale = 0.1f;
    private const float shoot_add_interval = 0.25f;

    public int counter_oranges = 5;
    public int counter_coconuts = 9;

    Ray ray;
    RaycastHit hit;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        health_bar = FindObjectOfType<healthbar>();
        shoot_bar = FindObjectOfType<shootbar>();
		camera = FindObjectOfType<camera_movement>();
        anchor_image = GameObject.Find("anchor");
		
        anchor_image.SetActive(false);
		FindObjectinScene();
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
	
	private void TakeDamage(float damage){
		health -= damage;
        if (health > 0.0f)
            RefreshHealth();
		else{
			Time.timeScale = 0;
			Destroy(camera.GetComponent<AudioListener>());
		}
	}

    private void OnCollisionEnter(Collision collision)
    {
		rb.AddForce(10f * - transform.forward, ForceMode.Impulse);
		camera.Shake(0.002f);
        TakeDamage(damage_scale);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "ball")
        {
            if (other.gameObject.GetComponent<ball_collision>().enemy_ball)
            {
                rb.AddForce(10f * -transform.forward, ForceMode.Impulse);
				TakeDamage(damage_scale);
            }
            camera.Shake(0.002f);
        }
    }
	
	public void RefreshHealth(){
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
        counter_coconuts -= 1;
        yield return new WaitForSeconds(4);
        shoot = true;
    }

    void Shooting()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (shoot == true && counter_coconuts > 0)
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
	
	static void FindObjectinScene(){
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
    }
}