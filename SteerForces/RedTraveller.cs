using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedTraveller : MonoBehaviour {
	//SET MASS = 1, SO FORCE = ACCELERATION

    public float maxForce;
    public float maxSpeed;
    public float seeAheadDistance;
    public float maxForceAvoid;
    public Vector3 destination;
    public float timeStop;

    private Vector3 velocity;
	private Rigidbody rb;
 	private bool shouldTurn;
 	private float timer;
 	private float size;

   
	// Use this for initialization
	void Start () {
		//set the destination
		shouldTurn = false;
		timer = 0.0f;
        int target = (int) Random.Range(0f, 1.99f);
		if(target == 0) destination = new Vector3(-21f, 1.8f, 5.5f);
		else destination = new Vector3(-21f, 1.8f, -5.5f);
        size = GetComponent<Renderer>().bounds.size.magnitude;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if(timer >= timeStop){
            ChangeDestination();
            timer = 0.0f;
        }
        else timer += Time.deltaTime;
        
        if (!shouldTurn) Move();
        else Turn();
    }

    //change the destination after a few seconds
    void ChangeDestination(){
        if(destination.z == 5.5f) destination = new Vector3(-21f, 1.8f, -5.5f);
        else destination = new Vector3(-21f, 1.8f, 5.5f);
    }

    //function that makes the player move according to the correct speeds and forces
    void Move() {
        Vector3 steer = Seek(destination);
        steer = Vector3.ClampMagnitude(steer,  maxForce);
        velocity = Vector3.ClampMagnitude(velocity + steer, maxSpeed);
        transform.forward = velocity.normalized;
        //make sure they avoid collision while moving
        steer = AvoidCollision();
        steer = Vector3.ClampMagnitude(steer, maxForce);
        velocity = Vector3.ClampMagnitude(velocity + steer, maxSpeed);
       //use rigid body to move player
        rb.MovePosition(transform.position + (velocity * Time.deltaTime));
        transform.forward = velocity.normalized;
    }

    //respawn function
    public void Respawn(){
        transform.position = new Vector3(20f, 1.8f, 0);
        timer = 0.0f;
    }

    //turn function
    void Turn(){
    	transform.Rotate(new Vector3(0, 180, 0));
        velocity = Vector3.ClampMagnitude(velocity, maxSpeed) * -1;
        rb.MovePosition(velocity * Time.deltaTime + transform.position);
        transform.forward = velocity.normalized;
        shouldTurn = false;
    }

    //the seek force
    Vector3 Seek(Vector3 dest){
        Vector3 desiredVel = dest - transform.position;
        desiredVel = maxSpeed * desiredVel.normalized;
        return (desiredVel - velocity);
    }

    Vector3 AvoidCollision(){
        Vector3 ahead = transform.position + velocity.normalized * seeAheadDistance;
        //use raycasthit to check ahead
        RaycastHit ray;
        Vector3 forceAvoid = new Vector3(0f, 0f, 0f);

        //sphere cast checks if there's an object right in front of us
        if (Physics.SphereCast(transform.position, size*0.5f, velocity, out ray, seeAheadDistance)){
            GameObject ob = ray.transform.gameObject;
            var obCenter = new Vector3(ob.transform.GetComponent<Renderer>().bounds.center.x, 1.8f, ob.transform.GetComponent<Renderer>().bounds.center.z);
          	forceAvoid = ahead - obCenter;
          	forceAvoid = forceAvoid.normalized;
           	forceAvoid *= maxForceAvoid;
        }
        else if (Physics.Raycast(transform.position, velocity, out ray, size)){
            if (!ray.transform.gameObject.CompareTag("Obstacle")){
            return forceAvoid;
         }
            shouldTurn = true;
        }
        return forceAvoid;
    }


}