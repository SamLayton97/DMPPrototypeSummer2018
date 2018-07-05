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
    // character data fields
    GameObject character;       // character game object which menu refers to

    // text menu-component fields
    Text charNameText;
    Text aliveStatusText;

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
        // retrieve proper text components of menu
        charNameText = GameObject.FindGameObjectWithTag("CharacterMenuNameText").GetComponent<Text>();
        aliveStatusText = GameObject.FindGameObjectWithTag("CharacterMenuAliveStatusText").GetComponent<Text>();
    }

    /// <summary>
    /// Called once per frame
    /// </summary>
    void Update()
    {
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
        if (character != null)
        {
            Debug.Log("Player performs an interrogation on " + charNameText.text +
            " Full functionality yet to be implemented.");
        }
        else
            Debug.Log("Error: No character selected.");
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
