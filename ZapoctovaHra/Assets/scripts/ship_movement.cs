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
	private camera_movement Camera;
	
    public GameObject ball;
    public float ball_force = 1500f;
	public float speed = 10f;
    public float rot_speed = 7f;
    private float health = 1f;
    const float damage_scale = 0.1f;
    private const float shoot_add_interval = 0.25f;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        health_bar = FindObjectOfType<healthbar>();
        shoot_bar = FindObjectOfType<shootbar>();
		Camera = FindObjectOfType<camera_movement>();
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

    private void OnCollisionEnter(Collision collision)
    {
		rb.AddForce(10f * - transform.forward, ForceMode.Impulse);
		Camera.Shake(0.002f);
        health -= damage_scale;
        if (health >= 0)
            health_bar.SetSize(health);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "ball")
        {
            rb.AddForce(10f * -transform.forward, ForceMode.Impulse);
            Camera.Shake(0.002f);
            health -= damage_scale;
            if (health >= 0)
                health_bar.SetSize(health);
        }
    }

    IEnumerator Shoot()
    {
        shoot_bar_size = 0f;
        shoot_bar.SetSize(shoot_bar_size);
        shoot = false;
		Camera.Shake(0.001f);
        for (int i = -1; i < 2; i++)
        {
            GameObject new_ball = Instantiate(ball, transform.position + transform.forward * i, Quaternion.identity);
            new_ball.GetComponent<Rigidbody>().AddForce((transform.right + transform.up) * ball_force, ForceMode.Impulse);
        }
        yield return new WaitForSeconds(4);
        shoot = true;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            anchor = !anchor;
        }
        angle = Vector3.Angle(transform.forward, wind_generator.position);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (shoot == true)
            {
                StartCoroutine(Shoot());
            }
        }

        if (shoot_bar_size < 1)
        {
            shoot_bar_size += Time.deltaTime * shoot_add_interval;
            shoot_bar.SetSize(shoot_bar_size);
        }
    }
}