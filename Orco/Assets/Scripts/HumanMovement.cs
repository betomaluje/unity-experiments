using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanMovement : MonoBehaviour
{
    [SerializeField] private float accelerationTime = 2f;
    [SerializeField] private float maxSpeed = 5f;
    private Vector2 movement;
    private float timeLeft;

    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        timeLeft -= Time.deltaTime;
        if (timeLeft <= 0)
        {
            movement = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
            timeLeft += accelerationTime;
        }
    }

    void FixedUpdate()
    {
        rb.AddForce(movement * maxSpeed);
    }
}
