using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class counter : MonoBehaviour
{
    private ship_movement ship;
    public Text orange_counter;
    public Text coconut_counter;
    // Start is called before the first frame update
    void Start()
    {
        ship = FindObjectOfType<ship_movement>();
    }

    // Update is called once per frame
    void Update()
    {
        orange_counter.text = ("" + ship.counter_oranges);
        coconut_counter.text = ("" + ship.counter_coconuts);
    }
}
