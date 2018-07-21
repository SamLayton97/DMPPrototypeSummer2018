using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Room behavior which displays mouse over elements pertaining to this room
/// </summary>
public class RoomUI : MonoBehaviour
{
    // name display support fields
    string roomName;
    Text nameText;

    // capacity display support
    Text capacityText;          // text displaying current # of characters in room
    int currOccupants;          // current number of characters stored in room
    int maxOccupancy;           // maximum number of characters stored in room
    Room roomScript;            // reference to Room's (game object) room script

	// Use this for initialization
	void Start ()
    {
        // finds room name pop-up UI and saves its text component
        nameText = GameObject.FindGameObjectWithTag("nametag").GetComponent<Text>();

        // if room is tagged as standard room, set name to unique room name
        if (CompareTag("room"))
            roomName = GetComponent<Room>().RoomName;
        // if room is tagged as lobby, set name to "lobby"
        else if (CompareTag("lobby"))
            roomName = "Lobby";
        // if room is tagged as execution room, set name to "kill shack" -- placeholder name
        else if (CompareTag("executionRoom"))
            roomName = "Kill Shack";
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
