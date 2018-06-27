using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The remains of a murder victim
/// </summary>
public class Corpse : MonoBehaviour
{
    // corpse identification fields
    CharacterList victimName = CharacterList.Baskerville;   // name of victim
    GameObject currentRoom;                                 // room corpse was left in

    // cause of death support fields
    WeaponTypes wepType;                                // type of weapon used to kill victim and create corpse
    List<string> causesOfDeath = new List<string>();    // collection of possible causes of death
                                                        // Note: contents of list are dependant on type of weapon 
                                                        // used to kill victim and create corpse

    /// <summary>
    /// Grants read and write access to name of victim
    /// </summary>
    public CharacterList VictimName
    {
        get { return victimName; }
        set { victimName = value; }
    }

    /// <summary>
    /// Grants read and write access to room
    /// corpse was left in
    /// </summary>
    public GameObject CurrentRoom
    {
        get { return currentRoom; }
        set { currentRoom = value; }
    }

    /// <summary>
    /// Grants write access to type of weapon used on victim
    /// Alters possible causes of death based on value
    /// </summary>
    public WeaponTypes WeaponTypeUsed
    {
        set
        {
            // sets weapon type to entered value
            wepType = value;

            // loads in possible causes of death
            causesOfDeath.Clear();
            switch (wepType)
            {
                // adds blunt weapon damage
                case WeaponTypes.blunt:
                    causesOfDeath.Add("bruising");
                    causesOfDeath.Add("bone fractures");
                    causesOfDeath.Add("blunt force trauma");
                    break;
                // adds poison weapon damage
                case WeaponTypes.poison:
                    causesOfDeath.Add("bloodshot eyes");
                    causesOfDeath.Add("swollen muscles");
                    causesOfDeath.Add("bluish lips");
                    break;
                // adds slashing weapon damage
                case WeaponTypes.slashing:
                    causesOfDeath.Add("lacerations");
                    causesOfDeath.Add("deep cuts");
                    break;
                // adds stabbing weapon damage
                case WeaponTypes.stabbing:
                    causesOfDeath.Add("penetrating trauma");
                    causesOfDeath.Add("cavitation");
                    causesOfDeath.Add("stab wounds");
                    break;
                // adds strangulation weapon damage
                case WeaponTypes.strangulation:
                    causesOfDeath.Add("neck abrasions");
                    causesOfDeath.Add("ligature marks");
                    causesOfDeath.Add("swollen tongue");
                    break;
                default:
                    break;
            }
        }
    }

    /// <summary>
    /// Returns a random cause of death
    /// </summary>
    public string CauseOfDeath
    {
        get
        {
            int randCOD = Random.Range(0, causesOfDeath.Count);
            return causesOfDeath[randCOD];
        }
    }
}
