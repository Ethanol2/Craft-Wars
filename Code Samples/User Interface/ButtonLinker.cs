using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[AddComponentMenu("UI/Button Linker")]
[RequireComponent(typeof(Button))]
[DisallowMultipleComponent]
public class ButtonLinker : MonoBehaviour, IPointerDownHandler
{
    [Header("Neighbours")]
    public GameObject left;
    public GameObject right;
    public GameObject up;
    public GameObject down;

    [Space]
    [Tooltip("Button trigered when the player presses B or Esc")]
    public GameObject backButton;

    [Header("Conditions")]
    [Tooltip("If the button is a part of a row with variable ammount of buttons. Menu will jump to button on Left/Right instead of skipping if inactive")]
    public bool variableRow = false;
    [Tooltip("If the button is a part of a row with variable ammount of buttons. Menu will jump to button on Up/Down instead of skipping if inactive")]
    public bool variableCollumn = false;
    [Tooltip("Prevents cursor from going to opposite side of the menu")]
    public bool preventLooping = false;

    [Space]
    [Tooltip("Button leads to new menu in the same scene (target)")]
    public bool transitionButton = false;
    public GameObject target;

    // ================================= Left ===================================
    public GameObject getLeft()
    {
        //Debug.Log("Getting Left");
        if (!left.GetComponent<Button>().interactable)
        {
            if (left.GetComponent<ButtonLinker>().variableCollumn)
            {
                return left.GetComponent<ButtonLinker>().getDown(this.gameObject, false);
            }
            return left.GetComponent<ButtonLinker>().getLeft();
        }
        else if (left == null)
        {
            return this.gameObject;
        }

        left.GetComponent<ButtonLinker>().right = this.gameObject;
        return left;
    }

    // ================================= Right ===================================

    public GameObject getRight()
    {
        //Debug.Log("Getting Right");
        if (!right.GetComponent<Button>().interactable)
        {
            if (right.GetComponent<ButtonLinker>().variableCollumn)
            {
                return right.GetComponent<ButtonLinker>().getDown(this.gameObject, true);
            }
            return right.GetComponent<ButtonLinker>().getRight();
        }
        else if (right == null)
        {
            return this.gameObject;
        }

        right.GetComponent<ButtonLinker>().left = this.gameObject;
        return right;
    }

    public GameObject getRight(GameObject a_firstCall, bool a_dir)
    {
        //Debug.Log("Getting Right");
        if (!right.GetComponent<Button>().interactable)
        {
            if (right.GetComponent<ButtonLinker>().variableCollumn)
            {
                return right.GetComponent<ButtonLinker>().getDown(a_firstCall, a_dir);
            }
            if (a_firstCall == this.gameObject)
            {
                return a_dir ? up : down;
            }
            return right.GetComponent<ButtonLinker>().getRight(a_firstCall, a_dir);
        }
        else if (right == null)
        {
            return this.gameObject;
        }

        right.GetComponent<ButtonLinker>().left = this.gameObject;
        return right;
    }

    // ================================= Up ===================================

    public GameObject getUp()
    {
        //Debug.Log("Getting Up");
        if (!up.GetComponent<Button>().interactable)
        {
            if (up.GetComponent<ButtonLinker>().variableRow)
            {
                return up.GetComponent<ButtonLinker>().getRight(this.gameObject, true);
            }
            return up.GetComponent<ButtonLinker>().getUp();
        }
        else if (up == null)
        {
            return this.gameObject;
        }

        up.GetComponent<ButtonLinker>().down = this.gameObject;
        return up;
    }

    // ================================= Down ===================================

    public GameObject getDown()
    {
        //Debug.Log("Getting Down");
        if (!down.GetComponent<Button>().interactable)
        {
            if (down.GetComponent<ButtonLinker>().variableRow)
            {
                return down.GetComponent<ButtonLinker>().getRight(this.gameObject, false);
            }
            return down.GetComponent<ButtonLinker>().getDown();
        }
        else if (down == null)
        {
            return this.gameObject;
        }

        down.GetComponent<ButtonLinker>().up = this.gameObject;
        return down;
    }

    public GameObject getDown(GameObject a_firstCall, bool a_dir)
    {
        //Debug.Log("Getting Down");
        if (!down.GetComponent<Button>().interactable)
        {
            if (down.GetComponent<ButtonLinker>().variableRow)
            {
                return down.GetComponent<ButtonLinker>().getRight(a_firstCall, a_dir);
            }
            if (a_firstCall == this.gameObject)
            {
                return a_dir ? right.GetComponent<ButtonLinker>().right : left.GetComponent<ButtonLinker>().left;
            }
            return down.GetComponent<ButtonLinker>().getDown(a_firstCall, a_dir);
        }
        else if (down == null)
        {
            return this.gameObject;
        }

        down.GetComponent<ButtonLinker>().up = this.gameObject;
        return down;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (transitionButton)
        {
            GameObject.FindWithTag("Menu").GetComponent<MenuNav>().setCurrentButton(target);
        }
    }
}