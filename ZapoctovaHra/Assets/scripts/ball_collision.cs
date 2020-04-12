using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ball_collision : MonoBehaviour

{
    private Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    IEnumerator DestroyBall()
    {
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "island")
        {
            gameObject.GetComponent<ParticleSystem>().Play();
            Destroy(rb);
            StartCoroutine(DestroyBall());
        }

    }
    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < 1.3f)
        {
            Destroy(gameObject);
        }
    }
}
