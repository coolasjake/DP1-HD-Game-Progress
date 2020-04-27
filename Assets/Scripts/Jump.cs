using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : MonoBehaviour
{
    public Transform playertransform;


    Vector3 jumppower = new Vector3(0, 5, 0);

    public CharacterController playercontroller;
    // Start is called before the first frame update
    float getstarts()
    {
        float start = playertransform.position.y;
        return start;
    }

    

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space") && playercontroller.isGrounded)
        {
            playercontroller.Move(jumppower);
        }
    }
}
