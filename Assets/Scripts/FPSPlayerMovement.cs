using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSPlayerMovement : MonoBehaviour
{

    public CharacterController controller;

    public float speed = 12f;
    public float gravity = -9.81f;

    public Transform groundcheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    Vector3 velocity;
    public bool isGrounded;


    //For reference Time.deltaTime makes sure that physics are not affected by the ammount of frames a player is getting.//

    void Update()
    {
        //This part of the code uses an ifcheck to see if the player is on the ground, if that is the case then the speed they are falling at will revert to normal.//
      
    }

    void FixedUpdate()
    {
          isGrounded = Physics.CheckSphere(groundcheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;

        }

        if (Input.GetKeyDown("space") && isGrounded)
        {
            velocity.y = 15 * Time.deltaTime;
        }

        //float x controls side to side movement//
        float x = Input.GetAxis("Horizontal");

        //float z controls forward backward movement//
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x * speed + transform.forward * z * speed;

        controller.Move(move);

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);

    }
}
