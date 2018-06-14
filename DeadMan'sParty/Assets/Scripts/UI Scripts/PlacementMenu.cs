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
    GameManager gameManager;

    // character data fields
    GameObject character;
    GameObject currRoom;
    Text charNameText;

    // location support fields
    RectTransform rectTransform;
    Image menuImage;
    Vector2 menuPosition = new Vector2();

    /// <summary>
    /// Provides public get / set access to character that menu refers to
    /// </summary>
    public GameObject Character
    {
        get { return character; }
        set
        {
            character = value;

            // if character isn't null
            if (character != null)
            {
                // update displayed name
                Character charComp = character.GetComponent<Character>();
                charNameText.text = charComp.CharName.ToString();
            }
            // otherwise (i.e., value is null)
            else
                // set name to safe default
                charNameText.text = "null";
        }
    }

    /// <summary>
    /// Called before Start() method
    /// </summary>
    void Awake()
    {
        // retreive necessary objects from scene
        charNameText = GameObject.FindGameObjectWithTag("menuCharacterName").GetComponent<Text>();
        gameManager = Camera.main.GetComponent<GameManager>();

        Character = GameObject.FindGameObjectWithTag("CharacterMenu").GetComponent<CharacterMenu>().Character;
    }

    #region Button Event Methods

    /// <summary>
    /// Called when player clicks close button
    /// </summary>
    public void OnClickCloseButton()
    {
        //// set character reference to null
        //if (character != null)
        //    Character = null;
        //else
        //    Debug.Log("No character selected.");

        Destroy(gameObject);
    }

    /// <summary>
    /// Called when player clicks lobby button
    /// </summary>
    public void OnClickLobbyButton()
    {
        // send current character to lobby
        if (character != null)
            gameManager.PushToRoom(character, 0);
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
            gameManager.PushToRoom(character, 1);
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
            gameManager.PushToRoom(character, 2);
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
            gameManager.PushToRoom(character, 3);
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
            gameManager.PushToRoom(character, 4);
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
            gameManager.PushToRoom(character, -1);
        else
            Debug.Log("No character selected.");
    }

    #endregion

}
