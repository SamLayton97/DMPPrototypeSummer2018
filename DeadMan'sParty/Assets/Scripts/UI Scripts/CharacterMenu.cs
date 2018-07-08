using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// A menu which allows players to move and question living characters
/// Opens when players click on a character object in scene
/// </summary>
public class CharacterMenu : MonoBehaviour
{
    // gets GameManager Script
    GameManager gameManager;

    // character data fields
    GameObject character;       // character game object which menu refers to

    // text menu-component fields
    Text charNameText;
    Text aliveStatusText;

    // placement menu support fields
    GameObject placementMenu;           // reference to proper placement menu to spawn
    RoomManager roomManager;            // reference to room manager - used to find number of std rooms in scene

    #region Properties

    /// <summary>
    /// Provides read / write access to character menu refers to
    /// Note: For menu to update, value must be tagged as either a
    /// character, murderer, or corpse.
    /// </summary>
    public GameObject Character
    {
        get { return character; }
        set
        {
            // if game object "value" is tagged as valid type
            if (value.CompareTag("character")
                || value.CompareTag("murderer")
                || value.CompareTag("corpse"))
            {
                // set field to value and update text
                character = value;
                UpdateText();
            }
            // otherwise (i.e., invalid "value" input),
            else
            {
                Debug.Log("Error: Attempting to set character menu to refer to invalid game object.");
            }
        }
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// Called before the Start() method
    /// </summary>
    void Awake()
    {
        // gets GameManager component
        gameManager = Camera.main.GetComponent<GameManager>();


        // retrieve proper text components of menu
        charNameText = GameObject.FindGameObjectWithTag("CharacterMenuNameText").GetComponent<Text>();
        aliveStatusText = GameObject.FindGameObjectWithTag("CharacterMenuAliveStatusText").GetComponent<Text>();

        // retrieve number of rooms within scene
        roomManager = Camera.main.GetComponent<RoomManager>();
        int numOfRooms = roomManager.NumOfRooms;

        // retrieve reference to proper placement menu to spawn
        switch (numOfRooms)
        {
            case 2:
                placementMenu = (GameObject)Resources.Load("PlacementMenu2Room");
                break;
            case 3:
                placementMenu = (GameObject)Resources.Load("PlacementMenu3Room");
                break;
            case 4:
                placementMenu = (GameObject)Resources.Load("PlacementMenu4Room");
                break;
            case 5:
                placementMenu = (GameObject)Resources.Load("PlacementMenu5Room");
                break;
            case 6:
                placementMenu = (GameObject)Resources.Load("PlacementMenu6Room");
                break;
            default:
                // print error message to Debug log
                Debug.Log("Error: Unable to load placement menu. Invalid number of standard rooms in scene.");
                break;
        }
    }

    /// <summary>
    /// Called once per frame
    /// </summary>
    void Update()
    {
        // update text if menu refers to character
        if (character != null)
            UpdateText();
    }

    /// <summary>
    /// Updates text to match reference character
    /// </summary>
    void UpdateText()
    {
        // retreive proper text components of menu
        charNameText = GameObject.FindGameObjectWithTag("CharacterMenuNameText").GetComponent<Text>();
        aliveStatusText = GameObject.FindGameObjectWithTag("CharacterMenuAliveStatusText").GetComponent<Text>();

        // if game object is tagged as living character
        if (character.CompareTag("character")
            || character.CompareTag("murderer"))
        {
            // update displayed name
            Character charComp = character.GetComponent<Character>();
            charNameText.text = charComp.CharName.ToString();

            // update living status to alive
            aliveStatusText.text = "Alive";
            aliveStatusText.color = Color.green;
        }
        // if game object is tagged as corpse
        else if (character.CompareTag("corpse"))
        {
            // update displayed name
            Corpse corpseComp = character.GetComponent<Corpse>();
            charNameText.text = corpseComp.VictimName.ToString();

            // update living status to dead
            aliveStatusText.text = "Dead";
            aliveStatusText.color = Color.red;
        }
    }

    #endregion

    #region Button Event Methods

    /// <summary>
    /// Called when player clicks on "Move" button
    /// Closes current menu and opens movement-specific menu
    /// </summary>
    public void OnClickMoveButton()
    {
        if (character != null)
        {
            // create placement menu refering to character-reference
            GameObject newPlacMenu = Instantiate(placementMenu);
            newPlacMenu.GetComponent<PlacementMenu>().Character = character;

            // Destroy open character menu
            Destroy(gameObject);
        }
        else
            Debug.Log("Error: No character selected.");
    }

    /// <summary>
    /// Called when player clicks "Interrogate" button
    /// Placeholder: Displays feedback to the debug log
    /// </summary>
    public void OnClickInterrogateButton()
    {
        if (character != null && gameManager.CurrentAP > 0)
        {
            Debug.Log("Player performs an interrogation on " + charNameText.text +
            ". Full functionality yet to be implemented.");

            // uses AP via GameManager UseAP Method
            gameManager.UseActionPoints();
        }
        else
            Debug.Log("Error: No character selected or not enough AP");
    }

    /// <summary>
    /// Called when player clicks "Dispose" button
    /// Destroys corpse and removes them from their current room
    /// Note: Functionality unique to the Corpse variant of this menu
    /// </summary>
    public void OnClickDisposeButton()
    {
        // disposes of corpse-character and destroys menu
        Corpse victim = character.GetComponent<Corpse>();
        victim.Dispose();
        Destroy(gameObject);
    }

    /// <summary>
    /// Called when player clicks "Autopsy" button
    /// Placeholder: Displays feedback to Debug log
    /// Note: Functionality unique to the Corpse variant of this menu
    /// </summary>
    public void OnClickAutopsyButton()
    {
        if (gameManager.CurrentAP > 0)
        {
            Debug.Log("Player performs an autopsy on " + charNameText.text +
            ". Full functionality yet to be implemented.");
            // uses AP via GameManager UseAP Method
            gameManager.UseActionPoints();
        }
        else
        {
            Debug.Log("Not enough AP");
        }
    }

    /// <summary>
    /// Called when player clicks "X" button or when another menu is opened
    /// Removes menu from the UI
    /// </summary>
    public void CloseMenu()
    {
        Destroy(gameObject);
    }

    #endregion

}
