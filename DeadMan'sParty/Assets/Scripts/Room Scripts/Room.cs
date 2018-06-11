using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A room to hold characters for the night
/// </summary>
public class Room : MonoBehaviour
{

    #region Fields

    // capacity support fields
    [SerializeField]
    const int maxCapacity = 4;                                      // max num of characters to fit in room
    public List<GameObject> occupants = new List<GameObject>();     // list of characters currently occupying room

    // character space fields
    [SerializeField]
    protected GameObject characterPrefab;                   // a prefab of the character game object
    protected float characterRadius;                        // radius of character's circle colliders
    Vector2[] occupantLocs = new Vector2[maxCapacity];      // an array of occupant locations

    // room identification fields
    int roomNumber = 0;
    SpriteRenderer spriteRenderer;
    [SerializeField]
    Sprite room1Sprite;
    [SerializeField]
    Sprite room2Sprite;
    [SerializeField]
    Sprite room3Sprite;
    [SerializeField]
    Sprite room4Sprite;

    #endregion

    #region Properties

    /// <summary>
    /// Returns whether room is at max capacity
    /// </summary>
    public bool IsFull
    {
        get
        {
            if (occupants.Count > (maxCapacity - 1))
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
                }
            }
        }
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Populates room with a character from lobby
    /// </summary>
    /// <param name="character">character to be added to room</param>
    public virtual void Populate(GameObject newOccupant)
    {
        // if there is space in the room
        if (occupants.Count < maxCapacity)
        {
            // add new occupant to list of occupants
            occupants.Add(newOccupant);

            // set occupant's current room to this
            newOccupant.GetComponent<Character>().CurrentRoom = gameObject;

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
        // create temp character to save its dimentions and then destroy it
        GameObject tempChar = Instantiate(characterPrefab);
        characterRadius = tempChar.GetComponent<CircleCollider2D>().radius;
        Destroy(tempChar);

        // calculate initial coordinates to place occupants
        BoxCollider2D boxCollider2D = GetComponent<BoxCollider2D>();
        float initialXLoc = transform.position.x - (characterRadius * ((float)maxCapacity / 4f));
        float initialYLoc = transform.position.y + characterRadius;

        // store occupant placement locations according to saved dimensions
        // splits room into two rows
        for (int i = 0; i < 2; i++)
        {
            // fills row
            for (int j = 0; j < (maxCapacity / 2); j++)
            {
                occupantLocs[(2 * i) + j] = new Vector2(initialXLoc + (j * characterRadius * 2),
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
    }
	
	// Update is called once per frame
	void Update ()
    {

	}

    #endregion

}
