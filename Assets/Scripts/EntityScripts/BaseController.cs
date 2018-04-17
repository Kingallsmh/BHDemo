using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseController : MonoBehaviour {

    protected bool btn0, btn1, btn2, btn3;
    protected Vector2 directionInput;

    abstract public void GetInput();

    public void ResetInput()
    {
        btn0 = false;
        btn1 = false;
        btn2 = false;
        btn3 = false;
        directionInput = Vector2.zero;
    }
    /// <summary>
    /// Get or set directionInput.
    /// </summary>
    public Vector2 DirectionInput
    {
        get
        {
            return directionInput;
        }

        set
        {
            directionInput = value;
        }
    }

    /// <summary>
    /// Return if the button wanted is being pressed.
    /// </summary>
    /// <param name="btnNum"></param>
    /// <returns></returns>
    public bool GetButton(int btnNum)
    {
        switch (btnNum)
        {
            case 0: return btn0;
            case 1: return btn1;
            case 2: return btn2;
            case 3: return btn3;
        }
        return false;
    }

    /// <summary>
    /// Sets the button's boolean value.
    /// </summary>
    /// <param name="btnNum"></param>
    /// <param name="pressed"></param>
    public void SetButton(int btnNum, bool pressed)
    {
        switch (btnNum)
        {
            case 0:
                btn0 = pressed;
                break;
            case 1:
                btn1 = pressed;
                break;
            case 2:
                btn2 = pressed;
                break;
            case 3:
                btn3 = pressed;
                break;
        }
    }
}
