using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    public Canvas mainMenuCanvas;
    public Canvas optionsMenuCanvas;
    public Canvas resetMenuCanvas;
    public Canvas UICanvas;
    public Camera transitionCamera;
    public FPSPlayerController player;

    public Transform pointMainMenu;
    public Transform pointOptions;
    public Transform pointReset;

    private bool inMainMenu = true;
    private bool inGameplay = false;
    private bool inTransition = false;
    private Vector3 defaultPlayerPos = Vector3.zero;
    
    void Awake()
    {
        transitionCamera.transform.position = pointMainMenu.position;
        transitionCamera.transform.rotation = pointMainMenu.rotation;

        defaultPlayerPos = player.transform.position;
    }

    void Update()
    {
        if (!Application.isFocused)
            return;


        if (Input.GetKeyDown(KeyCode.Escape) && !inTransition)
        {
            if (inMainMenu)
                Play();
            else
                ToMainMenu();
        }

        if (inGameplay && Input.GetMouseButtonDown(0))
        {
            if (Cursor.visible)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
    }

    public void Play()
    {
        StartCoroutine(CameraTransitionIn());
    }

    public void ToMainMenu()
    {
        inMainMenu = true;
        StartCoroutine(CameraTransitionTo(pointMainMenu, mainMenuCanvas));
    }
    
    public void ButtonOptions()
    {
        inMainMenu = false;

        StartCoroutine(CameraTransitionTo(pointOptions, optionsMenuCanvas));
    }

    public void ButtonReset()
    {
        inMainMenu = false;

        StartCoroutine(CameraTransitionTo(pointReset, resetMenuCanvas));
    }

    public void ButtonExit()
    {
        Application.Quit();
        UnityEditor.EditorApplication.ExitPlaymode();
    }

    public void ResetPlayer()
    {
        player.transform.position = defaultPlayerPos;
    }

    private IEnumerator CameraTransitionTo(Transform point, Canvas UItoActivate)
    {
        transitionCamera.gameObject.SetActive(true);
        player.gameObject.SetActive(false);

        transitionCamera.transform.SetParent(null);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        
        inGameplay = false;
        inTransition = true;

        DeactivateCanvases();

        float i = 1;
        while (i > 0)
        {
            transitionCamera.transform.position = Vector3.Lerp(transitionCamera.transform.position, point.position, (1f - i) / 15);
            transitionCamera.transform.rotation = Quaternion.Lerp(transitionCamera.transform.rotation, point.rotation, (1f - i) / 15);
            i -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        UItoActivate.gameObject.SetActive(true);

        inTransition = false;
    }

    private IEnumerator CameraTransitionIn()
    {
        inTransition = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        DeactivateCanvases();

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

        transitionCamera.transform.SetParent(player.transform);

        transitionCamera.gameObject.SetActive(false);
        player.gameObject.SetActive(true);

        UICanvas.gameObject.SetActive(true);

        inMainMenu = false;
        inGameplay = true;
        inTransition = false;
    }

    private void DeactivateCanvases()
    {
        mainMenuCanvas.gameObject.SetActive(false);
        UICanvas.gameObject.SetActive(false);
        optionsMenuCanvas.gameObject.SetActive(false);
        resetMenuCanvas.gameObject.SetActive(false);
    }
}
