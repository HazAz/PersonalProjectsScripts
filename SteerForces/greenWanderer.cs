using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class greenWanderer : MonoBehaviour {
	//SET MASS = 1, SO FORCE = ACCELERATION
    public float maxForce;
    public float maxSpeed;
    public float seeAheadDistance;
    public float maxForceAvoid;
    public float radius;
    public float wanderDistance;
    public float vision;
    public float maxBlockDistance;

    private float angle;
    private Vector3 velocity;
    private float size;
    private Rigidbody rb;
    private GameObject traveller;
    private bool isBlocking;
    private bool shouldTurn;
    

    // Use this for initialization
    void Start()
    {
        angle = 0f;
        isBlocking = false;
        shouldTurn = false;
        size = GetComponent<Renderer>().bounds.size.magnitude;
        rb = GetComponent<Rigidbody>();
        traveller = null;
    }

    // Update is called once per frame
    void Update()
    {
    	//if not blocking, check for closest traveller, if 
        if (!isBlocking) CheckForTraveller();
        if (shouldTurn) Turn();
        else Move();
    }

    void Move(){
    	//if wanderer is not blocking, then wander. If blocking, find corresponding steer motion. Avoid collision and clamp Steer to the max force.
    	//Set the velocity, but make sure its clamped
        Vector3 steer;
        if (!isBlocking) steer = Wander();
        else steer = BlockTraveller();
        steer += AvoidCollision();
        steer = Vector3.ClampMagnitude(steer, maxForce);
        velocity = Vector3.ClampMagnitude(velocity + steer, maxSpeed);
        rb.MovePosition(transform.position + (velocity * Time.deltaTime));
        transform.forward = velocity.normalized;
    }

    //finds closest traveller by finding the close colliders and checking distances
    void CheckForTraveller() {
        Collider[] col = Physics.OverlapSphere(transform.position, vision);
        GameObject closestTraveller = null;
        float distance = Mathf.Infinity;
        //traverses collider and finds the closest one, then tries to block it if its close enough
        foreach (Collider c in col){
            if (c.gameObject.CompareTag("Traveller")) {
                Vector3 disp = c.gameObject.transform.position - transform.position;
                float currentDistance = disp.sqrMagnitude;
                if (currentDistance < distance){
                	closestTraveller = c.gameObject;
                  	isBlocking = true;
                }
            }
        }
        traveller = closestTraveller;
    }

    //changes the angle
    Vector3 SetAngle(Vector3 vec, float angle){
        return new Vector3(Mathf.Cos(angle) * vec.magnitude, 0, Mathf.Sin(angle) * vec.magnitude);
    }

    //function that blocks travellers
    Vector3 BlockTraveller() {
        var dist = (traveller.transform.position - transform.position).magnitude;
        //if in range, block
        if (dist <= maxBlockDistance){
			Vector3 travellerDest = traveller.GetComponent<RedTraveller>().destination;
        	travellerDest = new Vector3(travellerDest.x, 1.8f, travellerDest.z);
        	Vector3 mid = (traveller.transform.position + travellerDest) / 2;
       	 	return Seek(mid); 	
        }
        //if out of range, wander
        else{
        	traveller = null;
            isBlocking = false;
            return Wander();
       	}
    }

     //turn function
    void Turn(){
        velocity = -1 * velocity;
        transform.Rotate(new Vector3(0, 180, 0));
        velocity = Vector3.ClampMagnitude(velocity, maxSpeed);
        rb.MovePosition(velocity * Time.deltaTime + transform.position);
        transform.forward = velocity.normalized;
        shouldTurn = false;
    }

//seek method as before
    Vector3 Seek(Vector3 dest){
        Vector3 dest2 = new Vector3(dest.x, 1.8f, dest.z);
        var desiredVelocity = dest2 - transform.position;
        desiredVelocity = desiredVelocity.normalized * maxSpeed;
        return desiredVelocity - velocity;
    }
    //wander method
    Vector3 Wander() {
    	Vector3 displacement = new Vector3(0,0,-1 * radius);
        angle += Random.Range(-45f, 45f);
        displacement = SetAngle(displacement, angle);
        return displacement + velocity.normalized *wanderDistance;
    }


    //avoids collision like before
    Vector3 AvoidCollision(){
        Vector3 ahead = transform.position + velocity.normalized * seeAheadDistance;
        RaycastHit ray;
        Vector3 forceAvoid = new Vector3(0f, 0f, 0f);

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
