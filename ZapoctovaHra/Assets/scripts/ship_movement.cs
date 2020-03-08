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
        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.Rotate(0.0f, -2.0f, 0.0f, Space.Self);
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Rotate(0.0f, 2.0f, 0.0f, Space.Self);
        }

        angle = Vector3.Angle(wind_generator.position, transform.forward);
        Debug.Log(angle);

        rb.AddForce(3f * transform.forward + transform.forward * 3f * Mathf.Cos(angle * (Mathf.PI / 180)), ForceMode.Force);
        Debug.Log(Mathf.Cos(angle * (Mathf.PI / 180)));
        Debug.Log(3f * transform.forward + transform.forward * 3f * Mathf.Cos(angle * (Mathf.PI / 180)));
    }
    void Update()
    {
        
    }
}
