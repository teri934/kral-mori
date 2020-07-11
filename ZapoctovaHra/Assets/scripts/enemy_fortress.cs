using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy_fortress : MonoBehaviour
{
    public GameObject ball;
    private GameObject ship;
    private float shoot_interval = 3f;
    private bool shoot = true;
    // Start is called before the first frame update
    void Start()
    {
        ship = ship_movement.objInScene.gameObject;
    }

    // Update is called once per frame
    IEnumerator Shoot()
    {
        gameObject.GetComponent<AudioSource>().Play();
        shoot = false;
        GameObject new_ball = Instantiate(ball, transform.position + Vector3.up * 10, Quaternion.identity);
        new_ball.GetComponent<ball_collision>().enemy_ball = true;
        Vector3 vector_between_ship = ship.transform.position - transform.position;
        new_ball.GetComponent<Rigidbody>().AddForce(vector_between_ship * 0.8f, ForceMode.Impulse);
        yield return new WaitForSeconds(shoot_interval);
        shoot = true;
    }
    void Update()
    {
        if ((Mathf.Abs(transform.position.x - ship.transform.position.x) < 40) && (Mathf.Abs(transform.position.z - ship.transform.position.z) < 40))
        {
            if (shoot == true)
            {
                StartCoroutine(Shoot());
            }
        }
    }
}