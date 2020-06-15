using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    public Canvas menuCanvas;
    public Canvas UICanvas;
    public Camera transitionCamera;
    public FPSPlayerController player;
    private bool inMenu = true;
    private bool inTransition = false;
    private Vector3 cameraPos = Vector3.zero;
    private Quaternion cameraRot = new Quaternion();

    // Start is called before the first frame update
    void Awake()
    {
        cameraPos = transitionCamera.transform.position;
        cameraRot = transitionCamera.transform.rotation;
    }

    void Update()
    {
        if (!Application.isFocused)
            return;


        if (Input.GetKeyDown(KeyCode.Escape) && !inTransition)
        {
            if (!inMenu)
                Pause();
            else
                ButtonPlay();
        }

        if (!inMenu && Input.GetMouseButtonDown(0))
        {
            if (Cursor.visible)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
    }

    public void ButtonPlay()
    {
        menuCanvas.gameObject.SetActive(false);
        UICanvas.gameObject.SetActive(true);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        inTransition = true;

        StartCoroutine(CameraTransitionIn());
    }

    private IEnumerator CameraTransitionIn()
    {
        float i = 1;
        while (i > 0)
        {
            transitionCamera.transform.position = Vector3.Lerp(transitionCamera.transform.position, player.mainCamera.transform.position, (1f - i) / 15);
            transitionCamera.transform.rotation = Quaternion.Lerp(transitionCamera.transform.rotation, player.mainCamera.transform.rotation, (1f - i) / 15);
            i -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        transitionCamera.transform.position = player.mainCamera.transform.position;
        transitionCamera.transform.rotation = player.mainCamera.transform.rotation;

        transitionCamera.gameObject.SetActive(false);
        player.gameObject.SetActive(true);

        inMenu = false;
        inTransition = false;
    }

    public void Pause()
    {
        inMenu = true;

        transitionCamera.gameObject.SetActive(true);
        player.gameObject.SetActive(false);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        inTransition = true;

        UICanvas.gameObject.SetActive(false);

        StartCoroutine(CameraTransitionOut());
    }

    private IEnumerator CameraTransitionOut()
    {
        transitionCamera.transform.position = player.mainCamera.transform.position;
        transitionCamera.transform.rotation = player.mainCamera.transform.rotation;

        float i = 1;
        while (i > 0)
        {
            transitionCamera.transform.position = Vector3.Lerp(transitionCamera.transform.position, cameraPos, (1f - i) / 15);
            transitionCamera.transform.rotation = Quaternion.Lerp(transitionCamera.transform.rotation, cameraRot, (1f - i) / 15);
            i -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        menuCanvas.gameObject.SetActive(true);

        inTransition = false;
    }

    public void ButtonOptions()
    {

    }

    public void ButtonExit()
    {
        Application.Quit();
        UnityEditor.EditorApplication.ExitPlaymode();
    }
}
