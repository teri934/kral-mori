using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camera_movement : MonoBehaviour
{
    public GameObject ship;
	const int max_dist_square = 13000;
	const float def_wave_amp = 0.0001f;
	const float def_wave_freq = 4;
    Vector3 last;
	Vector3 ship_position;
	float dy;
	float wave_amp = def_wave_amp;
	float wave_freq = def_wave_freq;
	

    void Start()
    {
        last = ship.transform.position;
    }

    void Update()
    {
		ship_position = ship.transform.position;
		//ship tracking
        transform.Translate(ship_position - last, Space.World);
		transform.Translate(dy*(ship_position-transform.position));
        last = ship_position;
		
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
		dy = wave_amp*Mathf.Sin(wave_freq*Time.time);
    }
	
	IEnumerator shakeTimer(float amplitude)
    {
		wave_amp = amplitude;
		wave_freq = 40;
        yield return new WaitForSeconds(0.5f);
		wave_amp = def_wave_amp;
		wave_freq = def_wave_freq;
    }
	
	public void Shake(float amplitude)
	{
		StartCoroutine(shakeTimer(amplitude));
	}
}
