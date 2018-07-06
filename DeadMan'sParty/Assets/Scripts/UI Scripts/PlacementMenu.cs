using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Menu controlling placement of characters
/// </summary>
public class PlacementMenu : MonoBehaviour
{
    // movement support
    RoomManager roomManager;

    // character data fields
    GameObject character;
    GameObject currRoom;
    Text charNameText;

    // location support fields
    RectTransform rectTransform;
    Image menuImage;
    Vector2 menuPosition = new Vector2();

    #region Properties

    /// <summary>
    /// Provides public get / set access to character that menu refers to
    /// </summary>
    public GameObject Character
    {
        get { return character; }
        set
        {
            // if game object "value" is tagged as valid type
            if (value.CompareTag("character")
                || value.CompareTag("murderer"))
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
    /// Called before Start() method
    /// </summary>
    void Awake()
    {
        // retreive necessary objects from scene
        charNameText = GameObject.FindGameObjectWithTag("PlacementMenuNameText").GetComponent<Text>();
        roomManager = Camera.main.GetComponent<RoomManager>();
    }

    /// <summary>
    /// Updates text to match reference character
    /// </summary>
    void UpdateText()
    {
        // retreive proper text components of menu
        charNameText = GameObject.FindGameObjectWithTag("PlacementMenuNameText").GetComponent<Text>();

        // update displayed name
        Character charComp = character.GetComponent<Character>();
        charNameText.text = charComp.CharName.ToString();
    }

    #endregion

    #region Button Event Methods

    /// <summary>
    /// Called when player clicks close button
    /// Closes menu
    /// </summary>
    public void CloseMenu()
    {
        Destroy(gameObject);
    }

    /// <summary>
    /// Called when player clicks back button
    /// Returns to general character menu refering to
    /// same character
    /// </summary>
    public void OnClickBackButton()
    {
        // create new "character menu" refering to character
        GameObject newCharMenu = Instantiate((GameObject)Resources.Load("CharacterMenu"));
        newCharMenu.GetComponent<CharacterMenu>().Character = character;

        // close existing placement menu
        Destroy(gameObject);
    }

    /// <summary>
    /// Called when player clicks lobby button
    /// </summary>
    public void OnClickLobbyButton()
    {
        // send current character to lobby
        if (character != null)
            roomManager.PushToRoom(character, 0);
        else
            Debug.Log("No character selected.");
    }

    /// <summary>
    /// Called when player clicks room 1 button
    /// </summary>
    public void OnClickRoom1Button()
    {
        // send character to room 1
        if (character != null)
            roomManager.PushToRoom(character, 1);
        else
            Debug.Log("No character selected.");
    }

    /// <summary>
    /// Called when player clicks room 2 button
    /// </summary>
    public void OnClickRoom2Button()
    {
        // send character to room 2
        if (character != null)
            roomManager.PushToRoom(character, 2);
        else
            Debug.Log("No character selected.");
    }

    /// <summary>
    /// Called when player clicks room 3 button
    /// </summary>
    public void OnClickRoom3Button()
    {
        // send character to room 3
        if (character != null)
            roomManager.PushToRoom(character, 3);
        else
            Debug.Log("No character selected.");
    }

    /// <summary>
    /// Called when player clicks room 4 button
    /// </summary>
    public void OnClickRoom4Button()
    {
        // send character to room 4
        if (character != null)
            roomManager.PushToRoom(character, 4);
        else
            Debug.Log("No character selected.");
    }

    /// <summary>
    /// Called when player clicks room 5 button
    /// </summary>
    public void OnClickRoom5Button()
    {
        // send character to room 5
        if (character != null)
            roomManager.PushToRoom(character, 5);
        else
            Debug.Log("No character selected.");
    }

    public void OnClickRoom6Button()
    {
        // send character to room 6
        if (character != null)
            roomManager.PushToRoom(character, 6);
        else
            Debug.Log("No character selected.");
    }

    /// <summary>
    /// Called when player clicks execute button
    /// </summary>
    public void OnClickExecuteButton()
    {
        // send character to execution room
        if (character != null)
            roomManager.PushToRoom(character, -1);
        else
            Debug.Log("No character selected.");
    }

    #endregion

}
