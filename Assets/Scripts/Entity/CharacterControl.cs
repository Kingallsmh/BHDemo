using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControl : BaseController {
    public override void GetInput()
    {
        directionInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        btn0 = Input.GetAxisRaw("Jump") > 0;
        btn1 = Input.GetAxisRaw("Fire1") > 0;
        btn2 = Input.GetAxisRaw("Fire2") > 0;
    }
}
