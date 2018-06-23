using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manages general operations of game
/// </summary>
public class GameManager : MonoBehaviour
{

    #region Fields

    // used for notification popup support
    Text notifPopUp;
    Button continueButton;

    // win / lose condition fields
    [SerializeField]
    int daysRemaining = 7;                  // counter tracking number of days remaining in game
    bool murdererAtLarge = true;            // flag indicating whether player has caught murderer
    GameObject characterExecuted;           // character game object executed and returned from execution room

    // character initialization fields
    [SerializeField]
    GameObject characterPrefab;                             // a prefab of the character game object
    float charRadius;                                       // radius of character's circle collider
    Dictionary<CharacterList, GameObject> suspects = 
        new Dictionary<CharacterList, GameObject>();        // Dictionary pairing character objects with character names
    [SerializeField]
    int numOfCharacters = 16;                               // total character count

    // murderer initialization fields
    [SerializeField]
    int numOfKillers = 2;                                   // total murderer count
    List<int> murdererIDs = new List<int>();                // list of murderer IDs - randomly generated each game
    List<GameObject> murderers = new List<GameObject>();    // list of instanced murderer game objects
    int killerToStrike = 0;                                 // list location of murderer to strike at end of day
    GameObject murderer;                                    // saved instance of the murderer game object

    // room initialization fields
    List<GameObject> rooms = new List<GameObject>();    // a list storing each room in game
    [SerializeField]
    int numOfRooms = 4;                                 // total room count
    [SerializeField]
    int maxNumOfRooms = 6;                              // max number of rooms in game
    [SerializeField]
    int occupancyOfRooms = Room.maxCapacity;                        // Occupancy of room
    [SerializeField]
    int minNumOfRooms = 2;                              // min number of rooms in game
                                                        // NOTE: in any standard level, this should never be less than 2
                                                        // as anything less removes the central game mechanic. Setting this to 1
                                                        // should only be used for testing purposes.
    // room initialization fields cont.
    [SerializeField]
    GameObject roomPrefab;                              // a prefab of the room game object
    float roomWidth;                                    // width of the room
    float roomHeight;                                   // height of the room
    Vector2[] roomLocations;                            // an array of room locations

    // lobby and gallows support fields
    GameObject lobbyInstance;           // the saved instance of a lobby prefab
    GameObject exeRoomInstance;         // the saved instance of an execution room prefab

    #endregion

    #region Properties

    /// <summary>
    /// Provides get access to days remaining in game
    /// </summary>
    public int DaysRemaining
    {
        get { return daysRemaining; }
    }

    /// <summary>
    /// Provides access to notificiation text
    /// </summary>
    public Text NotifPopUp
    {
        get { return notifPopUp; }
    }

