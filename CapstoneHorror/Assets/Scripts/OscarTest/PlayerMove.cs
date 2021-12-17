using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerMove : NetworkBehaviour
{
    public CharacterController controller;
    public float speed = 6f;

    // Update is called once per frame
    void FixedUpdate()
    {
        if (this.isLocalPlayer)
        {
            float horizontal = Input.GetAxisRaw("Horizontal");
            Vector3 direction = new Vector3(horizontal, 0f, 0f).normalized;

            if (direction.magnitude >= 0.1f)
            {
                controller.Move(direction * speed * Time.deltaTime);
            }
        }
    }
}
