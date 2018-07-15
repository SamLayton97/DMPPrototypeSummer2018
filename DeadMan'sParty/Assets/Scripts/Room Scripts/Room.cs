using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// A room to hold characters for the night
/// </summary>
public class Room : MonoBehaviour
{

    #region Fields

    // capacity support fields
    [SerializeField]
    int maxOccupancy = 4;                                            // max num of characters to fit in room
    public List<GameObject> occupants = new List<GameObject>();      // list of characters currently occupying room

    // character space fields
    [SerializeField]
    protected GameObject characterPrefab;                   // a prefab of the character game object
    protected float characterRadius;                        // radius of character's circle colliders
    Vector2[] occupantLocs;                                 // an array of occupant locations

    // room-unique weapon creation fields
    // Note: for proper creation of weapons, these arrays must align in inspector
    [SerializeField]
    string[] wepNames;          // serialized array of weapon names
    [SerializeField]
    WeaponTypes[] wepTypes;     // serialized array of weapon types

    // weapon storage fields
    List<Weapon> initWepList = new List<Weapon>();       // initial list of weapons stored in room
    List<Weapon> currWepList = new List<Weapon>();       // current list of weapons stored in room
                                                         // Note: Killers remove weapons from latter list. Former is for comparisons.

    // Room interaction fields
    public float distance = 1f;
    public GameObject box;
    bool drawInteractionOption = false;
    bool drawMenu = false;
    Text rmNameTextBox;
    Text wpNameTextBox;
    Text wpNameTextBox1;
    Text wpNameTextBox2;
    string weaponList;
    string newWepList;

    public GameObject menuUI;
    public static bool MenuPopUp = false;

    // room identification fields
    [SerializeField]
    string roomName;
    int roomNumber = 0;
    Text roomNameText;

    // room sprites
    // Note: Used for an older version that identified rooms by number
    SpriteRenderer spriteRenderer;
    [SerializeField]
    Sprite room1Sprite;
    [SerializeField]
    Sprite room2Sprite;
    [SerializeField]
    Sprite room3Sprite;
    [SerializeField]
    Sprite room4Sprite;
    [SerializeField]
    Sprite room5Sprite;
    [SerializeField]
    Sprite room6Sprite;

    #endregion

    #region Properties

    /// <summary>
    /// Returns number of weapons currently stored in room
    /// </summary>
    public int NumOfWeapons
    {
        get { return currWepList.Count; }
    }

    /// <summary>
    /// Returns max number of characters able to fit in room
    /// </summary>
    public int MaxOccupancy
    {
        get { return maxOccupancy; }
    }

    public string GetRoomName
    {
        get { return roomName; }
    }
    /// <summary>
    /// Returns whether room is at max capacity
    /// </summary>
    public bool IsFull
    {
        get
        {
            if (occupants.Count > (maxOccupancy - 1))
                return true;
            else
                return false;
        }
    }

    /// <summary>
    /// Returns number of characters placed in room
    /// </summary>
    public int Count
    {
        get { return occupants.Count; }
    }

    /// <summary>
    /// Provides set access to character dimensions
    /// </summary>
    public float CharacterRadius
    {
        set
        {
            // sets field
            characterRadius = value;
        }
    }

    /// <summary>
    /// Provides get and set access to room number
    /// Changes room's sprite accordingly
    /// </summary>
    public int RoomNumber
    {
        get { return roomNumber; }
        set
        {
            // if value is a valid room number (1-4)
            if (value < 5 && value > 0)
            {
                roomNumber = value;

                // changes room sprite accordingly
                spriteRenderer = GetComponent<SpriteRenderer>();
                switch (value)
                {
                    case 1:
                        spriteRenderer.sprite = room1Sprite;
                        break;
                    case 2:
                        spriteRenderer.sprite = room2Sprite;
                        break;
                    case 3:
                        spriteRenderer.sprite = room3Sprite;
                        break;
                    case 4:
                        spriteRenderer.sprite = room4Sprite;
                        break;
                    case 5:
                        spriteRenderer.sprite = room5Sprite;
                        break;
                    case 6:
                        spriteRenderer.sprite = room6Sprite;
                        break;
                    default:
                        spriteRenderer.sprite = room1Sprite;
                        break;
                }
            }
        }
    }

