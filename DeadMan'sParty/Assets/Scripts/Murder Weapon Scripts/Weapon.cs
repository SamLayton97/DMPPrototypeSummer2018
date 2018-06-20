using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A weapon used by a murderer
/// Note: This IS NOT instantiated as a game object.
/// Rather, it should be created and stored within room
/// and murderer scripts.
/// </summary>
public class Weapon : MonoBehaviour
{
    string weaponName;                                      // unique name of the weapon
    WeaponTypes type;                                       // universal type of weapon
    List<string> causesOfDeath = new List<string>();        // list of damages weapon can do

    /// <summary>
    /// Constructor for the weapon class
    /// </summary>
    /// <param name="wepName">name of new weapon</param>
    /// <param name="wepType">universal type of new weapon</param>
    public Weapon (string wepName, WeaponTypes wepType)
    {
        // sets name and type of weapon
        weaponName = wepName;
        type = wepType;

        // sets potential causes of death according to weapon's type
        switch (type)
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

    /// <summary>
    /// Returns name of weapon
    /// </summary>
    public string WeaponName
    {
        get { return weaponName; }
    }

    /// <summary>
    /// Returns a cause of death from murder weapon
    /// </summary>
    public string CauseOfDeath
    {
        get
        {
            // return random cause of death from weapon
            int randCOD = Random.Range(0, causesOfDeath.Count);
            return causesOfDeath[randCOD];
        }
    }

}
