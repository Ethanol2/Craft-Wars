// Programmed by Ethan Colucci

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

namespace Xinput
{
    public class GamepadManager : MonoBehaviour
    {
        //Analog Stick information
        public struct gamepadAxes
        {
            public float l_ThumbStick_X;
            public float l_ThumbStick_Y;

            public float r_ThumbStick_X;
            public float r_ThumbStick_Y;

            public float l_Trigger;
            public float r_Trigger;
        };

        //List of Buttons
        public struct ButtonCode
        {
            static public int A;
            static public int B;
            static public int X;
            static public int Y;

            static public int DPad_UP;
            static public int DPad_DOWN;
            static public int DPad_LEFT;
            static public int DPad_RIGHT;

            static public int Shldr_LEFT;
            static public int Shldr_RIGHT;

            static public int ThumbStick_LEFT;
            static public int ThumbStick_RIGHT;

            static public int Start;
            static public int Back;
            static public void init()
            {
                A = 0;
                B = 1;
                X = 2;
                Y = 3;

                DPad_UP = 4;
                DPad_DOWN = 5;
                DPad_LEFT = 6;
                DPad_RIGHT = 7;

                Shldr_LEFT = 8;
                Shldr_RIGHT = 9;

                ThumbStick_LEFT = 10;
                ThumbStick_RIGHT = 11;

                Start = 12;
                Back = 13;
            }
        };

        // Setup for gamepad IDs (0 to 3)
        [DllImport("ControllerInput")]
        public static extern void InitGamepads();

        // Checks gamepad state and updates input
        [DllImport("ControllerInput")]
        public static extern bool CheckGamepadConnected(int a_GamepadNum);

        // Returns true if the button is pressed
        [DllImport("ControllerInput")]
        public static extern bool GetControllerKeyDown(int a_GamepadNum, int a_ButtonID);

        //returns the analog stick input
        [DllImport("ControllerInput")]
        public static extern gamepadAxes GetControllerAxes(int a_GamepadNum);

        //sets rumble to constant
        [DllImport("ControllerInput")]
        public static extern void SetControllerRumble(int a_GamepadNum, float a_LeftMotor, float a_RightMotor);

        // Returns true if button was pressed after last update
        [DllImport("ControllerInput")]
        public static extern bool GetControllerKeyPressed(int a_GamepadNum, int a_ButtonID);

        //Returns true if button was released after the last update
        [DllImport("ControllerInput")]
        public static extern bool GetControllerKeyReleased(int a_GamepadNum, int a_ButtonID);

        // Returns true if button is pressed
        [DllImport("ControllerInput")]
        public static extern bool GetCommandDown(int a_Player, string a_Command);

        // Returns true if command was pressed after last update
        [DllImport("ControllerInput")]
        public static extern bool GetCommandPressed(int a_Player, string a_Command);

        // Returns true if command was released after last update
        [DllImport("ControllerInput")]
        public static extern bool GetCommandReleased(int a_Player, string a_Command);

        //sets rumble for X time
        [DllImport("ControllerInput")]
        public static extern void SetControllerRumbleTime(int gamepadNum, float leftMotor, float rightMotor, double time);

        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

        public static bool xinputStarted = false;
        public static bool[] connected = new bool[4];
        public static int gamepadsConnected = 0;

        // Use this for initialization
        void Start()
        {
            if (!xinputStarted)
            {
                xinputStarted = true;
                InitGamepads();
                ButtonCode.init();
            }
            for (int k = 0; k < 4; k++)
            {
                connected[k] = CheckGamepadConnected(k);
                if (connected[k])
                {
                    gamepadsConnected++;
                }
            }

        }

        // Update is called once per frame
        void Update()
        {
            gamepadsConnected = 0;
            for (int k = 0; k < 4; k++)
            {
                connected[k] = CheckGamepadConnected(k);
                if (connected[k])
                {
                    gamepadsConnected++;
                }
            }
        }
    }
}
