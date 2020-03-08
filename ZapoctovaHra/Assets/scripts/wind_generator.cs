using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wind_generator : MonoBehaviour
{
    public static Vector3 position;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        position = Vector3.back;
    }
}
