// Programmed by Ethan Colucci
// This child script of PlayerController accepts the input of a keyboard and standardizes the output for gameplay programming.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerKeyboard : PlayerController
{
	// This abstract class allows for a hidden distinction in between input thats meant for instant release and holding
    private abstract class inputType
    {
        public KeyCode keyCode;
        public int output;

        public inputType(KeyCode a_keyCode, int a_output)
        {
            keyCode = a_keyCode;
            output = a_output;
        }
        public abstract bool checkInput();
    }
	
	// This class uses the dll function that checks for a button tap
    private class releaseType : inputType
    {
        public releaseType(KeyCode a_keyCode, int a_output) : base(a_keyCode, a_output) { }
        public override bool checkInput()
        {
            return Input.GetKeyDown(keyCode);
        }
    }
	
	// This class uses dll function that checks for a button hold
    private class holdType : inputType
    {
        public holdType(KeyCode a_keyCode, int a_output) : base(a_keyCode, a_output) { }
        public override bool checkInput()
        {
            return Input.GetKey(keyCode);
        }
    }

	// The developers assign the inputs in the inspector
    [Header("inputs")]
    [SerializeField]
    KeyCode shoot;
    [SerializeField]
    KeyCode jumpTap;
    [SerializeField]
    KeyCode jumpHold;
    [SerializeField]
    KeyCode openMenu;
    [SerializeField]
    KeyCode backButton;

    [Header("Axis")]
    [SerializeField]
    KeyCode[] move = new KeyCode[4];

    inputType[] inputs = new inputType[5];
    List<KeyCode[]> axis = new List<KeyCode[]>();

    void Start()
    {
        // Fill Inputs Array
        {
            inputs[(int)OutputTypes.Shoot] = new holdType(shoot, (int)OutputTypes.Shoot);
            inputs[(int)OutputTypes.JumpTap] = new releaseType(jumpTap, (int)OutputTypes.JumpTap);
            inputs[(int)OutputTypes.OpenMenu] = new releaseType(openMenu, (int)OutputTypes.OpenMenu);
            inputs[(int)OutputTypes.Back] = new releaseType(backButton, (int)OutputTypes.Back);
            inputs[(int)OutputTypes.JumpHold] = new holdType(jumpHold, (int)OutputTypes.JumpHold);
        }

        // Fill axis list
        {
            axis.Add(move);
        }
        
    }

    // The only part of the script not edited per game. The update loop interprets the input and outputs
    void Update()
    {
        // Check Buttons
        foreach (inputType input in inputs)
        {
            outputs[input.output] = input.checkInput() & !killSwitch;
        }
        
		// Since axis input is done using 4 buttons on a keyboard, the script combines the 4 inputs to make a controller like Vector2
        for (int k = 0; k < axis.Count; k++)
        {
            axisOutputs[k].x =
                (Input.GetKey(axis[k][1]) ? -1 : 0) + (Input.GetKey(axis[k][3]) ? 1 : 0);
            axisOutputs[k].y =
                (Input.GetKey(axis[k][0]) ? 1 : 0) + (Input.GetKey(axis[k][2]) ? -1 : 0);
            axisOutputs[k] *= (killSwitch ? 0 : 1);
        }
    }
}
