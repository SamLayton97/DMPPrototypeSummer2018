using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages the creation and population of rooms
/// </summary>
public class RoomManager : MonoBehaviour
{

    #region Fields

    GameManager gameManager;        // reference to head game manager

    // room initialization fields
    List<GameObject> rooms = new List<GameObject>();    // a list storing each room in game
    [SerializeField]
    int numOfRooms;                                     // total room count
    int levelOccupancy = 0;                             // total number of characters able to fit into the scene
                                                        // (dependant on collective occupancy of each room in scene)

    // Serializations of room prefabs
    // Note: Scene should never have greater than 6 or fewer than 2 rooms
    [SerializeField]
    GameObject prefabRoom1;
    [SerializeField]
    GameObject prefabRoom2;
    [SerializeField]
    GameObject prefabRoom3;
    [SerializeField]
    GameObject prefabRoom4;
    [SerializeField]
    GameObject prefabRoom5;
    [SerializeField]
    GameObject prefabRoom6;

    // lobby and gallows support fields
    GameObject lobbyInstance;           // the saved instance of a lobby prefab
    GameObject exeRoomInstance;         // the saved instance of an execution room prefab

    #endregion

    #region Properties

    /// <summary>
    /// Returns number of standard rooms in scene
    /// </summary>
    public int NumOfRooms
    {
        get { return rooms.Count; }
    }

    /// <summary>
    /// Returns a reference to the execution room's ExecutionRoom script
    /// </summary>
    public ExecutionRoom ExecutionRoom
    {
        get { return exeRoomInstance.GetComponent<ExecutionRoom>(); }
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
    public void PushToRoom(GameObject charToPush, int roomNumber)
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
                // TODO: refactor notifications to work with separated managers
                // Checks for blank text box
                //if (notifPopUp.text == "")
                //{
                //    {
                //        // prints notification of full room
                //        notifPopUp.text = "Room " + roomNumber + " is full. Make room by clearing a current occupant"
                //        + " before placing another character within it.";
                //        continueButton.gameObject.SetActive(true);
                //    }
                //}

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
                // TODO: refactor notifications to work with separated managers
                //if (notifPopUp.text == "")
                //{
                //    // prints notification of occupied execution room
                //    notifPopUp.text = "Execution room is full. Remove its current occupant before placing " +
                //    "another character within it.";
                //    continueButton.gameObject.SetActive(true);
                //}

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
    /// Checks set of room-related conditions to see if
    /// game manager can proceed to end day
    /// </summary>
    /// <returns>returns whether GM is clear to end day</returns>
    public bool CanEndDay()
    {
        // if lobby is not empty
        if (!lobbyInstance.GetComponent<Lobby>().IsEmpty)
        {
            // TODO: refactor notif pop-up to work with separated managers
            //if (notifPopUp.text == "")
            //{
            //    // prints occupied lobby notification
            //    notifPopUp.text = "Cannot end day with characters in lobby.";
            //    continueButton.gameObject.SetActive(true);
            //}
            // break from end-day process
            Debug.Log("Cannot end day with characters in lobby.");
            return false;
        }

        // iterate through list of standard rooms
        foreach (GameObject room in rooms)
        {
            // if room contains only 1 character
            if (room.GetComponent<Room>().Count == 1)
            {
                // TODO: refactor notif pop-up to work with separated managers
                //if (notifPopUp.text == "")
                //{
                //    // prints single character end-day prevention message
                //    notifPopUp.text = "Cannot end day with only 1 character in a room. " +
                //    "Place either multiple characters in that rooms or empty it.";
                //    continueButton.gameObject.SetActive(true);
                //}
                // break from end-day process
                Debug.Log("Cannot end day with only 1 character in a room. " +
                    "Place either multiple characters in that rooms or empty it.");
                return false;
            }
        }

        // room-related conditions are satisfied, return true
        return true;
    }

    #endregion

    #region Private Methods

    // Use this for initialization
    void Start ()
    {
        // retrieve reference to game manager
        gameManager = Camera.main.GetComponent<GameManager>();

        // find and save lobby and execution room from scene
        lobbyInstance = GameObject.FindGameObjectWithTag("lobby");
        exeRoomInstance = GameObject.FindGameObjectWithTag("executionRoom");

        // add standard rooms to list of rooms
        GameObject[] roomsInScene = GameObject.FindGameObjectsWithTag("room");
        foreach (GameObject roomGameObject in roomsInScene)
        {
            rooms.Add(roomGameObject);
        }

        // iterate through list of rooms
        foreach (GameObject room in rooms)
        {
            // set room number according to place in list
            room.GetComponent<Room>().RoomNumber = (rooms.IndexOf(room) + 1);

            // calculate max occupancy of level
            levelOccupancy += room.GetComponent<Room>().MaxOccupancy;
            Debug.Log(room.GetComponent<Room>().RoomName + ": " + room.GetComponent<Room>().RoomNumber);
        }

        // if number of characters exceeds occupancy of level, confine former to latter
        if (gameManager.NumOfCharacters > levelOccupancy)
            gameManager.NumOfCharacters = levelOccupancy;


        // populate rooms with game-manager-generated characters
        int charEnumerator = 0;             // an enumerator to aid in proper character spawning
        // for each room in list of rooms
        foreach (GameObject room in rooms)
        {
            Room currRoom = room.GetComponent<Room>();

            // while current room isn't full
            // && there are characters left to place
            while (charEnumerator < gameManager.NumOfCharacters && !currRoom.IsFull)
            {
                // populate it with character from character list
                currRoom.Populate(gameManager.ReturnSuspect((CharacterList)charEnumerator));

                // increment character enumerator
                charEnumerator++;
            }
        }
    }

    #endregion

}
