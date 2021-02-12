// Programmed by Ethan Colucci
// This child script of PlayerController accepts the input of a gamepad and standardizes the output for gameplay programming. This script uses a custom xinput dll

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGamepad : PlayerController
{
	// Enums allow devs to modify the controls via the inspector
    private enum Buttons
    {
        A = 0,
        B = 1,
        X = 2,
        Y = 3,

        DPad_UP = 4,
        DPad_DOWN = 5,
        DPad_LEFT = 6,
        DPad_RIGHT = 7,

        Shldr_LEFT = 8,
        Shldr_RIGHT = 9,

        ThumbStick_LEFT = 10,
        ThumbStick_RIGHT = 11,

        Start = 12,
        Back = 13
    }
    private enum Axes
    {
        ThumbStick_L = 0,
        ThumbStick_R = 1,

        Trigger_RIGHT = 2,
        Trigger_LEFT = 3
    }

	// This abstract class allows for a hidden distinction in between input thats meant for instant release and holding
    private abstract class inputType
    {
        public int keyCode;
        public int output;

        public inputType(int a_keyCode, int a_output)
        {
            keyCode = a_keyCode;
            output = a_output;
        }
        public abstract bool checkInput(int a_GamepadID);
    }
	
	// This class uses the dll function that checks for a button tap
    private class releaseType : inputType
    {
        public releaseType(int a_keyCode, int a_output) : base(a_keyCode, a_output) { }
        public override bool checkInput(int a_GamepadID)
        {
            return Xinput.GamepadManager.GetControllerKeyReleased(a_GamepadID, keyCode);
        }
    }
    // This class uses dll function that checks for a button press on down
    private class reactType : inputType
    {
        public reactType(int a_keyCode, int a_output) : base(a_keyCode, a_output) { }
        public override bool checkInput(int a_GamepadID)
        {
            return Xinput.GamepadManager.GetControllerKeyPressed(a_GamepadID, keyCode);
        }
    }
	// This class uses dll function that checks for a button hold
    private class holdType : inputType
    {
        public holdType(int a_keyCode, int a_output) : base(a_keyCode, a_output) { }
        public override bool checkInput(int a_GamepadID)
        {
            return Xinput.GamepadManager.GetControllerKeyDown(a_GamepadID, keyCode);
        }
    }

    // The developers assign the inputs in the inspector
    [Header("Inputs")]
    [SerializeField]
    Buttons shoot;
    [SerializeField]
    Buttons jumpTap;
    [SerializeField]
    Buttons jumpHold;
    [SerializeField]
    Buttons openMenu;
    [SerializeField]
    Buttons backButton;
    
    [Header("Axis")]
    [SerializeField]
    Axes move;

    readonly inputType[] inputs = new inputType[5];
    readonly Axes[] axis = new Axes[1];
    void Start()
    { 
        // Fill inputs array
        {
            inputs[(int)OutputTypes.Shoot] =    new holdType((int)shoot, (int)OutputTypes.Shoot);
            inputs[(int)OutputTypes.JumpTap] =  new reactType((int)jumpTap, (int)OutputTypes.JumpTap);
            inputs[(int)OutputTypes.OpenMenu] = new releaseType((int)openMenu, (int)OutputTypes.OpenMenu);
            inputs[(int)OutputTypes.Back] =     new releaseType((int)backButton, (int)OutputTypes.Back);
            inputs[(int)OutputTypes.JumpHold] = new holdType((int)jumpHold, (int)OutputTypes.JumpHold);
        }

        // Fill axis array
        {
            axis[(int)OutputTypes.Move] = move;
        }
    }

    // The only part of the script not edited per game. The update loop interprets the input and outputs
    void Update()
    {
        // Check Buttons
        foreach (inputType input in inputs)
        {
            outputs[input.output] = input.checkInput(inputNumber) & !killSwitch;
        }

        int freezeAxes = killSwitch ? 0 : 1;

        // Check Axis
        Xinput.GamepadManager.gamepadAxes axes = Xinput.GamepadManager.GetControllerAxes(inputNumber);
        Vector2[] axesV2 = new Vector2[4];
        axesV2[0].x = axes.l_ThumbStick_X; axesV2[0].y = axes.l_ThumbStick_Y;
        axesV2[1].x = axes.r_ThumbStick_X; axesV2[1].y = axes.r_ThumbStick_Y;
        axesV2[2].x = axes.r_Trigger; axesV2[2].y = axes.r_Trigger;
        axesV2[3].x = axes.l_Trigger; axesV2[3].y = axes.l_Trigger; ;

        for (int k = 0; k < axis.Length; k++)
        {
            axisOutputs[k] = axesV2[(int)axis[k]] * freezeAxes;
        }
    }
}
