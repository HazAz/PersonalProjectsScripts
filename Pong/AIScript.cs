using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIScript : MonoBehaviour
{
    public float speed;
    private float speedmultiplier;

    private GameObject pongBall;
    // Start is called before the first frame update
    void Start()
    {
    	speed = 5;
        speedmultiplier = (float) GlobalScript.GetGameSpeed() / 3.5f;
        pongBall = GameObject.Find("PongBall");
    }

    // Update is called once per frame
    void Update()
    {
        if (pongBall.transform.localPosition.x > 0.15f)
        {
            float yDir = (Random.Range(0.0f, 1.0f) > 0.5f) ? pongBall.transform.position.y + 0.2f : pongBall.transform.position.y - 0.2f;
            var targetPositon = new Vector3(transform.position.x, pongBall.transform.position.y, transform.position.z);
            transform.position = Vector3.MoveTowards(transform.position, targetPositon, speed * speedmultiplier * Time.deltaTime);
        }
    }
}
