using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wind_generator : MonoBehaviour
{
    public static Vector3 position;
    System.Random rd;
    bool start;
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
        yield return new WaitForSeconds(10);
        start = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (start == true)
            StartCoroutine(GenerateWind());
    }
}
