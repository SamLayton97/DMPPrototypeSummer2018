using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Room behavior which displays UI elements pertaining to said room
/// </summary>
public class RoomUI : MonoBehaviour
{
    // name display support fields
    string roomName;
    Text nameText;

	// Use this for initialization
	void Start ()
    {
        // finds room name pop-up UI and saves its text component
        nameText = GameObject.FindGameObjectWithTag("nametag").GetComponent<Text>();

        // save name of this character
        roomName = GetComponent<Room>().RoomName;
	}

    /// <summary>
    /// Called when player mouses over room's collider
    /// </summary>
    void OnMouseOver()
    {
        // set text to display room name
        nameText.text = roomName;
        nameText.color = Color.black;
    }

    /// <summary>
    /// Called when player's mouse leaves room's collider
    /// </summary>
    void OnMouseExit()
    {
        // set room name text to be invisible
        nameText.color = Color.clear;
    }
}
