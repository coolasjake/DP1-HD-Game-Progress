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

    // Start is called before the first frame update
    void Start()
    {

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

        if (Input.GetKey(KeyCode.W))
            desiredMotion += Vector3.forward;
        if (Input.GetKey(KeyCode.A))
            desiredMotion -= Vector3.right;
        if (Input.GetKey(KeyCode.S))
            desiredMotion -= Vector3.forward;
        if (Input.GetKey(KeyCode.D))
            desiredMotion += Vector3.right;

        desiredRotation = Input.mousePosition;
    }

    private void DoCameraMovement()
    {

        //CAMERA CONTROL
        //Rotate player
        float rotationX = Input.GetAxis("Mouse X") * sensitivity;
        transform.localRotation *= Quaternion.AngleAxis(rotationX, Vector3.up);

        //CAMERA X-ROTATION
        //Clamp the angle to a range of -180 to 180 for easier maths.
        if (CameraAngle > 180 || CameraAngle < -180)
            CameraAngle = ClampAngleTo180(CameraAngle);
        
        //Apply the mouse input.
        CameraAngle -= Input.GetAxis("Mouse Y") * sensitivity;
        //Clamp the angle.
        CameraAngle = Mathf.Clamp(CameraAngle, -clamp, clamp);

        Quaternion Rot = new Quaternion();
        Rot.eulerAngles = new Vector3(CameraAngle, 0, 0);
        camera.transform.rotation = Rot;
    }

    void FixedUpdate()
    {
        DoMovement();
    }

    private void DoMovement()
    {
        if (desiredMotion != Vector3.zero)
        {
            Vector3 raycastPoint = transform.position + desiredMotion + transform.up;
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
