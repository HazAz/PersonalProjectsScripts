using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AISpaceRaceScript : MonoBehaviour
{
    private GameObject upBox;
    private GameObject leftBox;
    private GameObject rightBox;

    private GameObject SpaceRaceGame;

    public float speed;
    private float speedmultiplier;
    // Start is called before the first frame update
    void Start()
    {
        upBox = transform.Find("Up").gameObject;
        leftBox = transform.Find("Left").gameObject;
        rightBox = transform.Find("Right").gameObject;

        SpaceRaceGame = GameObject.Find("SpaceRaceGame");
    	speed = 1.5f;
        speedmultiplier = (float) GlobalScript.GetGameSpeed() / 2.0f;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    void Move()
    {
        bool upClear = upBox.GetComponent<AIScanner>().IsClear();
        bool leftClear = leftBox.GetComponent<AIScanner>().IsClear();
        bool rightClear = rightBox.GetComponent<AIScanner>().IsClear();
        if (upClear)
        {
            transform.Translate(Vector2.up * Time.deltaTime * speed * speedmultiplier);
        }
        else if (leftClear && rightClear)
        {
            transform.Translate(new Vector2(0, 0));
        }
        else
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
