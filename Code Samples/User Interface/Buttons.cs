
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using audioManager;

public class Buttons : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [Header("Setup")]
    public GameObject menuContainer;
    public Vector3 initialPos;
    public Vector3 initialScale;
    public bool triggerEventOnEnter = false;
    public UnityEvent enterEvent;
    public bool triggerEventOnExit = false;
    public UnityEvent exitEvent;
    Vector3 slideOutPosition;
    public bool mirror = false;
    public bool move = true;
    public bool x = true;
    public bool y, z;
    public bool scale = false;

    static public float slideOutDist = 15f;
    static public float scaleSize = 1.1f;
    float usedScaleSize = 1.1f;

    [Space]
    [Header("Slide Distance")]
    public bool useCustomSlideDistance = false;
    public float slideDist;

    [Space]
    [Header("Scale Size")]
    public bool useCustomScale = false;
    public float customScaleSize;

    // Use this for initialization
    void Start()
    {
        if (this.GetComponent<Button>().interactable)
        {
            initialPos = this.transform.localPosition;
            float tempNum = slideOutDist;
            if (useCustomSlideDistance)
            {
                tempNum = slideDist;
            }
            if (!mirror)
            {
                if (x)
                {
                    slideOutPosition = new Vector3(initialPos.x + tempNum, initialPos.y, initialPos.z);
                }
                else if (y)
                {
                    slideOutPosition = new Vector3(initialPos.x, initialPos.y + tempNum, initialPos.z);
                }
                else if (z)
                {
                    slideOutPosition = new Vector3(initialPos.x, initialPos.y, initialPos.z + tempNum);
                }
            }
            else
            {
                if (x)
                {
                    slideOutPosition = new Vector3(initialPos.x - tempNum, initialPos.y, initialPos.z);
                }
                else if (y)
                {
                    slideOutPosition = new Vector3(initialPos.x, initialPos.y - tempNum, initialPos.z);
                }
                else if (z)
                {
                    slideOutPosition = new Vector3(initialPos.x, initialPos.y, initialPos.z - tempNum);
                }
            }
        }

        initialScale = this.transform.localScale;

        usedScaleSize = scaleSize;
        if (useCustomScale)
        {
            usedScaleSize = customScaleSize;
        }

        if (enterEvent == null)
        {
            enterEvent = new UnityEvent();
        }
         if (exitEvent == null)
        {
            exitEvent = new UnityEvent();
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (this.GetComponent<Button>().interactable && (move || scale))
        {
            if (move)
            {
                menuContainer.GetComponent<UXEffects>().moveElement(this.gameObject, slideOutPosition);
            }
            else if (scale)
            {
                menuContainer.GetComponent<UXEffects>().scaleElement(this.gameObject, initialScale * usedScaleSize);
            }

            if (triggerEventOnEnter)
            {
                enterEvent.Invoke();
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (this.GetComponent<Button>().interactable && (move || scale))
        {
            if (move)
            {
                menuContainer.GetComponent<UXEffects>().moveElement(this.gameObject, initialPos);
            }
            else if (scale)
            {
                menuContainer.GetComponent<UXEffects>().scaleElement(this.gameObject, initialScale);
            }
        
            if (triggerEventOnExit)
            {
                exitEvent.Invoke();
            }
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // Play Click Sound

        AudioPlayer.playAudio((int)AudioPlayer._menuClick);

    }

    public void setInteractable(bool interactable, Vector3 interactablePos)
    {
        if (interactable)
        {
            initialPos = interactablePos;
            this.GetComponent<Button>().interactable = true;
            this.GetComponent<ButtonLinker>().enabled = true;
            float tempNum = slideOutDist;
            if (useCustomSlideDistance)
            {
                tempNum = slideDist;
            }
            if (!mirror)
            {
                if (x)
                {
                    slideOutPosition = new Vector3(initialPos.x + tempNum, initialPos.y, initialPos.z);
                }
                else if (y)
                {
                    slideOutPosition = new Vector3(initialPos.x, initialPos.y + tempNum, initialPos.z);
                }
                else if (z)
                {
                    slideOutPosition = new Vector3(initialPos.x, initialPos.y, initialPos.z + tempNum);
                }
            }
            else
            {
                if (x)
                {
                    slideOutPosition = new Vector3(initialPos.x - tempNum, initialPos.y, initialPos.z);
                }
                else if (y)
                {
                    slideOutPosition = new Vector3(initialPos.x, initialPos.y - tempNum, initialPos.z);
                }
                else if (z)
                {
                    slideOutPosition = new Vector3(initialPos.x, initialPos.y, initialPos.z - tempNum);
                }
            }
        }
        else
        {
            this.GetComponent<Button>().interactable = false;
            this.GetComponent<ButtonLinker>().enabled = false;
        }
    }

    public void resetButton()
    {
        if (move)
        {
            menuContainer.GetComponent<UXEffects>().moveElement(this.gameObject, initialPos);
        }
        if (scale)
        {
            menuContainer.GetComponent<UXEffects>().scaleElement(this.gameObject, initialScale);
        }
    }

}
