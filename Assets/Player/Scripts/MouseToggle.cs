using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseToggle : MonoBehaviour
{
    bool mouseVisible = true;


    // Start is called before the first frame update
    void Start()
    {
        SetMouseVisibility(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            mouseVisible = !mouseVisible;
            SetMouseVisibility(mouseVisible);
        }
    }

    public void SetMouseVisibility(bool visibility)
    {
        mouseVisible = visibility;
        if (mouseVisible)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }


}
