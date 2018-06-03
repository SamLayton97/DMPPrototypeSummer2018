using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// An room to house characters player suspects to be murderer
/// </summary>
public class ExecutionRoom : Room
{
    // character placement fields
    bool occupied = false;
    GameObject charToBeExecuted;

    /// <summary>
    /// Provides get access to whether chamber is occupied
    /// </summary>
    public bool IsOccupied
    {
        get { return occupied; }
    }

    /// <summary>
    /// Override of the base room's populate method
    /// Houses up to a single character in the execution room
    /// </summary>
    /// <param name="newOccupant">character to be placed in room</param>
    public override void Populate(GameObject newOccupant)
    {
        // if room is not currently occupied
        if (!occupied)
        {
            // save new occupant as character to be executed
            charToBeExecuted = newOccupant;
            occupied = true;

            // place new occupant at center of the room
            newOccupant.transform.position = gameObject.transform.position;
        }
        // otherwise (i.e., occupied room)
        else
        {
            // print error message to console
            Debug.Log("Error: Attempting to place a character in an occupied execution room.");
        }
    }

    /// <summary>
    /// Removes sole occupant from execution room
    /// and sets occupied flag to false
    /// </summary>
    public void Clear()
    {
        charToBeExecuted = null;
        occupied = false;
    }

    /// <summary>
    /// Executes occupant
    /// </summary>
    /// <returns>Returns true if innocent character is killed</returns>
    public bool ExecuteOccupant()
    {
        // if character isn't tagged as murderer
        if (!charToBeExecuted.CompareTag("murderer"))
        {
            // execute innocent character and return true
            Destroy(charToBeExecuted);
            return true;
        }
        // otherwise (i.e., character is murderer)
        else
        {
            // execute murderer and return false
            Destroy(charToBeExecuted);
            return false;
        }
    }
}
