using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shootbar : MonoBehaviour
{
    // Start is called before the first frame update
    private Transform bar;
    void Start()
    {
        bar = transform.Find("barsprite_shoot");
    }

    // Update is called once per frame

    public void SetSize(float size)
    {
        bar.localScale = new Vector3(size, 1f);
    }

    void Update()
    {
        
    }
}
