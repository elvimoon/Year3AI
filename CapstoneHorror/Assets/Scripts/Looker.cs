using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Looker : MonoBehaviour
{

    public GameObject guard;
    private float reset = 5;
    private bool movingDown;

    // Start is called before the first frame update
    //void Start()
    //{
        
    //}

    // Update is called once per frame
    void Update()
    {
        reset -= Time.deltaTime;
        if (reset < 0)
        {
            guard.GetComponent<Guard>().enabled = false;
            GetComponent<SphereCollider>().enabled = true;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            guard.GetComponent<Guard>().enabled = true;
            reset = 5;
            GetComponent<SphereCollider>().enabled = false;
        }
    }
}
