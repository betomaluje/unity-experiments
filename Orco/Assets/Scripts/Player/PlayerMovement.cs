using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    private enum Direction
    {
        BOTH, HORIZONTAL, VERTICAL
    }

    [SerializeField] private Direction movementDirection;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float turnSpeed = 100f;

    private Rigidbody2D rb;
    private Animator anim;

    private Vector2 movement;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        if (movementDirection.Equals(Direction.HORIZONTAL))
        {
            movement.y = 0;
        } else if (movementDirection.Equals(Direction.VERTICAL))
        {
            movement.x = 0;
        }        

        anim.SetBool("isWalking", movement.x != 0 || movement.y != 0);
    }

    private void FixedUpdate()
    {
        // movement
        rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);

        // rotation
        Vector3 realDirection = Camera.main.transform.TransformDirection(movement);
        // this line checks whether the player is making inputs.
        if (realDirection.magnitude > 0.1f)
        {
            Quaternion newRotation = Quaternion.LookRotation(Vector3.forward, realDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, Time.fixedDeltaTime * 10);
        }
    }

}
