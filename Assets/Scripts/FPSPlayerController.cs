using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSPlayerController : MonoBehaviour
{
    public CharacterController controller;
    public Camera mainCamera;

    public float speed = 12f;
    public float gravity = -19.81f;
    public float jumpHeight = 12f;

    public Transform groundcheck;

    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    Vector3 velocity;
    public bool isGrounded;

    //Mouse values
    public float mouseSensitivity = 100f;
    public Transform playerBody;
    float xRotation = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    //For reference Time.deltaTime makes sure that physics are not affected by the ammount of frames a player is getting.//

    void Update()
    {
        MouseMovement();
        PlayerMovement();
    }

    public void MouseMovement()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;


        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -85f, 85f);


        mainCamera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    private void PlayerMovement()
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
            //Debug.Log(isGrounded);
        }

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
    }
}
