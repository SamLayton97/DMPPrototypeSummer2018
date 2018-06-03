using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Character behavior which displays UI elements pertaining to said character
/// </summary>
public class CharacterUI : MonoBehaviour
{
    // name display support fields
    CharacterList characterName;
    Text nameText;

	// Use this for initialization
	void Start ()
    {
        // finds name pop-up UI object and saves its text component
        nameText = GameObject.FindGameObjectWithTag("nametag").GetComponent<Text>();

        // saves name of this character
        characterName = GetComponent<Character>().CharName;
	}

    /// <summary>
    /// When player mouses over character
    /// </summary>
    void OnMouseEnter()
    {
        // set text to display character's name
        nameText.text = characterName.ToString();
        nameText.color = Color.black;
    }

    /// <summary>
    /// When player's mouse leaves character
    /// </summary>
    void OnMouseExit()
    {
        // sets name text to be invisible
        nameText.color = Color.clear;
    }

    /// <summary>
    /// Called when player clicks on character object
    /// </summary>
    void OnMouseDown()
    {
        // find character placement menu in scene
        // and set menu to refer to this character game object
        GameObject placementMenu = GameObject.FindGameObjectWithTag("PlacementMenu");
        placementMenu.GetComponent<PlacementMenu>().Character = gameObject;
    }
}
