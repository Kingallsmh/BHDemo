using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : BaseController
{
    EntityInterpret interpret;

    private void Start()
    {
        interpret = GetComponent<EntityInterpret>();
        interpret.SetFacing(-1);
    }

    public override void GetInput()
    {
        
    }
}