    /// <summary>
    /// Provides access to button
    /// </summary>
    public Button ContinueButton
    {
        get { return continueButton; }
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Moves a character from one room to another
    /// Note: If a character is pushed to a full room, they are sent to lobby.
    /// </summary>
    /// <param name="charToPush">character game object to move</param>
    /// <param name="roomNumber">ID of room to push character to
    /// Note: 0 pushes character to lobby and -1 pushes character to execution room</param>
    public void PushToRoom (GameObject charToPush, int roomNumber)
    {
        // retrieve character component and current room game object
        Character charComponent = charToPush.GetComponent<Character>();
        GameObject currRoom = charComponent.CurrentRoom;

        // Push character from one room to another
        // if room number matches one of list of standard rooms
        if (roomNumber > 0 && roomNumber <= rooms.Count)
        {
            // if specified standard room isn't full 
            if (!rooms[roomNumber - 1].GetComponent<Room>().IsFull)
            {
                // retrieve type-specific room component of current room
                // and remove character from it
                if (currRoom.CompareTag("room"))
                    currRoom.GetComponent<Room>().Remove(charToPush);
                else if (currRoom.CompareTag("lobby"))
                {
                    currRoom.GetComponent<Lobby>().Remove(charToPush);
                }
                else if (currRoom.CompareTag("executionRoom"))
                    currRoom.GetComponent<ExecutionRoom>().Clear();

                // push character to appropriate room
                charComponent.CurrentRoom = rooms[roomNumber - 1];
                rooms[roomNumber - 1].GetComponent<Room>().Populate(charToPush);
            }
            else
            {
                // Checks for blank text box
                if (notifPopUp.text == "")
                {
                    {
                        // prints notification of full room
                        notifPopUp.text = "Room " + roomNumber + " is full. Make room by clearing a current occupant"
                        + " before placing another character within it.";
                        continueButton.gameObject.SetActive(true);
                    }
                }

                // print full room message to console
                Debug.Log("Room " + roomNumber + " is full. Make room by clearing a current occupant"
                    + " before placing another character within it.");
            }
        }
        // if room number is 0 (i.e. lobby ID)
        else if (roomNumber == 0)
        {
            // retrieve type-specific room component of current room
            // and remove character from it
            if (currRoom.CompareTag("room"))
                currRoom.GetComponent<Room>().Remove(charToPush);
            else if (currRoom.CompareTag("lobby"))
                currRoom.GetComponent<Lobby>().Remove(charToPush);
            else if (currRoom.CompareTag("executionRoom"))
                currRoom.GetComponent<ExecutionRoom>().Clear();

            // push character to lobby
            charComponent.CurrentRoom = lobbyInstance;
            lobbyInstance.GetComponent<Lobby>().Populate(charToPush);
        }
        // if room number is -1 (i.e. execution room ID)
        else if (roomNumber == -1)
        {
            // if execution chamber isn't occupied
            if (!exeRoomInstance.GetComponent<ExecutionRoom>().IsOccupied)
            {
                // retrieve type-specific room component of current room
                // and remove character from it
                if (currRoom.CompareTag("room"))
                    currRoom.GetComponent<Room>().Remove(charToPush);
                else if (currRoom.CompareTag("lobby"))
                    currRoom.GetComponent<Lobby>().Remove(charToPush);
                else if (currRoom.CompareTag("executionRoom"))
                    currRoom.GetComponent<ExecutionRoom>().Clear();

                // push character to execution room
                charComponent.CurrentRoom = exeRoomInstance;
                exeRoomInstance.GetComponent<ExecutionRoom>().Populate(charToPush);
            }
            else
            {
                if (notifPopUp.text == "")
                {
                    // prints notification of occupied execution room
                    notifPopUp.text = "Execution room is full. Remove its current occupant before placing " +
                    "another character within it.";
                    continueButton.gameObject.SetActive(true);
                }

                // print full room message to console
                Debug.Log("Execution room is full. Remove its current occupant before placing " +
                        "another character within it.");
            }
        }
        // otherwise (i.e., invalid room number)
        else
        {
            // print error to console
            Debug.Log("Error: Invalid room ID. Moving to lobby.");
            PushToRoom(charToPush, 0);
        }
    }

    /// <summary>
    /// Ends in-game day
    /// </summary>
    public void EndDay()
    {
        // if lobby is not empty
        if (!lobbyInstance.GetComponent<Lobby>().IsEmpty)
        {
            if (notifPopUp.text == "")
            {
                // prints occupied lobby notification
                notifPopUp.text = "Cannot end day with characters in lobby.";
                continueButton.gameObject.SetActive(true);
            }
            // break from end-day process
            // Note: placeholder for first iteration
            Debug.Log("Cannot end day with characters in lobby.");
            return;
        }

        // iterate through list of standard rooms
        foreach (GameObject room in rooms)
        {
            // if room contains only 1 character
            if (room.GetComponent<Room>().Count == 1)
            {
                if (notifPopUp.text == "")
                {
                    // prints single character end-day prevention message
                    notifPopUp.text = "Cannot end day with only 1 character in a room. " +
                    "Place either multiple characters in that rooms or empty it.";
                    continueButton.gameObject.SetActive(true);
                }
                // break from end day process
                // Note: placeholder for first iteration
                Debug.Log("Cannot end day with only 1 character in a room. " +
                    "Place either multiple characters in that rooms or empty it.");
                return;
            }
        }

        // decrement number of days left and lose game if final day ends
        --daysRemaining;
        if (daysRemaining < 0 && murdererAtLarge)
        {
            if (notifPopUp.text == "")
            {
                // prints murderer escaped fail state message
                notifPopUp.text = "Murderer escaped. Game Over.";
                continueButton.gameObject.SetActive(true);
            }
            // Note: placeholder for first iteration
            Debug.Log("Murderer escaped. Game Over.");
        }

        // if execution chamber is occupied
        if (exeRoomInstance.GetComponent<ExecutionRoom>().IsOccupied)
        {
            // destroy occupant
            characterExecuted = exeRoomInstance.GetComponent<ExecutionRoom>().ExecuteOccupant();

            // Remove killer from list of murderers if applicable
            // and return appropriate feedback to player
            // Note: debug logs are placeholders for first iteration
            if (!characterExecuted.CompareTag("murderer"))
            {
                if (notifPopUp.text == "")
                {
                    // prints innocent killed fail state message
                    notifPopUp.text = "Innocent killed. Game Over.";
                    continueButton.gameObject.SetActive(true);
                }
                Debug.Log("Innocent killed. Game Over.");
            }
            else
            {
                // remove killer from list of murderers
                murderers.Remove(characterExecuted);

                // if player has found all killers, tell them they've won
                if (murderers.Count < 1)
                {
                    if (notifPopUp.text == "")
                    {
                        // prints killers found victory condition message
                        notifPopUp.text = "All killers found. You win!";
                        continueButton.gameObject.SetActive(true);
                    }
                    Debug.Log("All killers found. You win!");
                    murdererAtLarge = false;

                    // break from end day process
                    return;
                }
                // otherwise (i.e., killers remain to be found)
                else
                {
                    if (notifPopUp.text == "")
                    {
                        // displays number of murderers left
                        if (murderers.Count > 1)
                        {
                            notifPopUp.text = "Murderer caught. " + murderers.Count + " killers remain.";
                            continueButton.gameObject.SetActive(true);
                        }
                        else if (murderers.Count == 1)
                        {
                            notifPopUp.text = "Murderer caught. " + murderers.Count + " killer remains.";
                            continueButton.gameObject.SetActive(true);
                        }
                    }
                    Debug.Log("Murderer caught. " + murderers.Count + " killers remain.");
                }
            }
        }

        // Note: win / lose conditions haven't been met

        // arm any unarmed killers in game
        // Note: Placeholder for week 3 version. In later version, killers will take weapons from
        // list of weapons in their current room.
        //foreach(GameObject killer in murderers)
        //{
        //    killer.GetComponent<Murderer>().IsArmed = true;
        //}

        // pass killer-to-strike on to next killer in list
        killerToStrike++;
        if (killerToStrike >= murderers.Count)
        {
            killerToStrike = 0;
        }

        // killer to strike tonight determines to kill a roommate
        murderers[killerToStrike].GetComponent<Murderer>().DetermineToKill();
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// Called before the Start() method
    /// </summary>
    void Awake()
    {
        // create temp room to save its dimensions and then destroy it
        GameObject tempRoom = Instantiate(roomPrefab);
        BoxCollider2D tempCollider = tempRoom.GetComponent<BoxCollider2D>();
        roomWidth = tempCollider.size.x;
        roomHeight = tempCollider.size.y;
        Destroy(tempRoom);

        // save room locations into array of preset length
        // confine number of rooms to given bounds
        if (numOfRooms > maxNumOfRooms)
            numOfRooms = maxNumOfRooms;
        else if (numOfRooms < minNumOfRooms)
            numOfRooms = minNumOfRooms;
        roomLocations = new Vector2[numOfRooms + (numOfRooms % 2)];

        // determine number of rows of rooms
        int numOfRows = 0;
        if (numOfRooms > 2)
            numOfRows = 2;
        else
            numOfRows = 1;

        // calculate initial coordinates to place rooms
        float initialXLoc = 0 - roomWidth;
        float initialYLoc = roomHeight * (numOfRows - 1);

        // calculate number of rooms per row and modify initial x coordinate if appropriate
        int roomsPerRow = (numOfRooms / numOfRows) + (numOfRooms % numOfRows);
        if (roomsPerRow == 3)
            initialXLoc -= .9f;

        // store room placement locations according to saved dimensions
        // splits game scene into appropriate number of rows
        for (int i = 0; i < numOfRows; i++)
        {
            // fills spots of current row
            for (int j = 0; j < roomsPerRow; j++)
            {
                roomLocations[((numOfRooms / 2 + numOfRooms % 2) * i) + j] = 
                    new Vector2(initialXLoc + (j * roomWidth * (2 - .5f * (roomsPerRow - 2))),
                    initialYLoc - (i * roomHeight * 2));
            }
        }
    }

    // Use this for initialization
    void Start ()
    {
        // gets text block component
        notifPopUp = GameObject.FindGameObjectWithTag("NotifPopUp").GetComponent<Text>();
        notifPopUp.text = "";

        // gets button component
        continueButton = GameObject.FindGameObjectWithTag("ContinueButton").GetComponent<Button>();
        continueButton.gameObject.SetActive(false);

        // find and save lobby and execution room from scene
        lobbyInstance = GameObject.FindGameObjectWithTag("lobby");
        exeRoomInstance = GameObject.FindGameObjectWithTag("executionRoom");

        // instantiate a set of rooms and add them to a room list
        for (int i = 0; i < numOfRooms; i++)
        {
            GameObject room = Instantiate(roomPrefab, roomLocations[i], Quaternion.identity);
            rooms.Add(room);
        }

        // iterate through list of rooms
        foreach (GameObject room in rooms)
        {
            // set room number according to place in list
            room.GetComponent<Room>().RoomNumber = (rooms.IndexOf(room) + 1);
        }

        // Generate random murderer ID(s)
        murdererIDs = RandomSet.RandomSetOfInts(0, numOfCharacters, numOfKillers, true);

		// create a fresh set of characters with one murderer
        for (int i = 0; i < numOfCharacters; i++)
        {
            bool flaggedAsKiller = false;   // flag marking whether new character is killer

            // instantiate new character and pair it with name in dictionary
            GameObject newChar = Instantiate(characterPrefab);
            suspects.Add((CharacterList)i, newChar);

            // if i matches one of murderer IDs, set char's status to be murderer
            foreach (int ID in murdererIDs)
            {
                if (i == ID)
                {
                    newChar.GetComponent<Character>().SetData((CharacterList)i, true);
                    newChar.AddComponent<Murderer>();
                    newChar.tag = "murderer";
                    flaggedAsKiller = true;

                    // add reference of new murderer to list of murderers
                    murderers.Add(newChar);
                }
            }

            // if character wasn't marked as killer
            if (!flaggedAsKiller)
            {
                // set char's status to be non-murderer
                newChar.GetComponent<Character>().SetData((CharacterList)i, false);
            }
        }

        int charEnumerator = 0;             // an enumerator to aid in proper character spawning
        // for each room in list of rooms
        foreach (GameObject room in rooms)
        {
            Room currRoom = room.GetComponent<Room>();

            // while current room isn't full
            // && there are characters left to place
            while (charEnumerator < suspects.Count && !currRoom.IsFull)
            {
                // populate it with character from character list
                currRoom.Populate(suspects[(CharacterList)charEnumerator]);

                // increment character enumerator
                charEnumerator++;
            }
        }
    }

    #endregion

}
