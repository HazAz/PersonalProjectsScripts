using UnityEngine;
using System;

public class Movement : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D rb;
    private Action action;
    private float speed;
    private Transform targetTransform;
    private bool isMoving;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        isMoving = false;
    }

    public void MoveTo(Transform target, float speed, Action nextAction = null)
    {
        this.targetTransform = target;
        this.speed = speed;
        if (nextAction != null) this.action = nextAction;
        isMoving = true;
    }

    // Update is called once per frame
    private void Update()
    {
        if (isMoving)
        {
            Vector3 moveDir = (targetTransform.position - transform.position).normalized;
            rb.velocity = moveDir * speed;

            if (moveDir.x > 0.5f) animator.SetInteger("Direction", 2);
            else if (moveDir.x < -0.5f) animator.SetInteger("Direction", 3);
            else if (moveDir.y > 0.5f) animator.SetInteger("Direction", 1);
            else if (moveDir.y < 0.5f) animator.SetInteger("Direction", 0);
            animator.SetBool("IsMoving", moveDir.magnitude > 0);

            if (Vector3.Distance(targetTransform.position, this.transform.position) < 0.5f)
            {
                isMoving = false;
                rb.velocity = Vector3.zero;
                if (action != null) action();
            }
        }
    }
}
