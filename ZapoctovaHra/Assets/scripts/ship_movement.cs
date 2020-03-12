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

        Debug.Log(angle);

        if (anchor == false)
        {
            rb.AddForce(17f * transform.forward + transform.forward * 17f * Mathf.Cos(angle * (Mathf.PI / 180)), ForceMode.Force);

            if (Input.GetKey(KeyCode.LeftArrow))
            {
                rb.AddTorque(0f, -7f, 0f);
            }

            if (Input.GetKey(KeyCode.RightArrow))
            {
                rb.AddTorque(0f, 7f, 0f);
            }
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "cube")
        {
            rb.AddForce(10f * - transform.forward, ForceMode.Impulse);
        }
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            anchor = !anchor;
        }

        angle = Vector3.Angle(wind_generator.position, transform.forward);
    }
}
