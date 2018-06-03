using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// A character name pop-up which follows the mouse
/// </summary>
public class NamePopUp : MonoBehaviour
{
    Vector2 currPosition = new Vector2();   // current position of the name text
    RectTransform rectTransform;            // rect transform component of this text object
    Vector2 offset = new Vector2();         // relative position offset of text in relation to mouse

	// Use this for initialization
	void Start ()
    {
        // sets text offset according to text's size
        rectTransform = GetComponent<RectTransform>();
        offset.x = rectTransform.sizeDelta.x / 3;
        offset.y = rectTransform.sizeDelta.y / 3;
	}
	
	// Update is called once per frame
	void Update ()
    {
        // follow the mouse position
        currPosition = transform.position;
        currPosition.x = Input.mousePosition.x + offset.x;
        currPosition.y = Input.mousePosition.y + offset.y;
        transform.position = currPosition;
	}
}
