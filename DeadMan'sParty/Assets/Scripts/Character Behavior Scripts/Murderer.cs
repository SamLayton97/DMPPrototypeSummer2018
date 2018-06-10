using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script controlling murderer-specific behaviors
/// </summary>
public class Murderer : MonoBehaviour
{
    List<GameObject> killList = new List<GameObject>();

    /// <summary>
    /// Murderer determines whether to kill a neighbor
    /// </summary>
    public void DetermineToKill()
    {
        // generate kill list from occupants of current room
        Room currRoom = GetComponent<Character>().CurrentRoom.GetComponent<Room>();
        foreach (GameObject occupant in currRoom.occupants)
        {
            // if occupant isn't tagged as a murderer
            // Note: this excludes self and other killers
            if (!occupant.CompareTag("murderer"))
            {
                // add them to kill list
                killList.Add(occupant);
            }
        }

        // if kill list contains multiple characters
        if (killList.Count > 1)
        {
            // kill random character from list
            int victimID = Random.Range(0, killList.Count - 1);
            Kill(killList[victimID]);
        }
        // otherwise (i.e., single character in list)
        else
        {
            // determine to hold off for the night
            // Note: placeholder for first iteration
            Debug.Log("Murderer holds off for the night...");
        }

        // clear kill list for next night
        killList.Clear();
    }

    /// <summary>
    /// Kills specified neighbor and properly disposes of them
    /// </summary>
    /// <param name="victim">character to be killed by murderer</param>
    void Kill(GameObject victim)
    {
        // save victim's character component and current room
        Character victimScript = victim.GetComponent<Character>();
        Room currRoom = victimScript.CurrentRoom.GetComponent<Room>();

        // log victim's murder to console
        // Note: placeholder for first iteration
        Debug.Log(victimScript.CharName + " has been killed!");

        // remove character from current room
        currRoom.Remove(victim);

        // destroy victim
        Destroy(victim);
    }
}
