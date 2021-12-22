using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// referenced from https://gamedevelopment.tutsplus.com/tutorials/understanding-steering-behaviors-wander--gamedev-1624
public class SteerWander : MonoBehaviour
{
    [SerializeField] 
    private float circleDistance; // used to set the centre of the circle of possible movement directions

    [SerializeField] 
    private float offset; // used to set how extreme direction changes can be

    [SerializeField] 
    private float maxSpeed; // used to set how fast the object can go

    public Vector3 velocity;

    private void Start()
    {
        //set initial velocity to zero
        velocity = Vector3.zero;
    }

    void Update()
    {
        // Generate a circle to position the potential veloicties
        Vector3 circleCentre = velocity;
        circleCentre = circleCentre.normalized * circleDistance;
        // displacement will give a random point on the edge of the circle
        Vector2 direction = Random.insideUnitCircle;
        Vector3 displacement = new Vector3(direction.x, 0.0f, direction.y);
        displacement = displacement.normalized * offset;
        
        velocity = circleCentre + displacement;
        velocity = velocity.normalized * maxSpeed;
        transform.position = transform.position + velocity * Time.deltaTime;
    }
}