    /// <summary>
    /// Provides get access to room's (i.e., building's) name / type
    /// </summary>
    public string RoomName
    {
        get { return roomName; }
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Removes and returns a weapon from the current weapon list
    /// </summary>
    /// <param name="weaponID">location of desired weapon</param>
    /// <returns>Weapon removed from list</returns>
    public Weapon RemoveWeapon(int weaponID)
    {
        // saves copy of weapon and pops original from list
        Weapon removedWeapon = currWepList[weaponID];
        currWepList.RemoveAt(weaponID);

        // return removed weapon
        return removedWeapon;
    }

    /// <summary>
    /// Populates room with a character / corpse from lobby
    /// </summary>
    /// <param name="character">character / corpse to be added to room</param>
    public virtual void Populate(GameObject newOccupant)
    {
        // if there is space in the room
        if (occupants.Count < maxOccupancy)
        {
            // add new occupant to list of occupants
            occupants.Add(newOccupant);

            // set new occupant's current room to this
            // Note: this is dependant on the occupant's type
            if (!newOccupant.CompareTag("corpse"))
                newOccupant.GetComponent<Character>().CurrentRoom = gameObject;
            else
                newOccupant.GetComponent<Corpse>().CurrentRoom = gameObject;

            // move character to first free space in room
            Vector2 firstFreeLoc = occupantLocs[occupants.Count - 1];
            newOccupant.transform.position = firstFreeLoc;
        }
        // otherwise (i.e., full room)
        else
        {
            // print error message
            Debug.Log("Error: Attempting to add character to full room.");
        }
    }

    /// <summary>
    /// Removes character from the room and returns them to the lobby
    /// </summary>
    /// <param name="occupant">character to return to lobby</param>
    public virtual void Remove(GameObject occupant)
    {
        // remove character from list of room's occupants
        occupants.Remove(occupant);

        // re-order remaining occupants to fill any positional gaps
        for (int i = 0; i < occupants.Count; i++)
        {
            occupants[i].transform.position = occupantLocs[i];
        }
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// Called before the Start() method
    /// </summary>
    protected virtual void Awake()
    {
        // creates memory for occupant locations array that rounds up to nearest even number
        if (maxOccupancy % 2 != 0)
            occupantLocs = new Vector2[maxOccupancy + 1];
        else
            occupantLocs = new Vector2[maxOccupancy];

        // create temp character to save its dimentions and then destroy it
        GameObject tempChar = Instantiate(characterPrefab);
        characterRadius = tempChar.GetComponent<CircleCollider2D>().radius;
        Destroy(tempChar);

        // calculate initial coordinates to place occupants
        BoxCollider2D boxCollider2D = GetComponent<BoxCollider2D>();
        float initialXLoc = transform.position.x - (characterRadius * ((float)maxOccupancy / 4f));
        float initialYLoc = transform.position.y + characterRadius;

        // calculate number of characters per row
        int charsPerRow = (maxOccupancy / 2) + (maxOccupancy % 2);

        // store occupant placement locations according to saved dimensions
        // splits room into two rows
        for (int i = 0; i < 2; i++)
        {
            // fills row
            for (int j = 0; j < charsPerRow; j++)
            {
                occupantLocs[(charsPerRow * i) + j] = new Vector2(initialXLoc + (j * characterRadius * 2),
                    initialYLoc - (i * characterRadius * 2));
            }
        }
    }

    // Use this for initialization
    void Start ()
    {
        // Destroy room's box collider
        // Note: this is done so as not to interfere with characters' colliders in the
        // OnMouseEnter() method
        Destroy(GetComponent<BoxCollider2D>());

        // create set of weapons according to serialized fields
        for (int i = 0; i < wepNames.Length; i++)
        {
            if (wepNames[i] != null)
            {
                Weapon wepToAdd = new Weapon(wepNames[i], wepTypes[i]);
                initWepList.Add(wepToAdd);
            }
        }

        // copy initial list to current list
        foreach (Weapon weapon in initWepList)
        {
            currWepList.Add(weapon);
        }

        // Sets text below room to display its name
        // Note: action performed only for standard rooms
        if (CompareTag("room") && roomName != null)
        {
            roomNameText = GetComponentInChildren<Text>();
            roomNameText.text = roomName;
        }
    }

    // Update updates based on local framerate, called once per frame
    void Update()
    {
        // defines boxmax on layer 10 (i.e. player)
        int boxMask = 1 << 10;

        // Create and store hit raycast
        Physics2D.queriesStartInColliders = false;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.left * transform.localScale.x * 1.5f, distance, boxMask);

        // Interaction check
        if (hit.collider != null)
        {
                drawInteractionOption = true;
                box = hit.collider.gameObject;
        }
        else
        {
            drawInteractionOption = false;
            drawMenu = false;
        }

        // checks if the collider and ray is being hit and if e is being pressed
        if (hit.collider != null && Input.GetKey(KeyCode.E))
        {
            MenuPopUp = true;
            if (MenuPopUp)
            {
                PopUp();
                box = hit.collider.gameObject;
                drawMenu = true;
                rmNameTextBox = GameObject.FindGameObjectWithTag("RoomNameText").GetComponent<Text>();
                rmNameTextBox.text = "Location: " + roomName;

                wpNameTextBox = GameObject.FindGameObjectWithTag("WeaponNameText").GetComponent<Text>();

                for (int i = 0; i < currWepList.Count; i++)
                {
                    weaponList += currWepList[i].WeaponName + "\n";
                    newWepList = weaponList;
                }
                wpNameTextBox.text = newWepList;
            }
            else
            {
                PopDown();
            }
        }
    }

    // draw line in scene view to represent ray
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, (Vector2)transform.position + Vector2.left * transform.localScale.x * distance * 1.5f);
    }

    // Draw GUI Method
    void OnGUI()
    {
        // Press E Check
        if (drawInteractionOption == true)
        {
            GUI.Label(new Rect(10, 10, Screen.width, Screen.height), "PRESS E TO INTERACT");
        }
        //// Draw Menu
        if (drawMenu == true)
        {
            //    // Room Names
            //    GUI.Label(new Rect(10, 30, Screen.width, Screen.height), "Room: " + roomName);
            //    // Weapons in Room
            //    GUI.Label(new Rect(10, 60, Screen.width, Screen.height), "Weapons in Room:");
            for (int i = 0; i < currWepList.Count; i++)
        {
            GUI.Label(new Rect(15, 75 + (i * 15), Screen.width, Screen.height), " - " + currWepList[i].WeaponName);
        }
        }

    }

    void PopDown()
    {
        menuUI.SetActive(false);
        MenuPopUp = false;
    }

    void PopUp()
    {
        menuUI.SetActive(true);
        MenuPopUp = true;
    }
    #endregion
}