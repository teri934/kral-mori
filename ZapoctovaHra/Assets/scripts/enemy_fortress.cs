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
        ship = GameObject.Find("ship_01");
    }

    // Update is called once per frame
    IEnumerator Shoot()
    {
        Debug.Log("shoot");
        shoot = false;
        GameObject new_ball = Instantiate(ball, transform.position, Quaternion.identity);
        Vector3 vector_between_ship = ship.transform.position - transform.position;
        new_ball.GetComponent<Rigidbody>().AddForce((Vector3.up + vector_between_ship) * 100, ForceMode.Impulse);
        yield return new WaitForSeconds(shoot_interval);
        shoot = true;
    }
    void Update()
    {
        if ((Mathf.Abs(transform.position.x - ship.transform.position.x) < 30) && (Mathf.Abs(transform.position.z - ship.transform.position.z) < 30))
        {
            if (shoot == true)
            {
                StartCoroutine(Shoot());
            }
        }
    }
}
//TODO make balls actually shoot