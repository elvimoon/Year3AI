using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    private Rigidbody rb;
    Vector3 _EulerAngleVelocity;
    public Transform _camera;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        _EulerAngleVelocity = new Vector3(0, 25, 0);
    }

    // Update is called once per frame
    void Update()
    {
        Quaternion deltaRotationR = Quaternion.Euler(_EulerAngleVelocity * Time.fixedDeltaTime);
        Quaternion deltaRotationL = Quaternion.Euler(-_EulerAngleVelocity * Time.fixedDeltaTime);
        if (Input.GetKey(KeyCode.UpArrow))
            rb.AddForce(_camera.forward * 5);
        if (Input.GetKey(KeyCode.DownArrow))
            rb.AddForce(_camera.forward * -5);
        if (Input.GetKey(KeyCode.LeftArrow))
            rb.MoveRotation(rb.rotation * deltaRotationL);
        if (Input.GetKey(KeyCode.RightArrow))
            rb.MoveRotation(rb.rotation * deltaRotationR);
    }
}
