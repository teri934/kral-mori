using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ship_movement : MonoBehaviour
{
    private Rigidbody rb;
    float angle;
    private bool anchor;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anchor = false;
    }

    // Update is called once per frame

    private void FixedUpdate()
    {

        if (Input.GetKeyDown(KeyCode.A))
        {
            anchor = !anchor;
            Debug.Log(anchor);
        }


        angle = Vector3.Angle(wind_generator.position, transform.forward);
        Debug.Log(angle);


        if (anchor == false)
        {
            rb.AddForce(3f * transform.forward + transform.forward * 3f * Mathf.Cos(angle * (Mathf.PI / 180)), ForceMode.Force);

            if (Input.GetKey(KeyCode.RightArrow))
            {
                //transform.Rotate(0.0f, -1.0f, 0.0f, Space.Self);
                rb.AddTorque(0f, -0.5f, 0f);
            }

            if (Input.GetKey(KeyCode.LeftArrow))
            {
                rb.AddTorque(0f, 0.5f, 0f);
            }
        }


		
        rb.AddForce(3f * transform.forward + transform.forward * 3f * Mathf.Cos(angle * (Mathf.PI / 180)), ForceMode.Force);

    }
    void Update()
    {
        
    }
}
