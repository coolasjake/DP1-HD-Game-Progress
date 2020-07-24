using System.Collections;
using UnityEngine;

[RequireComponent(typeof(FPSPlayerController))]
public class PickUpObjects : MonoBehaviour
{
    private FPSPlayerController controller;

    private bool carrying;
    private GameObject carriedObject;
    public float distance;
    public float smooth;
    public LayerMask layer;
    private Transform targetObj;
    public Material highlightMat;
    private Material normalMat;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<FPSPlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if(carrying)
        {
            carry(carriedObject);
            checkDrop();
        }
        else
        {
            pickup();
        }
        ChangeMaterial();
    }


    void ChangeMaterial()
    {
        if (targetObj != null)
        {
            targetObj.GetComponentInChildren<Renderer>().material = normalMat;
        }

        if (carrying)
        {
            return;
        }
       
        int x = Screen.width / 2;
        int y = Screen.height / 2;

        Ray ray = controller.mainCamera.ScreenPointToRay(new Vector3(x, y));
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, distance + 1, layer))
        {
            Debug.Log(hit.transform.name);
            if (hit.transform.CompareTag("Pickupable"))
            {

                targetObj = hit.transform;
                normalMat = targetObj.GetComponentInChildren<Renderer>().material;
                targetObj.GetComponentInChildren<Renderer>().material = highlightMat;

            }
        }
        
        
    }
    void rotateObject()
    {
        carriedObject.transform.Rotate(5, 10, 15);
    }

    void carry(GameObject o)
    {
        o.transform.position = Vector3.Lerp(o.transform.position,
            controller.mainCamera.transform.position+ controller.mainCamera.transform.forward * distance, Time.deltaTime * smooth);
        o.GetComponent<Rigidbody>().velocity = Vector3.zero;
        o.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
    }

    void pickup()
    {
        if(Input.GetMouseButtonDown(1))
        {
            int x = Screen.width / 2;
            int y = Screen.height / 2;

            Ray ray = controller.mainCamera.ScreenPointToRay(new Vector3(x, y));
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, distance + 1, layer))
            {
                Debug.Log(hit.transform.name);
                if (hit.transform.CompareTag("Pickupable"))
                {
                    
                    //PickUp p = hit.collider.GetComponent<PickUp>();
                    carrying = true;
                    carriedObject = hit.transform.gameObject;
                    //hit.transform.gameObject.GetComponent<Rigidbody>().isKinematic = true;
                    hit.transform.gameObject.GetComponent<Rigidbody>().useGravity = false;
                }
            }
                
        }
    }

    void checkDrop()
    {
        if(Input.GetMouseButtonDown(1))
        {
            dropObject();
        }
    }

    void dropObject()
    {
        carrying = false;
        //carriedObject.gameObject.GetComponent<Rigidbody>().isKinematic = false;
        carriedObject.gameObject.GetComponent<Rigidbody>().useGravity = true;
        carriedObject = null;
    }
}
