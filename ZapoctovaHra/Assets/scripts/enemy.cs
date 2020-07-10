using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy : MonoBehaviour
{
	Rigidbody rb;
	ship_movement player;
	float speed = 5f;
	float windAngle;
	Vector3 playerVect;
	int health = 3;
	
	public GameObject ball;
	float ball_force = 6f;
	
	float lastShootTime;
	const int shootDelay = 4;

    // Start is called before the first frame update
    void Start()
    {
		player = ship_movement.objInScene;
		Debug.Log("Enemy: "+ player.transform.position);
		rb = GetComponent<Rigidbody>();
    }
	
	Vector3 vectorToPlayer(){
		return (player.transform.position - transform.position);
	}
	
	bool InSight()
	{
		Ray ray = new Ray(transform.position, transform.right);
		if (Physics.Raycast(ray, out RaycastHit hit, 50))
		{
			return true;
		}
		else
		{
			return false;
		}
	}
	
	void TakeDamage(GameObject other)
	{
		if(health > 0)
		{
			rb.AddForce(3f * (transform.position - other.gameObject.transform.position), ForceMode.Impulse);
			health--;
			Debug.Log(health);
		}
	}
	
	void Sink()
	{
		transform.Translate(-2 * Vector3.up * Time.deltaTime);
		Debug.Log("Sinking");
		if(transform.position.y < -3){
			ship_movement.objInScene.SpawnEnemy(1);
			ship_movement.objInScene.AddToScore(100);
			Destroy(gameObject);
		}
	}

	void Emerge()
	{
		transform.Translate(Vector3.up * Time.deltaTime);
		Debug.Log("Emerging");
	}
	
	private void OnCollisionEnter(Collision collision)
    {
        TakeDamage(collision.gameObject);
    }
	
	private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "ball")
        {
            if (!other.gameObject.GetComponent<ball_collision>().enemy_ball)
            {
				TakeDamage(other.gameObject);
            }
        }
    }
	
	void Shoot(){
        for (int i = -1; i < 2; i++)
        {
            GameObject new_ball = Instantiate(ball, transform.position + transform.forward * 2 * i, Quaternion.identity);
			new_ball.GetComponent<ball_collision>().enemy_ball = true;
            new_ball.GetComponent<Rigidbody>().AddForce((transform.right + transform.up) * ball_force, ForceMode.Impulse);
        }
		lastShootTime = Time.time;
		gameObject.GetComponent<AudioSource>().Play();
	}


    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < 3)
        {
			Emerge();
        }

		if (health < 1)
		{
			Sink();
			return;
		}
		playerVect = vectorToPlayer();
		windAngle = Vector3.SignedAngle(transform.forward, wind_generator.position, Vector3.up);
		
		if (Vector3.Angle (transform.forward, playerVect) < 90)
		{
			rb.AddTorque(0.02f * Vector3.SignedAngle(transform.forward, playerVect - player.transform.right * 2, Vector3.up) * Vector3.up);
		}
		else
		{
			rb.AddTorque(0.02f * Vector3.SignedAngle(transform.forward, playerVect + player.transform.right * 2, Vector3.up) * Vector3.up);
		}

		if (playerVect.sqrMagnitude < 100)
		{
			if (InSight() && Time.time - lastShootTime > shootDelay)
			{
				Shoot();
			}
		}
		rb.AddForce(speed * transform.forward * Mathf.Cos(0.5f * windAngle * (Mathf.PI / 180)));
    }
}
