using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorControl : MonoBehaviour
{
    void Start()
    {
        if (Application.isFocused)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    void Update()
    {
        if (!Application.isFocused)
            return;
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Cursor.visible)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (Cursor.visible)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
    }
}
