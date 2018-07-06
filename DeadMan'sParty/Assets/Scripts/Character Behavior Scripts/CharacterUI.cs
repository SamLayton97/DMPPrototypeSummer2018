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
    void Start()
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
        // find existing character and placement menus in scene
        GameObject[] existingCharMenus = GameObject.FindGameObjectsWithTag("CharacterMenu");
        GameObject[] existingPlacMenus = GameObject.FindGameObjectsWithTag("PlacementMenu");

        // add existing menus to list
        List<GameObject> existingMenus = new List<GameObject>();
        for (int i = 0; i < existingCharMenus.Length; i++)
        {
            existingMenus.Add(existingCharMenus[i]);
        }
        for (int i = 0; i < existingPlacMenus.Length; i++)
        {
            existingMenus.Add(existingPlacMenus[i]);
        }

        // close any remaining menus
        foreach (GameObject openMenu in existingMenus)
        {
            Destroy(openMenu);
        }

        // create a new character menu referencing this game object
        // Note: creates corpse variant if character is tagged as such
        if (gameObject.CompareTag("character") || gameObject.CompareTag("murderer"))
        {
            GameObject newCharMenu = Instantiate((GameObject)Resources.Load("CharacterMenu"));
            newCharMenu.GetComponent<CharacterMenu>().Character = gameObject;
        }
        else if (gameObject.CompareTag("corpse"))
        {
            GameObject newCharMenu = Instantiate((GameObject)Resources.Load("CorpseMenu"));
            newCharMenu.GetComponent<CharacterMenu>().Character = gameObject;
        }
    }
}
