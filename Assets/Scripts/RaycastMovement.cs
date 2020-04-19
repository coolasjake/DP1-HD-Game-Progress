using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider))]
public class RaycastMovement : MonoBehaviour
{
    private CapsuleCollider capCollider;
    Vector3 desiredMotion = Vector3.zero;
    Vector2 desiredRotation = Vector2.zero;
    public LayerMask mask;
    public float sensitivity = 1;
    public float clamp = 1;
    public float moveSpeed = 0.1f;
    public static bool Paused = false;
    private float CameraAngle = 0;
    private Camera camera;

    void Awake()
    {
        capCollider = GetComponent<CapsuleCollider>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        camera = GetComponentInChildren<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
        DoCameraMovement();
    }

    public void Pause()
    {
        Paused = !Paused;
        if (Paused)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Time.timeScale = 0;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            Time.timeScale = 1;
        }
    }

    private void GetInput()
    {
        if (Input.GetKeyDown(KeyCode.BackQuote) || Input.GetKeyDown(KeyCode.Escape))
            Pause();

        if (Paused)
            return;

        if (Input.GetKey(KeyCode.W))
            desiredMotion += transform.forward;
        if (Input.GetKey(KeyCode.A))
            desiredMotion -= transform.right;
        if (Input.GetKey(KeyCode.S))
            desiredMotion -= transform.forward;
        if (Input.GetKey(KeyCode.D))
            desiredMotion += transform.right;

        desiredMotion.Normalize();

        desiredRotation = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
    }

    private void DoCameraMovement()
    {
        float rotationX = desiredRotation.x * sensitivity;
        transform.localRotation *= Quaternion.AngleAxis(rotationX, Vector3.up);
        
        if (CameraAngle > 180 || CameraAngle < -180)
            CameraAngle = ClampAngleTo180(CameraAngle);
        
        CameraAngle -= desiredRotation.y * sensitivity;
        CameraAngle = Mathf.Clamp(CameraAngle, -clamp, clamp);

        Quaternion Rot = new Quaternion();
        Rot.eulerAngles = new Vector3(CameraAngle, 0, 0);
        camera.transform.localRotation = Rot;
    }

    void FixedUpdate()
    {
        DoMovement();
    }

    private void DoMovement()
    {
        if (desiredMotion != Vector3.zero)
        {
            Vector3 raycastPoint = transform.position + (desiredMotion * moveSpeed) + transform.up;
            RaycastHit raycastHit;
            if (Physics.Raycast(raycastPoint, -transform.up, out raycastHit, 10, mask))
            {   //Ground exists at that point.
                Vector3 capPoint = raycastHit.point + (transform.up * (capCollider.radius + 0.2f));
                if (!Physics.CheckCapsule(capPoint, capPoint + (transform.up * capCollider.height), capCollider.radius, mask))
                {
                    transform.position = raycastHit.point + ((capCollider.radius + (capCollider.height * 0.5f)) * transform.up);
                }
            }

            desiredMotion = Vector3.zero;
        }
    }

    /// <summary> Clamps the angle so that it is within the range [-180, 180], while maintaining the relative direction of the angle. </summary>
	public static float ClampAngleTo180(float angle)
    {
        while (angle > 180)
            angle -= 360;

        while (angle < -180)
            angle += 360;

        return angle;
    }
}
