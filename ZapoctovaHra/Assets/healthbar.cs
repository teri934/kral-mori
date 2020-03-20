using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class healthbar : MonoBehaviour
{
    // Start is called before the first frame update
    private Transform bar;
    void Start()
    {
        bar = transform.Find("barsprite");
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
