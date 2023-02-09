using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private Vector2 move;
    public float speed;
    private float maxForce = 1;
    private Rigidbody rb;

    private Vector3 velocity;

    public float gravity = -9.81f;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        
    }


    private void Update()
    {

        //velocity.y += gravity * Time.deltaTime;
        //GetComponent<CharacterController>().Move(velocity * Time.deltaTime);
       // Debug.Log(velocity.y);
    }

    void FixedUpdate()
    {
        Move();
    }
    
    public void OnMove(InputAction.CallbackContext context)
    {
        move = context.ReadValue<Vector2>();
    }
    
    void Move()
    {
        //find target velocity
        Vector3 currentVelocity = rb.velocity;
        Vector3 targetVelocity = new Vector3(move.x, 0, move.y);
        targetVelocity *= speed;
        
        //Allign direction
        targetVelocity = transform.TransformDirection(targetVelocity);
        
        //Calculate forces
        Vector3 velocityChange = targetVelocity - currentVelocity;
        velocityChange = new Vector3(velocityChange.x, 0, velocityChange.z);
        //limit force
        //Vector3.ClampMagnitude(velocityChange, maxForce);
        rb.AddForce(velocityChange, ForceMode.VelocityChange );
    }
}
