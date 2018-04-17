using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerScript : MonoBehaviour {

    public static GameManagerScript Instance;
    bool pauseInput = false; //Prevent player from using input, maybe with the exception of a button
    bool pauseActions = false; //Prevents entities from recieving inputs or doing actions
    bool pauseAnimations = false; // Stops animations

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }



    public bool PauseInput
    {
        get
        {
            return pauseInput;
        }

        set
        {
            pauseInput = value;
        }
    }

    public bool PauseActions
    {
        get
        {
            return pauseActions;
        }

        set
        {
            pauseActions = value;
        }
    }
}
