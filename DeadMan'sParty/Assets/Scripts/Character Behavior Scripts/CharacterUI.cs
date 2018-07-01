using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Character behavior which displays UI elements pertaining to said character
/// </summary>
public class CharacterUI : MonoBehaviour
{
    // character menu assets
    [SerializeField]
    GameObject prefabCharacterMenu;

    // name display support fields
    CharacterList characterName;
    Text nameText;

	// Use this for initialization
	void Start ()
    {
        // finds name pop-up UI object and saves its text component
        nameText = GameObject.FindGameObjectWithTag("nametag").GetComponent<Text>();

        // saves name of this character (pulled from appropriate component)
        if (!CompareTag("corpse"))
            characterName = GetComponent<Character>().CharName;
        else
            characterName = GetComponent<Corpse>().VictimName;
	}

    /// <summary>
    /// When player mouses over character
    /// </summary>
    void OnMouseEnter()
    {
        // set text to display character's name
        nameText.text = characterName.ToString();

        // sets color of text appropriate to whether character is alive
        if (!CompareTag("corpse"))
            nameText.color = Color.black;
        else
            nameText.color = Color.red;
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
        //// find character placement menu in scene
        //// and set menu to refer to this character game object
        //GameObject placementMenu = GameObject.FindGameObjectWithTag("PlacementMenu");
        //placementMenu.GetComponent<PlacementMenu>().Character = gameObject;
        GameObject characterMenu;

        // create the character menu
        if (GameObject.FindGameObjectsWithTag("CharacterMenu").Length == 0)
        {
            characterMenu = Instantiate(prefabCharacterMenu, new Vector3(-350, 0, 0), Quaternion.identity);

            // find the character menu in scene
            // and set the menu to refer to this character
            characterMenu.GetComponent<CharacterMenu>().Character = gameObject;
        }
        else
        {
            characterMenu = GameObject.FindGameObjectWithTag("CharacterMenu");
            Destroy(characterMenu);
            Instantiate(prefabCharacterMenu, new Vector3(-350, 0, 0), Quaternion.identity);

            // find the character menu in scene
            // and set the menu to refer to this character
            characterMenu = GameObject.FindGameObjectWithTag("CharacterMenu");
            characterMenu.GetComponent<CharacterMenu>().Character = gameObject;
        }
    }
}
