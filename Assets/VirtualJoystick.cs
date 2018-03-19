using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine;
using System;

public class VirtualJoystick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler {
    private Image bgImage;
    private Image joystickImage;
    private Vector3 inputVector;

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 pos;
        if(RectTransformUtility.ScreenPointToLocalPointInRectangle(bgImage.rectTransform,eventData.position,eventData.pressEventCamera, out pos))
        {
            //Get tap position
            pos.x = (pos.x / bgImage.rectTransform.sizeDelta.x);
            pos.y = (pos.y / bgImage.rectTransform.sizeDelta.y);

            inputVector = new Vector3(pos.x * 2 ,0, pos.y * 2);
            inputVector = (inputVector.magnitude > 1.0f) ? inputVector.normalized : inputVector;
            //Move Joystick
            joystickImage.rectTransform.anchoredPosition = new Vector3(inputVector.x * (bgImage.rectTransform.sizeDelta.x / 3), inputVector.z * (bgImage.rectTransform.sizeDelta.y / 3));
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        inputVector = Vector3.zero;
        joystickImage.rectTransform.anchoredPosition = Vector3.zero;
    }

    // Use this for initialization
    void Start () {
        bgImage = GetComponent<Image>();
        joystickImage = transform.GetChild(0).GetComponent<Image>();
	}

    public float Horizontal()
    {
        if(inputVector.x != 0)
        {
            return inputVector.x;
        }
        else
        {
            return Input.GetAxis("Horizontal");
        }
    }
    public float Vertical()
    {
        if(inputVector.z != 0)
        {
            return inputVector.z;
        }
        else
        {
            return Input.GetAxis("Vertical");
        }

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
