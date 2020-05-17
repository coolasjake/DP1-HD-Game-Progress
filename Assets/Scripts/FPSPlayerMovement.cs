using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSPlayerMovement : MonoBehaviour
{

    public CharacterController controller;

    public float speed = 12f;
    public float gravity = -19.81f;
    public float jumpHeight = 12f;

    public Transform groundcheck;

    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    Vector3 velocity;
    public bool isGrounded;


    //For reference Time.deltaTime makes sure that physics are not affected by the ammount of frames a player is getting.//

    void Update()
    {
        //This part of the code uses an ifcheck to see if the player is on the ground, if that is the case then the speed they are falling at will revert to normal.//
        isGrounded = Physics.CheckSphere(groundcheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
            {
            velocity.y = -2f;
            } 
        
        //float x controls side to side movement//
        float x = Input.GetAxis("Horizontal");
        //float z controls forward backward movement//
        float z = Input.GetAxis("Vertical");

        ///controls the movement update before jump & gravity are calculatred 
        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * speed * Time.deltaTime);

        if (Input.GetKeyDown("space") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -1f * gravity);
            Debug.Log(isGrounded);
        }

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);

    }
}
