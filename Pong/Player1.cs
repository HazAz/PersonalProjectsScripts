using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player1 : MonoBehaviour
{
	public float speed;
    private float speedmultiplier;
    // Start is called before the first frame update
    void Start()
    {
    	speed = 5;
        speedmultiplier = (float) GlobalScript.GetGameSpeed() / 2.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
        	transform.Translate(Vector2.up * Time.deltaTime * speed * speedmultiplier);
        }

        if (Input.GetKey(KeyCode.S))
        {
        	transform.Translate(Vector2.down * Time.deltaTime * speed * speedmultiplier);
        }
    }
}
