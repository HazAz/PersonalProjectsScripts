using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2SpaceRace : MonoBehaviour
{
    public float speed;
    private float speedmultiplier;

    private GameObject SpaceRaceGame;
    // Start is called before the first frame update
    void Start()
    {
        SpaceRaceGame = GameObject.Find("SpaceRaceGame");
        ResetPosition();
    	speed = 1.5f;
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

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Ball")
        {
            ResetPosition();
        }
        if (col.gameObject.tag == "Finish")
        {
            ResetPosition();
            SpaceRaceGame.GetComponent<SpaceRaceScript>().Player2Scored();
        }
    }

    void ResetPosition()
    {
        transform.position = new Vector3(3.75f, -4.5f, -3.0f);
    }
}
