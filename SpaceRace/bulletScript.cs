using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletScript : MonoBehaviour
{
    private float speed;
    private float speedmultiplier;
    private Rigidbody2D rigidbody;

    private GameObject SpaceRaceGame;

    private Vector2 direction;
    // Start is called before the first frame update
    void Start()
    {
        SpaceRaceGame = GameObject.Find("SpaceRaceGame");
        rigidbody = GetComponent<Rigidbody2D>();
    	speed = 1;
        speedmultiplier = (float) GlobalScript.GetGameSpeed() / 2.0f;
        direction = transform.position.x > 0 ? Vector2.left : Vector2.right;
        rigidbody.AddForce(direction * speed, ForceMode2D.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        rigidbody.velocity = speed * direction * speedmultiplier;
        if (transform.position.x < -8 || transform.position.x > 8)
        {
            Destroy(this.gameObject);
            SpaceRaceGame.GetComponent<SpaceRaceScript>().numBulletsCreated--;
        }
    }
}
