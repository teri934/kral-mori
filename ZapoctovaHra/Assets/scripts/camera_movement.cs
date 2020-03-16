using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camera_movement : MonoBehaviour
{
    public GameObject ship;
	const int max_dist_square = 13000;
    Vector3 last;
    // Start is called before the first frame update
    void Start()
    {
        last = ship.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
		//ship tracking
        transform.Translate(ship.transform.position - last, Space.World);
        last = ship.transform.position;
		
        if ((ship.transform.position - transform.position).sqrMagnitude < max_dist_square)
        {
            //camera zoom
            if (Input.GetAxis("Mouse ScrollWheel") != 0f)
            {
                transform.Translate(Input.GetAxis("Mouse ScrollWheel") * (ship.transform.position - transform.position), Space.World);
            }
        }

        if ((ship.transform.position - transform.position).sqrMagnitude >= max_dist_square)
        {
            //camera zoom
            if (Input.GetAxis("Mouse ScrollWheel") > 0f)
            {
                transform.Translate(Input.GetAxis("Mouse ScrollWheel") * (ship.transform.position - transform.position), Space.World);
            }
        }
    }
}
