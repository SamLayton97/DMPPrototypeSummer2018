using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A lobby to hold characters yet to be roomed
/// </summary>
public class Lobby : Room
{
    // character placement fields
    const int lobbyCapacity = 15;
    List<GameObject> lobbyOccupants = new List<GameObject>();
    Vector2[] charLocations = new Vector2[lobbyCapacity];
    BoxCollider2D boxCollider2D;

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

            // place character at the next open location in lobby
            Vector2 firstFreeLoc = charLocations[lobbyOccupants.Count - 1];
            newOccupant.transform.position = firstFreeLoc;
        }
        // otherwise (i.e., full lobby)
        else
        {
            // print error message
            Debug.Log("Error: Attempting to add character to full lobby.");
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

        // reposition remaining occupants to fill any positional gaps
        for (int i = 0; i < lobbyOccupants.Count; i++)
        {
            lobbyOccupants[i].transform.position = charLocations[i];
        }
    }

    /// <summary>
    /// Called before the Start() method
    /// </summary>
    protected override void Awake()
    {
        // create temp character to save its dimentions and then destroy it
        GameObject tempChar = Instantiate(characterPrefab);
        characterRadius = tempChar.GetComponent<CircleCollider2D>().radius;
        Destroy(tempChar);

        // calculate initial coordinates to place characters
        boxCollider2D = GetComponent<BoxCollider2D>();
        float initialXLoc = transform.position.x - (boxCollider2D.size.x / 2) + (characterRadius * 2.5f);
        float initialYLoc = transform.position.y;

        // save character placement coordinates according to saved dimensions
        for (int i = 0; i < lobbyCapacity; i++)
        {
            charLocations[i] = new Vector2(initialXLoc + (characterRadius * i * 2), 
                initialYLoc);
        }
    }
}
