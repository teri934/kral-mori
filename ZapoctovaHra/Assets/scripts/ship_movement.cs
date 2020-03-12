using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ship_movement : MonoBehaviour
{
    private Rigidbody rb;
    float angle;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame

    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            //transform.Rotate(0.0f, -1.0f, 0.0f, Space.Self);
            rb.AddTorque(0f, -5f, 0f);
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            rb.AddTorque(0f, 5f, 0f);
        }

        angle = Vector3.Angle(wind_generator.position, transform.forward);
        Debug.Log(angle);
		
        rb.AddForce(3f * transform.forward + transform.forward * 3f * Mathf.Cos(angle * (Mathf.PI / 180)), ForceMode.Force);
    }
    void Update()
    {
        
    }
}
