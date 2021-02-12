// Programmed by Ethan Colucci
// This script abstracts player input for use in gameplay

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Xinput;	// Custom Xinput dll

public class PlayerController : MonoBehaviour
{
    [Header("Setup")]
    public bool killSwitch = false;					// Freezes the input
    public int inputNumber;							// Identifier number for the input method

    // The possible inputs allowed by the game. These values can be modified for each game. These arrays are non serialized to prevent the unity inspector from modifying the values
    [System.NonSerialized]
    public bool[] outputs = new bool[5];			// The number of binary inputs used by the game.
    [System.NonSerialized]
    public Vector2[] axisOutputs = new Vector2[1];	// The number of axis inputs used by the game.

    // Enums for ease of programming
    protected enum OutputTypes
    {
        // Buttons
        Shoot = 0, JumpTap = 1, OpenMenu = 2, Back = 3, JumpHold = 4,
        // Axes
        Move = 0
    }
}
