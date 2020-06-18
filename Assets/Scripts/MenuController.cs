using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    public Canvas mainMenuCanvas;
    public Canvas optionsMenuCanvas;
    public Canvas resetMenuCanvas;
    public Canvas UICanvas;
    public Camera transitionCamera;
    public FPSPlayerController player;

    public SoundObjectsManager SOM;

    public Transform pointMainMenu;
    public Transform pointOptions;
    public Transform pointReset;

    [System.Serializable]
    public class OptionsMenuItems
    {
        public Slider minAudioSlider;
        public InputField minTextInput;
        public Slider maxAudioSlider;
        public InputField maxTextInput;
        public Slider volumeSlider;
        public InputField volumeTextInput;

        public RectTransform minRep;
        public RectTransform maxRep;
        public float minRange;
        public float maxRange;
        [HideInInspector]
        public float minRepSize;
        [HideInInspector]
        public float maxRepSize;
    }

    public OptionsMenuItems OpMen;

    private bool inMainMenu = true;
    private bool inGameplay = false;
    private bool inTransition = false;
    private Vector3 defaultPlayerPos = Vector3.zero;
    
    void Awake()
    {
        OpMen.maxRepSize = OpMen.maxRep.rect.width;
        OpMen.minRepSize = OpMen.minRep.rect.width;

        UpdateMaxSlide();
        UpdateMinSlide();
        UpdateVolumeSlide();

        OpMen.maxAudioSlider.minValue = OpMen.minRange;
        OpMen.maxAudioSlider.minValue = OpMen.minRange;

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

    public void UpdateMinSlide()
    {
        if (OpMen.minAudioSlider.value > OpMen.maxAudioSlider.value)
            OpMen.minAudioSlider.value = OpMen.maxAudioSlider.value;
        OpMen.minTextInput.text = OpMen.minAudioSlider.value.ToString();
        SOM.SetMinDist(OpMen.minAudioSlider.value);
        RepCircles();
    }

    public void UpdateMaxSlide()
    {
        if (OpMen.maxAudioSlider.value < OpMen.minAudioSlider.value)
            OpMen.maxAudioSlider.value = OpMen.minAudioSlider.value;
        OpMen.maxTextInput.text = OpMen.maxAudioSlider.value.ToString();
        SOM.SetMaxDist(OpMen.maxAudioSlider.value);
        RepCircles();
    }

    public void TextUpdateMin()
    {
        if (OpMen.minTextInput.text == "")
            return;
        float val = float.Parse(OpMen.minTextInput.text);
        if (val > OpMen.maxAudioSlider.value)
            val = OpMen.maxAudioSlider.value;
        OpMen.minAudioSlider.value = val;
        SOM.SetMinDist(OpMen.minAudioSlider.value);
    }

    public void RepCircles()
    {
        float size = (OpMen.maxRepSize - OpMen.minRepSize) * ((OpMen.maxAudioSlider.value - OpMen.minRange) / (OpMen.maxRange - OpMen.minRange)) + OpMen.minRepSize;
        OpMen.maxRep.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, size);
        OpMen.maxRep.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, size);

        size = (OpMen.maxRepSize - OpMen.minRepSize) * ((OpMen.minAudioSlider.value - OpMen.minRange) / (OpMen.maxRange - OpMen.minRange)) + OpMen.minRepSize;
        OpMen.minRep.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, size);
        OpMen.minRep.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, size);
    }

    public void TextUpdateMax()
    {
        if (OpMen.maxTextInput.text == "")
            return;
        float val = float.Parse(OpMen.maxTextInput.text);
        if (val < OpMen.minAudioSlider.value)
            val = OpMen.minAudioSlider.value;
        OpMen.maxAudioSlider.value = val;
        SOM.SetMaxDist(OpMen.maxAudioSlider.value);
    }

    public void UpdateVolumeSlide()
    {
        OpMen.volumeTextInput.text = OpMen.volumeSlider.value.ToString();
        AudioListener.volume = OpMen.volumeSlider.value / 100;
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
