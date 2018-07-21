using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// A lobby to hold characters yet to be roomed
/// </summary>
public class Lobby : Room
{
    // character placement fields
    int lobbyCapacity;                                          // number of characters lobby holds -- equal to # of characters in game
    List<GameObject> lobbyOccupants = new List<GameObject>();   // list of character game objects currently in lobby
    Vector2 occupantLocation = new Vector2();                   // position lobby holds character game objects in

    // capacity display fields
    Text lobbyCapacityText;

    /// <summary>
    /// Returns whether lobby is empty
    /// </summary>
    public bool IsEmpty
    {
        get
        {
            if (lobbyOccupants.Count == 0)
                return true;
            else
                return false;
        }
    }

    /// <summary>
    /// Updates displayed capacity text below room object
    /// </summary>
    void UpdateCapacityText()
    {
        // if no reference to capacity text component
        if (lobbyCapacityText == null)
        {
            // find reference to capacity text
            lobbyCapacityText = GetComponentInChildren<Text>();
        }

        // update text according to current number of characters
        lobbyCapacityText.text = lobbyOccupants.Count.ToString() + " / " + lobbyCapacity.ToString();
    }

    /// <summary>
    /// Override of the base room's Populate() method
    /// Places character in the first open space in the lobby
    /// </summary>
    /// <param name="newOccupant">occupant to be added to lobby</param>
    public override void Populate(GameObject newOccupant)
    {
        // if lobby is not full
        // Note: occupants should never exceed max capacity of lobby
        if (lobbyOccupants.Count <= lobbyCapacity)
        {
            // add new character to list of occupants
            lobbyOccupants.Add(newOccupant);

            // place character at holding spot
            newOccupant.transform.position = occupantLocation;
        }
        // otherwise (i.e., full lobby)
        else
        {
            // print error message
            Debug.Log("Error: Improperly set lobby capacity." +
                " Attempting to add character to full lobby.");
        }
    }

    /// <summary>
    /// Override of the base room's Remove() method
    /// Removes select character from the lobby and re-orders remaining occupants
    /// </summary>
    /// <param name="occupant"></param>
    public override void Remove(GameObject occupant)
    {
        // removes specified character from lobby's occupants list
        lobbyOccupants.Remove(occupant);
    }

    /// <summary>
    /// Called before the Start() method
    /// </summary>
    protected override void Awake()
    {
        // set lobby capacity to equal number of characters in game
        lobbyCapacity = Camera.main.GetComponent<GameManager>().NumOfCharacters;

        // set occupant holding location to be lobby's position
        occupantLocation = transform.position;

        // update capacity text accordingly
        UpdateCapacityText();
    }
}
