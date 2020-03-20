using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ship_movement : MonoBehaviour
{
    private Rigidbody rb;
    private float angle;
    private bool anchor;
    private bool shoot;
    private healthbar bar;
    public GameObject ball;
    public float ball_force = 1500f;
	public float speed = 10f;
    public float rot_speed = 7f;
    private float health;
    const float damage_scale = 0.1f;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anchor = false;
        shoot = true;
        health = 1f;
        bar = FindObjectOfType<healthbar>();
    }

    // Update is called once per frame

    private void FixedUpdate()
    {

        if (anchor == false)
        {
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
        health -= damage_scale;
        if (health >= 0)
            bar.SetSize(health);
    }

    IEnumerator Shoot()
    {
        shoot = false;
        for (int i = -2; i < 3; i++)
        {
            GameObject new_ball = Instantiate(ball, transform.position + transform.forward * i, Quaternion.identity);
            new_ball.GetComponent<Rigidbody>().AddForce((transform.right + transform.up) * ball_force, ForceMode.Impulse);
        }
        yield return new WaitForSeconds(3);
        shoot = true;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            anchor = !anchor;
        }
        angle = Vector3.Angle(wind_generator.position, transform.forward);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (shoot == true)
            {
                StartCoroutine(Shoot());
            }
        }
    }
}
