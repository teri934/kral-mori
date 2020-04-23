using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wind_generator : MonoBehaviour
{
    public static Vector3 position;
    System.Random rd;
    bool start;
    public GameObject arrow;
    float angle;
	int wind_interval = 10;
    Quaternion to_rotation;
    // Start is called before the first frame update
    void Start()
    {
        rd = new System.Random();
        start = true;
    }

    IEnumerator GenerateWind()
    {
        start = false;
        position = new Vector3(rd.Next(-5, 5), 0, rd.Next(-5, 5));
        angle = Vector3.SignedAngle(Vector3.forward, position, Vector3.up);
        yield return new WaitForSeconds(wind_interval);
        start = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (start == true)
        {
            StartCoroutine(GenerateWind());
            to_rotation = Quaternion.Euler(0f, 0f, -angle);
        }
        arrow.transform.rotation = Quaternion.Slerp(arrow.transform.rotation, to_rotation, Time.deltaTime);
    }
}
