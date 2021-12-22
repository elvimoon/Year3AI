using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//reference: https://gamedevelopment.tutsplus.com/series/understanding-steering-behaviors--gamedev-12732

public class SteeringBehaviors : MonoBehaviour
{

    public Transform target;
    private Vector3 velocity;

    public float mass = 15;
    public float maxVel = 3;
    public float maxForce = 15;

    public float health = 100;

    public float circleDistance = 5; // used to set the centre of the circle of possible movement directions
    public float offset = 1; //used to set how extreme direction changes can be

    public float detectionCircle = 2;

    void Start()
    {
        //set initial velocity to zero
        velocity = Vector3.zero;
    }

    void Update()
    {
        //calculate the distance between the AI and target via desired velocity
        Vector3 desiredVel = target.transform.position - transform.position;
        float distance = desiredVel.magnitude;

        //check distance to detected whether character is inside detection circle
        //low health
        if (health < 25)
        {
            FleeBehavior();
        }
        //inside the detection circle
        else if (distance < detectionCircle && health >= 25)
        {
            ArriveBehavior();
            //SeekBehavior();
        }
        //outside detection circle
        else if (health >= 25)
        {
            WanderBehavior();
        }
    }

    private void SeekBehavior()
    {
        //desired velocity is force that guides AI to target in shortest path possible, it will update every time the target moves
        Vector3 desiredVel = target.transform.position - transform.position;
        desiredVel = desiredVel.normalized * maxVel;

        //steering is result of desired velocity subtracted by current velocity, addition of force will create seek path and gradual change to update velocity
        //allowing for smoother and non-abrupt movement towards target position as it updates
        Vector3 steering = desiredVel - velocity;
        steering = Vector3.ClampMagnitude(steering, maxForce);
        steering = steering / mass;

        velocity = Vector3.ClampMagnitude(velocity + steering, maxVel);
        transform.position = transform.position + velocity * Time.deltaTime;
    }

    private void ArriveBehavior()
    {
        //desired velocity is force that guides AI to target in shortest path possible, it will update every time the target moves
        Vector3 desiredVel = target.transform.position - transform.position;
        float distance = desiredVel.magnitude;
        float slowingRadius = 10;

        //if within the radius of circle, slow down speed
        if (distance < slowingRadius)
        {
            desiredVel = desiredVel.normalized * maxVel * (distance / slowingRadius);
        }
        // else no change in speed
        else
        {
            desiredVel = desiredVel.normalized * maxVel;
        }

        //steering is result of desired velocity subtracted by current velocity, addition of force will create seek path and gradual change to update velocity
        //allowing for smoother and non-abrupt movement towards target position as it updates
        Vector3 steering = desiredVel - velocity;
        steering = Vector3.ClampMagnitude(steering, maxForce);
        steering = steering / mass;

        velocity = Vector3.ClampMagnitude(velocity + steering, maxVel);
        transform.position = transform.position + velocity * Time.deltaTime;
    }

    private void FleeBehavior()
    {
        //exact same calculation as SeekBehavior, but we are subtracting AI position from target position instead
        //desired velocity is now the easiest route the AI can use to escape from target
        Vector3 desiredVel = transform.position - target.transform.position;
        desiredVel = desiredVel.normalized * maxVel;

        //steering will result in AI abandoning current route, pushing towards direction of desired velocity vector
        Vector3 steering = desiredVel - velocity;
        steering = Vector3.ClampMagnitude(steering, maxForce);
        steering = steering / mass;

        velocity = Vector3.ClampMagnitude(velocity + steering, maxVel);
        transform.position = transform.position + velocity * Time.deltaTime;
    }

    private void WanderBehavior()
    {
        // Generate a circle to position the potential veloicties
        Vector3 circleCentre = velocity;
        circleCentre = circleCentre.normalized * circleDistance;
        // displacement will give a random point on the edge of the circle
        Vector2 direction = Random.insideUnitCircle;
        Vector3 displacement = new Vector3(direction.x, 0.0f, direction.y);
        displacement = displacement.normalized * offset;

        velocity = circleCentre + displacement;
        velocity = velocity.normalized * maxVel;
        transform.position = transform.position + velocity * Time.deltaTime;
    }

    //When AI reaches target, stop all movement
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GetComponent<SteeringBehaviors>().enabled = false;
            this.gameObject.GetComponent<Renderer>().material.color = Color.green;
        }
    }

    //When target moves away from AI, resume script and movement
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GetComponent<SteeringBehaviors>().enabled = true;
            this.gameObject.GetComponent<Renderer>().material.color = Color.red;
        }
    }
}
