using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages general operations of game
/// </summary>
public class GameManager : MonoBehaviour
{

    #region Fields

    // win / lose condition fields
    int daysRemaining = 7;                  // counter tracking number of days remaining in game
    bool murdererAtLarge = true;            // flag indicating whether player has caught murderer
    GameObject characterExecuted;           // character game object executed and returned from execution room

    // character initialization fields
    [SerializeField]
    GameObject characterPrefab;                             // a prefab of the character game object
    float charRadius;                                       // radius of character's circle collider
    Dictionary<CharacterList, GameObject> suspects = 
        new Dictionary<CharacterList, GameObject>();        // Dictionary pairing character objects with character names
    int numOfCharacters = 16;                               // total character count

    // murderer initialization fields
    int numOfKillers = 2;                                   // total murderer count
    List<int> murdererIDs = new List<int>();                // list of murderer IDs - randomly generated each game
    List<GameObject> murderers = new List<GameObject>();    // list of instanced murderer game objects
    int killerToStrike = 0;                                 // list location of murderer to strike at end of day
    GameObject murderer;                                    // saved instance of the murderer game object

    // room initialization fields
    List<GameObject> rooms = new List<GameObject>();    // a list storing each room in game
    int numOfRooms = 4;                                 // total room count
    [SerializeField]
    GameObject roomPrefab;                              // a prefab of the room game object
    float roomWidth;                                    // width of the room
    float roomHeight;                                   // height of the room
    Vector2 room1Loc;                                   // location of room 1
    Vector2 room2Loc;                                   // location of room 2
    Vector2 room3Loc;                                   // location of room 3
    Vector4 room4Loc;                                   // location of room 4

    // lobby initialization fields
    [SerializeField]
    GameObject lobbyPrefab;             // a prefab of the lobby game object
    GameObject lobbyInstance;           // the saved instance of a lobby prefab
    Vector2 lobbyLocation;              // location to spawn the lobby at
    float lobbyOffset = 1;              // y-offset to keep lobby on-screen

    // execution room initialization fields
    [SerializeField]
    GameObject executionPrefab;             // a prefab of the execution room game object
    GameObject exeRoomInstance;             // the saved instance of an execution room prefab
    Vector2 exeRoomLoc;                     // location to spawn the execution room at

    #endregion

    #region Properties

    /// <summary>
    /// Provides get access to days remaining in game
    /// </summary>
    public int DaysRemaining
    {
        get { return daysRemaining; }
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
                    currRoom.GetComponent<Lobby>().Remove(charToPush);
                else if (currRoom.CompareTag("executionRoom"))
                    currRoom.GetComponent<ExecutionRoom>().Clear();

                // push character to appropriate room
                charComponent.CurrentRoom = rooms[roomNumber - 1];
                rooms[roomNumber - 1].GetComponent<Room>().Populate(charToPush);
            }
            else
                // print full room message to console
                Debug.Log("Room " + roomNumber + " is full. Make room by clearing a current occupant"
                    + " before placing another character within it.");
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
                // print full room message to console
                Debug.Log("Execution room is full. Remove its current occupant before placing " +
                    "another character within it.");
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
                Debug.Log("Innocent killed. Game Over.");
            }
            else
            {
                // remove killer from list of murderers
                murderers.Remove(characterExecuted);

                // if player has found all killers, tell them they've won
                if (murderers.Count < 1)
                {
                    Debug.Log("All killer found. You win!");
                    murdererAtLarge = false;

                    // break from end day process
                    return;
                }
                // otherwise (i.e., killers remain to be found)
                else
                {
                    Debug.Log("Murderer caught. " + murderers.Count + " killers remain.");
                }
            }
        }

        // Note: win / lose conditions haven't been met
        // killer to strike tonight determines to kill roommate
        murderers[killerToStrike].GetComponent<Murderer>().DetermineToKill();

        // killer to strike passes on to next killer in list
        killerToStrike++;
        if (killerToStrike >= murderers.Count)
        {
            killerToStrike = 0;
        }
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

        // save room locations according to saved dimensions
        room1Loc = new Vector2(-roomWidth, roomHeight);
        room2Loc = new Vector2(roomWidth, roomHeight);
        room3Loc = new Vector2(-roomWidth, -roomHeight);
        room4Loc = new Vector2(roomWidth, -roomHeight);
    }

    // Use this for initialization
    void Start ()
    {
        // create an instance of an empty lobby
        lobbyLocation = new Vector2(0, ScreenUtils.ScreenBottom + lobbyOffset);
        lobbyInstance = Instantiate(lobbyPrefab, lobbyLocation, Quaternion.identity);

        // create an instance of an empty execution room
        exeRoomLoc = new Vector2(ScreenUtils.ScreenRight / 2, 0);
        exeRoomInstance = Instantiate(executionPrefab, exeRoomLoc, Quaternion.identity);

        // instantiate a set of rooms and add them to a room list
        GameObject room1 = Instantiate(roomPrefab, room1Loc, Quaternion.identity);
        rooms.Add(room1);
        GameObject room2 = Instantiate(roomPrefab, room2Loc, Quaternion.identity);
        rooms.Add(room2);
        GameObject room3 = Instantiate(roomPrefab, room3Loc, Quaternion.identity);
        rooms.Add(room3);
        GameObject room4 = Instantiate(roomPrefab, room4Loc, Quaternion.identity);
        rooms.Add(room4);

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

                    // TODO: add reference of new murderer to list of murderers
                    murderers.Add(newChar);
                    //murderer = newChar;
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
