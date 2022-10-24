using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseVis : MonoBehaviour
{
    public KeyCode mouseKey;
    public bool visible;
    public float sensDefault;

    // Start is called before the first frame update
    void Start()
    {
        sensDefault = Options.xSens;
        visible = false;
        MouseMode(visible);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(mouseKey))
        {
            visible = !visible;
        }

        MouseMode(visible);
    }

    public void MouseMode(bool mouseSwitch)
    {
        if(!mouseSwitch)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            Options.xSens = sensDefault;
            Options.ySens = sensDefault;
        }
        else if(mouseSwitch)
        {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;

            Options.xSens = 0;
            Options.ySens = 0;
        }
    }
}