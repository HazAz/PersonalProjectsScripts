using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YellowSocial : MonoBehaviour {
	//SET MASS = 1, SO FORCE = ACCELERATION

    public float seeAheadDistance;
    public float maxForceAvoid;
    public float radius;
    public float wanderDistance;
    public float vision;
	public float maxForce;
	public bool isCooling;
	public YellowCircle circle;
    
    public float maxSpeed;
    private Vector3 velocity;
    private float size;
    private Rigidbody rb;
    private bool shouldTurn;
    private float angle;


    // Use this for initialization
    void Start()
    {
    	isCooling = false;
        angle = 0f;
        shouldTurn = false;
        size = GetComponent<Renderer>().bounds.size.magnitude;
        rb = GetComponent<Rigidbody>();
    }
    // Update is called once per frame
    void Update()
    {
        if (!shouldTurn) Move();
        else Turn();
    }

    //move function
    void Move(){
        Vector3 steer = Wander();
        steer += AvoidCollision();
        steer = Vector3.ClampMagnitude(steer, maxForce);
        velocity = Vector3.ClampMagnitude(velocity + steer, maxSpeed);
        rb.MovePosition(transform.position + (velocity * Time.deltaTime));
        transform.forward = velocity.normalized;
    }

   //seek function used by move
    Vector3 Seek(Vector3 dest){
        Vector3 dest2 = new Vector3(dest.x, 1.8f, dest.z);
        var desiredVelocity = dest2 - transform.position;
        desiredVelocity = desiredVelocity.normalized * maxSpeed;
        return desiredVelocity - velocity;
    }

    //same wander as wanderer
    Vector3 Wander() {
    	Vector3 displacement = new Vector3(0,0,-1 * radius);
        angle += Random.Range(-45f, 45f);
        displacement = SetAngle(displacement, angle);
        return displacement + velocity.normalized *wanderDistance;
    }

    //change angles like wanderer
    Vector3 SetAngle(Vector3 vec, float angle){
        return new Vector3(Mathf.Cos(angle) * vec.magnitude, 0, Mathf.Sin(angle) * vec.magnitude);
    }

    //wanted to make an aray that finds the closest social group, then joins closest but no time.
    private void OnTriggerEnter(Collider c) {
		if (c.CompareTag("Social") && !isCooling) {
			YellowSocial person = c.gameObject.GetComponent<YellowSocial>();
			
			if (this.circle == null && person.circle != null) {
				// inviting myself to the party like a loser
				this.Enter(person.circle);
			} 
			else if (this.circle == null && person.circle == null && !person.isCooling) {
				StopCoroutine("Wander");
				Vector3 center = transform.position * 0.5f + person.transform.position * 0.5f;
				circle = new YellowCircle(center, this);
				person.Enter(circle);
				StartCoroutine("Socialize");
			}
		}
	}

    //enter a circle 
    public void Enter(YellowCircle circle) {
		this.circle = circle;
		circle.Enter(this);
		StopCoroutine("Wander");
		StartCoroutine("Socialize");
	}

	//avoid collision like before
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
    //turn function
     void Turn(){
        velocity = -1 * velocity;
        transform.Rotate(new Vector3(0, 180, 0));
        velocity = Vector3.ClampMagnitude(velocity, maxSpeed);
        rb.MovePosition(velocity * Time.deltaTime + transform.position);
        transform.forward = velocity.normalized;
        shouldTurn = false;
    }



}
