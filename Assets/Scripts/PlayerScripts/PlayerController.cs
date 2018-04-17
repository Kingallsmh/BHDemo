using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : BaseController {

    public int playerNum = 1;

    /// <summary>
    /// Reads input from the registered controller values in the project settings.
    /// </summary>
    public override void GetInput()
    {
        switch (playerNum)
        {
            case 1: PlayerOneInput();
                break;
            case 2: PlayerTwoInput();
                break;
        }
        
    }

    void PlayerOneInput()
    {
        directionInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        btn0 = Input.GetAxisRaw("Button0") > 0;
        btn1 = Input.GetAxisRaw("Button1") > 0;
        btn2 = Input.GetAxisRaw("Button2") > 0;
        btn3 = Input.GetAxisRaw("Button3") > 0;
    }

    void PlayerTwoInput()
    {
        directionInput = new Vector2(Input.GetAxisRaw("P2Horizontal"), Input.GetAxisRaw("P2Vertical"));
        btn0 = Input.GetAxisRaw("P2Button0") > 0;
        btn1 = Input.GetAxisRaw("P2Button1") > 0;
        btn2 = Input.GetAxisRaw("P2Button2") > 0;
        btn3 = Input.GetAxisRaw("P2Button3") > 0;
    }
}