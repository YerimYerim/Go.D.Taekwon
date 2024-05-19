using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class InputManager
{
    //Using Xbox XInput controller bindings 
    public static bool AButtonDown()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.JoystickButton0))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public static bool AButtonUp()
    {
        if (Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.JoystickButton0))
        {
            return true;
        }
        else
        {
            return false;
        }
    }


    public static bool BButtonDown()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift) || Input.GetKeyDown(KeyCode.JoystickButton1))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public static bool XButtonDown()
    {
        if (Input.GetKeyDown(KeyCode.C) || Input.GetKeyDown(KeyCode.JoystickButton2))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public static bool XButtonHold()
    {
        if (Input.GetKey(KeyCode.C) || Input.GetKey(KeyCode.JoystickButton2))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public static bool XButtonUp()
    {
        if (Input.GetKeyUp(KeyCode.C) || Input.GetKeyUp(KeyCode.JoystickButton2))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public static bool YButtonHold()
    {
        if (Input.GetKey(KeyCode.V) || Input.GetKey(KeyCode.JoystickButton3))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public static bool YButtonUp()
    {
        if (Input.GetKeyUp(KeyCode.V) || Input.GetKeyUp(KeyCode.JoystickButton3))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public static bool RBButtonDown()
    {
        if (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.JoystickButton5))
        { 
            return true;
        }
        else
        {
            return false;
        }
    }

    public static bool CtrlKeyHold()
    {
        if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public static bool CtrlKeyUp()
    {
        if (Input.GetKeyUp(KeyCode.LeftControl) || Input.GetKeyUp(KeyCode.RightControl))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

 }
