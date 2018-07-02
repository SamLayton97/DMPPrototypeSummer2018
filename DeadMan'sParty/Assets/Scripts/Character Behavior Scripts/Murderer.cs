using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script controlling murderer-specific behaviors
/// </summary>
public class Murderer : MonoBehaviour
{
    List<GameObject> killList = new List<GameObject>();

    // murder weapon support fields
    Weapon weapon;                      // current weapon equipped
    bool isActive = false;              // flag determining whether killer can strike tonight
                                        // Note: this flag is lowered if killer takes weapon during current night

    #region Properties

    /// <summary>
    /// Provides get and set access to whether killer is active enough to strike tonight
    /// </summary>
    public bool IsActive
    {
        get { return isActive; }
        set { isActive = value; }
    }

    /// <summary>
    /// Provides get to whether killer has weapon on them
    /// by checking whether their weapon field is populated
    /// </summary>
    public bool IsArmed
    {
        get
        {
            if (weapon != null)
                return true;
            else
                return false;
        }
    }

    /// <summary>
    /// Provides get access to name of killer's weapon
    /// </summary>
    public string WeaponName
    {
        get { return weapon.WeaponName; }
    }

    /// <summary>
    /// Provides get access to type of killer's weapon
    /// </summary>
    public WeaponTypes WeaponType
    {
        get { return weapon.Type; }
    }

    #endregion

    #region Methods

    /// <summary>
    /// Arms murderer with weapon from their current room
    /// </summary>
    public void Arm()
    {
        // retreives current room of killer-character 
        // and number of weapons stored in said room
        Room currRoom = GetComponent<Character>().CurrentRoom.GetComponent<Room>();
        int numOfWeapons = currRoom.NumOfWeapons;

        // if room holds no weapons, break from arming process with printed error
        if (numOfWeapons < 1)
        {
            Debug.Log("Error: Murderer attempting to pull weapon from empty room.");
            return;
        }

        // randomly removes weapon from room 
        // and sets killer's weapon to this
        int wepID = Random.Range(0, numOfWeapons);
        weapon = currRoom.RemoveWeapon(wepID);
    }

    /// <summary>
    /// Disarms killer of their current weapon
    /// Called when they lose weapon after killing victim
    /// </summary>
    void Disarm()
    {
        weapon = null;
    }

    /// <summary>
    /// Murderer determines whether to kill a neighbor
    /// </summary>
    public void DetermineToKill()
    {
        // if murderer is armed
        if (weapon != null)
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
        // otherwise (i.e., killer determines to kill w/ no weapon)
        else
        {
            // Print error to Debug console
            Debug.Log("Killer holds off to find a weapon.");
        }
    }

    /// <summary>
    /// Kills specified neighbor and properly disposes of them
    /// </summary>
    /// <param name="victim">character to be killed by murderer</param>
    void Kill(GameObject victim)
    {
        // kill victim and disarm killer
        victim.GetComponent<Character>().Die(weapon.Type);
        Disarm();
    }

    #endregion

}
