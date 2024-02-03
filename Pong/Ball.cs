using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
	private Rigidbody2D rigidbody;
	private float currentSpeed;
	private Transform startPosition;
	private GameObject pongGame;
    private float speedmultiplier;

    private float slowSpeed;
    private float normalSpeed;

    private float stopWatch;

    private float countDown;

    private bool resettingPosition;
    // Start is called before the first frame update
    void Start()
    {
        speedmultiplier = (float) GlobalScript.GetGameSpeed() / 2.0f;
    	currentSpeed = 5;
        slowSpeed = 2;
        normalSpeed = 5;

        stopWatch = 0.0f;
        countDown = 3.0f;

    	startPosition = GameObject.Find("CenterPosition").transform;
        rigidbody = GetComponent<Rigidbody2D>();
        ResetPosition();
        pongGame = GameObject.Find("pongGame");
    }

    // Update is called once per frame
    void Update()
    {
        CheckOutOfRange();
        CheckIfStuck();
        rigidbody.velocity = currentSpeed * (rigidbody.velocity.normalized) * speedmultiplier;
    }

    private void ResetPosition(int playerScored = 0)
    {
    	transform.position = startPosition.position;
        float xDir, yDir;
        if (playerScored == 0)
            xDir = Random.Range(-1.0f, 1.0f);
        else if (playerScored == 1)
            xDir = Random.Range(-1.0f, -0.2f);
        else
            xDir = Random.Range(0.2f, 1.0f);
        yDir = Random.Range(-0.2f, 0.2f);
        while (Mathf.Abs(xDir) < 2*Mathf.Abs(yDir))
        {
            xDir = Random.Range(-1.0f, 1.0f);
        }
    	var direction = new Vector2(xDir, yDir);
        currentSpeed = slowSpeed;
    	rigidbody.AddForce(direction.normalized * currentSpeed, ForceMode2D.Impulse);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
    	if (col.gameObject.name == "RightLine")
    	{
    		pongGame.GetComponent<pongScript>().Player1Scored();
            ResetPosition(1);
    	}
    	else
    	{
    		pongGame.GetComponent<pongScript>().Player2Scored();
            ResetPosition(2);
    	}
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Player") currentSpeed = normalSpeed;
    }

    void CheckOutOfRange()
    {
        if (transform.localPosition.x > 0.5 || transform.localPosition.x < -0.5
            || transform.localPosition.y > 0.5 || transform.localPosition.y < -0.5
            || (rigidbody.velocity.x < 0.2f && rigidbody.velocity.x > -0.2f))
        {
            ResetPosition();
        }
    }

    void CheckIfStuck()
    {
        if ((rigidbody.transform.localPosition.y < -0.465f && rigidbody.transform.localPosition.y > -0.5f)
        || (rigidbody.transform.localPosition.y < 0.5f && rigidbody.transform.localPosition.y > 0.465f))
        {
            stopWatch += Time.deltaTime;
            if (stopWatch >= countDown)
            {
                ResetPosition();
            }
        } 
        else 
        {
            stopWatch = 0.0f;
        }
    }
}
