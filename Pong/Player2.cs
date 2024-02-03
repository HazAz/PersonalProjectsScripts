using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2 : MonoBehaviour
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
        if (Input.GetKey(KeyCode.UpArrow))
        {
        	transform.Translate(Vector2.up * Time.deltaTime * speed * speedmultiplier);
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
        	transform.Translate(Vector2.down * Time.deltaTime * speed * speedmultiplier);
        }
    }
}
