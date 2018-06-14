using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterMenu : MonoBehaviour
{
    // placement menu assets
    [SerializeField]
    GameObject prefabPlacementMenu;

    // character data fields
    GameObject character;
    GameObject currRoom;
    Text charNameText;

    private void Awake()
    {
        // retreive necessary objects from scene
        charNameText = GameObject.FindGameObjectWithTag("menuCharacterName").GetComponent<Text>();
    }

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
    /// What happens when you click the placement button
    /// </summary>
    public void OnClickPlacementButton()
    {
        Instantiate(prefabPlacementMenu, new Vector3(-350, 0, 0), Quaternion.identity);
        Destroy(gameObject);
    }

    /// <summary>
    /// What happens when you click the close button
    /// </summary>
    public void OnClickCloseButton()
    {
        Destroy(gameObject);
    }
}
