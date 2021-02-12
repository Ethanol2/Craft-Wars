using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Xinput;


public class MenuNav : MonoBehaviour
{
    [Header("Setup")]
    public int[] playersWithAccess = new int[1];
    public GameObject startButton;
    public GameObject cursor;

    [Space]
    public float thumbstickTime = 0.25f;

    [Header("Conditions")]
    public bool lockToStartButton = false;

    [Header("Status")]
    public float currentThumbstickTime = 0f;
    public string currentButtonName = "";

    // Private and Protected Variables
    Vector3 lastMousePos;
    PointerEventData buttonInteract;
    protected GameObject currentButton;

    // Enums
    int Left = 0;
    int Right = 1;
    int Up = 2;
    int Down = 3;
    int Jump = 4;

    // Start is called before the first frame update
    void Start()
    {
        currentButton = startButton;
        lastMousePos = Input.mousePosition;
        buttonInteract = new PointerEventData(EventSystem.current);

        cursor.transform.position = startButton.transform.position;
        ExecuteEvents.Execute(currentButton, buttonInteract, ExecuteEvents.pointerEnterHandler);

    }

    // Update is called once per frame
    void Update()
    {
#if (UNITY_EDITOR)
        // Modify Time Scale
        if (GamepadManager.GetControllerKeyReleased(playersWithAccess[0], GamepadManager.ButtonCode.Y))
        {
            Time.timeScale++;
        }
        else if (GamepadManager.GetControllerKeyReleased(playersWithAccess[0], GamepadManager.ButtonCode.X))
        {
            Time.timeScale--;
        }

        // Other stuff
        if (GamepadManager.GetControllerKeyReleased(playersWithAccess[0], GamepadManager.ButtonCode.Shldr_LEFT))
        {
            LevelController.respawnTiles = !LevelController.respawnTiles;
        }
        if (GamepadManager.GetControllerKeyReleased(playersWithAccess[0], GamepadManager.ButtonCode.Shldr_RIGHT))
        {
            LevelController.timedFall = !LevelController.timedFall;
        }

        currentButtonName = currentButton.gameObject.name;
#endif

        if (!currentButton.GetComponent<Button>().interactable && !lockToStartButton)
        {
            try { currentButton = currentButton.GetComponent<ButtonLinker>().getRight(); }
            catch
            {
                try { currentButton = currentButton.GetComponent<ButtonLinker>().getDown(); }
                catch { Debug.Log("Controller cursor locked on inactive button!"); }
            };
        }

        for (int k = 0; k < playersWithAccess.Length; k++)
        {
            // Press A
            if (GamepadManager.GetControllerKeyReleased(playersWithAccess[k], GamepadManager.ButtonCode.A) || keyboardNavigation(Jump))
            {
                ExecuteEvents.Execute(currentButton, buttonInteract, ExecuteEvents.pointerClickHandler);
                if (currentButton.GetComponent<ButtonLinker>().transitionButton)
                {
                    ExecuteEvents.Execute(currentButton, buttonInteract, ExecuteEvents.pointerExitHandler);
                    currentButton = currentButton.GetComponent<ButtonLinker>().target;
                    ExecuteEvents.Execute(currentButton, buttonInteract, ExecuteEvents.pointerEnterHandler);
                }
                currentThumbstickTime = thumbstickTime;
            }

            // Press B
            if (GamepadManager.GetControllerKeyReleased(playersWithAccess[k], GamepadManager.ButtonCode.B) || Input.GetKeyDown(KeyCode.Escape))
            {
                if (currentButton.GetComponent<ButtonLinker>().backButton != null)
                {
                    ExecuteEvents.Execute(currentButton.GetComponent<ButtonLinker>().backButton, buttonInteract, ExecuteEvents.pointerClickHandler);
                    ExecuteEvents.Execute(currentButton, buttonInteract, ExecuteEvents.pointerExitHandler);
                    currentButton = currentButton.GetComponent<ButtonLinker>().backButton.GetComponent<ButtonLinker>().target;
                    ExecuteEvents.Execute(currentButton, buttonInteract, ExecuteEvents.pointerEnterHandler);
                }
                currentThumbstickTime = thumbstickTime;
            }

            if (currentThumbstickTime <= 0f)
            {
                // Press UP
                if (controllerMenuNavigation(playersWithAccess[k], Up) || keyboardNavigation(Up))
                {
                    ExecuteEvents.Execute(currentButton, buttonInteract, ExecuteEvents.pointerExitHandler);
                    try
                    {
                        currentButton = currentButton.GetComponent<ButtonLinker>().getUp();
                    }
                    catch { Debug.Log("UP Direction is NULL"); };

                    cursor.transform.position = currentButton.transform.position;
                    currentThumbstickTime = thumbstickTime;
                    ExecuteEvents.Execute(currentButton, buttonInteract, ExecuteEvents.pointerEnterHandler);
                }

                // Press Down
                else if (controllerMenuNavigation(playersWithAccess[k], Down) || keyboardNavigation(Down))
                {
                    ExecuteEvents.Execute(currentButton, buttonInteract, ExecuteEvents.pointerExitHandler);
                    try
                    {
                        currentButton = currentButton.GetComponent<ButtonLinker>().getDown();
                    }
                    catch { Debug.Log("DOWN Direction is NULL"); };

                    cursor.transform.position = currentButton.transform.position;
                    currentThumbstickTime = thumbstickTime;
                    ExecuteEvents.Execute(currentButton, buttonInteract, ExecuteEvents.pointerEnterHandler);
                }

                // Press Left
                else if (controllerMenuNavigation(playersWithAccess[k], Left) || keyboardNavigation(Left))
                {
                    ExecuteEvents.Execute(currentButton, buttonInteract, ExecuteEvents.pointerExitHandler);
                    try
                    {
                        currentButton = currentButton.GetComponent<ButtonLinker>().getLeft();
                    }
                    catch { Debug.Log("LEFT Direction is NULL"); };
                    cursor.transform.position = currentButton.transform.position;
                    currentThumbstickTime = thumbstickTime;
                    ExecuteEvents.Execute(currentButton, buttonInteract, ExecuteEvents.pointerEnterHandler);
                }

                // Press Right
                else if (controllerMenuNavigation(playersWithAccess[k], Right) || keyboardNavigation(Right))
                {
                    ExecuteEvents.Execute(currentButton, buttonInteract, ExecuteEvents.pointerExitHandler);
                    try
                    {
                        currentButton = currentButton.GetComponent<ButtonLinker>().getRight();
                    }
                    catch { Debug.Log("RIGHT Direction is NULL"); };
                    cursor.transform.position = currentButton.transform.position;
                    currentThumbstickTime = thumbstickTime;
                    ExecuteEvents.Execute(currentButton, buttonInteract, ExecuteEvents.pointerEnterHandler);
                }

            }
        }

        currentThumbstickTime -= Time.fixedDeltaTime; //Time.deltaTime;
    }

