using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 10;

    //public float jumpForce = 7;
    private Rigidbody2D body;
    private CircleCollider2D col;

    public float jumpForce = 0.001f;
    public float moveSpeed = 5;
    public Transform groundCheck;
    private bool isGrounded;
    public bool canMove;
    public GameObject webGO;
    private AudioSource playerSource;


    private void Start()
    {
        body = GetComponent<Rigidbody2D>();
        col = GetComponent<CircleCollider2D>();
        playerSource = GetComponent<AudioSource>();
        canMove = true;
    }

    private void Update()
    {
        if (canMove == false)
        {
            isGrounded = true;
            if (VirtualInputManager.Instance.Jump)
            {
                if(webGO)
                {
                    Debug.Log("Jump." + webGO);
                    webGO.GetComponent<BoxCollider2D>().isTrigger = false;
                    webGO.GetComponent<StickyTrigger>().activated = false;
                    StartCoroutine("SetTriggerTrue");
                }
                body.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                this.GetComponent<Rigidbody2D>().gravityScale = 1;
                canMove = true;
                isGrounded = false;
            } 
            else
            {
                return;
            }
        }

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.05f) != null;

        if (VirtualInputManager.Instance.MoveLeft)
        {
            body.transform.Translate(Vector2.left * speed * Time.deltaTime);
        }

        if (VirtualInputManager.Instance.MoveRight)
        {
            body.transform.Translate(Vector2.right * speed * Time.deltaTime);
        }

        if (isGrounded)
        {
            if (VirtualInputManager.Instance.Jump && isGrounded)
            {
                playerSource.Play();
                body.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                isGrounded = false;
            }
        }
    }

    void OnCollisionStay()
    {
        isGrounded = true;
    }

    IEnumerator SetTriggerTrue()
    {
        Debug.Log("in SetTriggerTrue");
        yield return new WaitForSeconds(0.5f);
        webGO.GetComponent<BoxCollider2D>().isTrigger = true;
        webGO = null;
    }
}
