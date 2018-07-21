using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Script controlling basic character behaviors
/// Note: Character is invisible bundle of data (game object)
/// held within Room scripts.
/// </summary>
public class Character : MonoBehaviour
{

    #region Fields

    // character sprite fields
    SpriteRenderer spriteRenderer;
    [SerializeField]
    Sprite characterSprite;
    [SerializeField]
    Sprite murdererSprite;

    // character info fields
    CharacterList charName = CharacterList.Baskerville;
    bool isMurderer = false;
    GameObject currentRoom;

    // corpse instantiation fields
    [SerializeField]
    GameObject corpsePrefab;

    #endregion

    #region Properties

    /// <summary>
    /// Provides get access to character's name
    /// </summary>
    public CharacterList CharName
    {
        get { return charName; }
    }

    /// <summary>
    /// Provides get access to character's murderer status
    /// </summary>
    public bool IsMurderer
    {
        get { return isMurderer; }
    }

    /// <summary>
    /// Provides get / set access to character's current room
    /// </summary>
    public GameObject CurrentRoom
    {
        get { return currentRoom; }
        set { currentRoom = value; }
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// Called before Start() method
    /// </summary>
    void Awake()
    {
        // gets sprite renderer component for future use
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Called when character is killed by murderer
    /// Properly removes char from game and creates
    /// corpse with appropriate causes of death
    /// </summary>
    /// <param name="weaponType">weapon type used to kill character</param>
    public void Die(WeaponTypes weaponType)
    {
        // PLACEHOLDER
        // Log victim's death to console
        Debug.Log(charName + " has been killed!");

        // remove character from their current room
        Room currRoomScript = currentRoom.GetComponent<Room>();
        currRoomScript.Remove(gameObject);

        // create corpse with appropriate cause of death
        // and add them to the current room
        Vector2 corpseSpawnLoc = new Vector2(transform.position.x, transform.position.y);
        GameObject newCorpse = Instantiate(corpsePrefab, corpseSpawnLoc, Quaternion.identity);
        newCorpse.GetComponent<Corpse>().WeaponTypeUsed = weaponType;
        newCorpse.GetComponent<Corpse>().VictimName = charName;
        currRoomScript.Populate(newCorpse);

        // destroy victim game object
        Destroy(gameObject);
    }

    /// <summary>
    /// Sets basic data of character upon instantiation
    /// </summary>
    /// <param name="name">character's name</param>
    /// <param name="murdererStatus">whether character is murderer</param>
    public void SetData(CharacterList name, bool murdererStatus)
    {
        charName = name;
        isMurderer = murdererStatus;
    }

    #endregion

}