    public bool controllerMenuNavigation(int a_PlayerNumber, int a_Direction)
    {
        if (currentThumbstickTime <= 0f)
        {
            GamepadManager.gamepadAxes axes = GamepadManager.GetControllerAxes(a_PlayerNumber);

            if (a_Direction == Left && (GamepadManager.GetControllerKeyDown(a_PlayerNumber, GamepadManager.ButtonCode.DPad_LEFT) || axes.l_ThumbStick_X <= -0.3f))
            {
                return true;
            }
            if (a_Direction == Right && (GamepadManager.GetControllerKeyDown(a_PlayerNumber, GamepadManager.ButtonCode.DPad_RIGHT) || axes.l_ThumbStick_X >= 0.3f))
            {
                return true;
            }
            if (a_Direction == Up && (GamepadManager.GetControllerKeyDown(a_PlayerNumber, GamepadManager.ButtonCode.DPad_UP) || axes.l_ThumbStick_Y >= 0.3f))
            {
                audioManager.AudioPlayer.playAudio((int)audioManager.AudioPlayer.clips.click03);
                return true;
            }
            if (a_Direction == Down && (GamepadManager.GetControllerKeyDown(a_PlayerNumber, GamepadManager.ButtonCode.DPad_DOWN) || axes.l_ThumbStick_Y <= -0.3f))
            {
                audioManager.AudioPlayer.playAudio((int)audioManager.AudioPlayer.clips.click03);
                return true;
            }
        }
        return false;
    }

    public bool keyboardNavigation(int a_Direction)
    {
        if (currentThumbstickTime <= 0f)
        {
            if (a_Direction == Left && (Input.GetKey("a") || Input.GetKey(KeyCode.LeftArrow)))
            {
                return true;
            }
            if (a_Direction == Right && (Input.GetKey("d") || Input.GetKey(KeyCode.RightArrow)))
            {
                return true;
            }
            if (a_Direction == Up && (Input.GetKey("w") || Input.GetKey(KeyCode.UpArrow)))
            {
                audioManager.AudioPlayer.playAudio((int)audioManager.AudioPlayer.clips.click03);
                return true;
            }
            if (a_Direction == Down && (Input.GetKey("s") || Input.GetKey(KeyCode.DownArrow)))
            {
                audioManager.AudioPlayer.playAudio((int)audioManager.AudioPlayer.clips.click03);
                return true;
            }
            if (a_Direction == Jump && (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.Keypad0)))
            {
                audioManager.AudioPlayer.playAudio((int)audioManager.AudioPlayer.clips.click03);
                return true;
            }
        }
        return false;
    }

    public void setCurrentButton(GameObject a_button)
    {
        currentButton = a_button;
        return;
    }
}
