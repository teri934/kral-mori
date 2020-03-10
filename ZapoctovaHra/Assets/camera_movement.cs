using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camera_movement : MonoBehaviour
{
    public GameObject ship;
    Vector3 last;
    // Start is called before the first frame update
    void Start()
    {
        last = ship.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(ship.transform.position - last, Space.World);
        last = ship.transform.position;
        Debug.Log(last);
    }
}
