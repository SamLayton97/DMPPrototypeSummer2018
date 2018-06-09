using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Script controlling basic character behaviors
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
    /// Sets basic data of character upon instantiation
    /// </summary>
    /// <param name="name">character's name</param>
    /// <param name="murdererStatus">whether character is murderer</param>
    public void SetData(CharacterList name, bool murdererStatus)
    {
        charName = name;
        isMurderer = murdererStatus;

        // changes character's sprite accordingly
        // Note: Used only for debugging purposes
        if (isMurderer)
            spriteRenderer.sprite = murdererSprite;
        else
            spriteRenderer.sprite = characterSprite;
    }

    #endregion

}
