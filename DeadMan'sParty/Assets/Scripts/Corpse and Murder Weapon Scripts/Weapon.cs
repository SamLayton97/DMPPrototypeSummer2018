using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A weapon used by a murderer
/// Note: This IS NOT instantiated as a game object.
/// Rather, it should be created and stored within room
/// and murderer scripts.
/// </summary>
public class Weapon
{
    string weaponName;                                      // unique name of the weapon
    WeaponTypes type;                                       // universal type of weapon

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
    }

    /// <summary>
    /// Returns name of weapon
    /// </summary>
    public string WeaponName
    {
        get { return weaponName; }
    }

    /// <summary>
    /// Returns weapon's type
    /// </summary>
    public WeaponTypes Type
    {
        get { return type; }
    }
}
