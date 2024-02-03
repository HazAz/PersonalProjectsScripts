using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickyTrigger : MonoBehaviour
{
    public bool activated = false;
    private Rigidbody2D playerRb;
    private Transform playerDestination;
    private GameObject playerGameObject;

    private void Start()
    {
        playerGameObject = GameObject.FindGameObjectWithTag("Player");
        playerRb = playerGameObject.GetComponent<Rigidbody2D>();
        playerDestination = this.transform.GetChild(0).transform;
 
    }
    // Update is called once per frame
    void Update()
    {
        if (activated)
        {
            playerGameObject.GetComponent<PlayerMovement>().canMove = false;
            playerRb.velocity = Vector2.zero;
            playerRb.gravityScale = 0;
            playerGameObject.transform.position = playerDestination.position;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        playerGameObject.GetComponent<PlayerMovement>().webGO = this.gameObject;
        activated = true;
    }
}
