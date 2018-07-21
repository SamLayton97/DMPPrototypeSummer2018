using System;
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

    // character placement fields
    [SerializeField]
    protected GameObject characterPrefab;                   // a prefab of the character game object
    protected float characterRadius;                        // radius of character's circle colliders
    Vector2 occupantLoc = new Vector2();                    // position to place occupant game objects
                                                            // Note: relocating physical character game objects used purely
                                                            //  for debugging purposes (i.e., no effect on gameplay)

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
    Text rmNameTextBox;
    Text wpNameTextBox;
    string weaponList;

    public GameObject menuUI;
    public static bool MenuPopUp = false;

    // room identification fields
    [SerializeField]
    string roomName;
    int roomNumber = 0;

    // capacity display fields
    Text capacityText;          // text displaying current and max number of characters in room

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
    /// Provides get and set access to room number
    /// Changes room's sprite accordingly
    /// </summary>
    public int RoomNumber
    {
        get { return roomNumber; }
        set
        {
            // if value is a valid room number
            if (value < 5 && value > 0)
            {
                roomNumber = value;
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
            // and update capacity text
            occupants.Add(newOccupant);
            UpdateCapacityText();

            // set new occupant's current room to this
            // Note: this is dependant on the occupant's type
            if (!newOccupant.CompareTag("corpse"))
                newOccupant.GetComponent<Character>().CurrentRoom = gameObject;
            else
                newOccupant.GetComponent<Corpse>().CurrentRoom = gameObject;

            // move character to location within room
            newOccupant.transform.position = occupantLoc;
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
        // and update capacity display text
        occupants.Remove(occupant);
        UpdateCapacityText();
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// Called before the Start() method
    /// </summary>
    protected virtual void Awake()
    {
        // set occupant location to be room's position
        occupantLoc = transform.position;
    }

    // Use this for initialization
    void Start ()
    {
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

        // Sets text below room to character capacity
        // Note: action performed only for standard rooms
        if (CompareTag("room"))
        {
            UpdateCapacityText();
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
        }

        // checks if the collider and ray is being hit and if e is being pressed
        if (hit.collider != null && Input.GetKeyUp(KeyCode.E))
        {
            MenuPopUp = true;
            if (MenuPopUp)
            {
                // handles menu opening and closing
                PopUp();
                box = hit.collider.gameObject;
                // Grabs the name of the room and places it in menu
                rmNameTextBox = GameObject.FindGameObjectWithTag("RoomNameText").GetComponent<Text>();
                rmNameTextBox.text = "Location: " + roomName;
                // Reset string
                weaponList = "";
                // Grabs names of weapons in room
                wpNameTextBox = GameObject.FindGameObjectWithTag("WeaponNameText").GetComponent<Text>();
                for (int i = 0; i < currWepList.Count; i++)
                {
                    weaponList += currWepList[i].WeaponName + "\n";
                }
                wpNameTextBox.text = weaponList;
            }
            else
            {
                PopDown();
            }
        }
    }

    /// <summary>
    /// Updates displayed capacity text below room object
    /// </summary>
    void UpdateCapacityText()
    {
        // if no reference to capacity text component
        if (capacityText == null)
        {
            // find reference to capacity text
            capacityText = GetComponentInChildren<Text>();
        }

        // update text according to current number of characters
        capacityText.text = occupants.Count.ToString() + " / " + maxOccupancy.ToString();
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