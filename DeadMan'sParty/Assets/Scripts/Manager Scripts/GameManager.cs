using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manages general operations of game
/// Note: Due to complexity of game's management,
/// room related operations are handled in the Room
/// Manager script.
/// </summary>
public class GameManager : MonoBehaviour
{

    #region Fields

    // used for notification popup support
    Text notifPopUp;
    Button continueButton;

    // stored for Action Points
    [SerializeField]
    float totalAP = 7;            // total amount of AP
    float currentAP;              // current amount of AP

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

    // reference to the Room Manager
    RoomManager roomManager;

    #endregion

    #region Properties

    /// <summary>
    /// Provides read/write access to the current number of AP left
    /// </summary>
    public float CurrentAP
    {
        get { return currentAP; }
        set { currentAP = value; }
    }

    /// <summary>
    /// Provides access to total amount of AP
    /// </summary>
    public float TotalAP
    {
        get { return totalAP; }
    }

    /// <summary>
    /// Provides get / set access to number of characters in game
    /// </summary>
    public int NumOfCharacters
    {
        get { return numOfCharacters; }
        set
        {
            // protects from invalid set-values
            if (value < 0)
            {
                numOfCharacters = 0;
                Debug.Log("Error: Attempting to set number of characters to negative number.");
            }
            else
                numOfCharacters = value;
        }
    }

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
    /// Returns reference to suspect (character) by name
    /// </summary>
    /// <param name="suspectName">name of character to return</param>
    /// <returns>character which was paired with name parameter</returns>
    public GameObject ReturnSuspect (CharacterList suspectName)
    {
        return suspects[suspectName];
    }

    /// <summary>
    /// Useable method to subract AP from CurrentAP
    /// </summary>
    public void UseActionPoints()
    {
        --currentAP;
        Debug.Log(currentAP);
    }

    /// <summary>
    /// Ends in-game day
    /// </summary>
    public void EndDay()
    {
        // determines whether game has met room-related end-day conditions
        if (roomManager.CanEndDay() == false)
        {
            // break from end-day process
            return;
        }

        // decrement number of days left and lose game if final day ends
        --daysRemaining;
        if (daysRemaining < 0 && murdererAtLarge)
        {
            // TODO: refactor notif pop-up to work with separate managers
            //if (notifPopUp.text == "")
            //{
            //    // prints murderer escaped fail state message
            //    notifPopUp.text = "Murderer escaped. Game Over.";
            //    continueButton.gameObject.SetActive(true);
            //}
            // Note: placeholder for first iteration
            Debug.Log("Murderer escaped. Game Over.");
        }

        // if execution chamber is occupied
        if (roomManager.ExecutionRoom.IsOccupied)
        {
            // destroy occupant
            characterExecuted = roomManager.ExecutionRoom.ExecuteOccupant();

            // Remove killer from list of murderers if applicable
            // and return appropriate feedback to player
            // Note: debug logs are placeholders for first iteration
            if (!characterExecuted.CompareTag("murderer"))
            {
                // TODO: refactor notif pop-up to work with separate managers
                //if (notifPopUp.text == "")
                //{
                //    // prints innocent killed fail state message
                //    notifPopUp.text = "Innocent killed. Game Over.";
                //    continueButton.gameObject.SetActive(true);
                //}
                Debug.Log("Innocent killed. Game Over.");
            }
            else
            {
                // remove killer from list of murderers
                murderers.Remove(characterExecuted);

                // if player has found all killers, tell them they've won
                if (murderers.Count < 1)
                {
                    // TODO: refactor notif pop-up to work with separate managers
                    //if (notifPopUp.text == "")
                    //{
                    //    // prints killers found victory condition message
                    //    notifPopUp.text = "All killers found. You win!";
                    //    continueButton.gameObject.SetActive(true);
                    //}
                    Debug.Log("All killers found. You win!");
                    murdererAtLarge = false;

                    // break from end day process
                    return;
                }
                // otherwise (i.e., killers remain to be found)
                else
                {
                    // TODO: refactor notif pop-up to work with separate managers
                    //if (notifPopUp.text == "")
                    //{
                    //    // displays number of murderers left
                    //    if (murderers.Count > 1)
                    //    {
                    //        notifPopUp.text = "Murderer caught. " + murderers.Count + " killers remain.";
                    //        continueButton.gameObject.SetActive(true);
                    //    }
                    //    else if (murderers.Count == 1)
                    //    {
                    //        notifPopUp.text = "Murderer caught. " + murderers.Count + " killer remains.";
                    //        continueButton.gameObject.SetActive(true);
                    //    }
                    //}
                    Debug.Log("Murderer caught. " + murderers.Count + " killers remain.");
                }
            }
        }

        // Note: win / lose conditions haven't been met

        // finally, arm any unarmed killers in game
        foreach (GameObject killer in murderers)
        {
            Murderer killerComp = killer.GetComponent<Murderer>();

            // if killer isn't armed
            if (!killerComp.IsArmed)
            {
                // arm killer and set them to be inactive for night
                killerComp.Arm();
                killerComp.IsActive = false;
            }
            // otherwise (killer currently holds weapon)
            else
            {
                // set killer to be active tonight (i.e., can determine to strike tonight)
                killerComp.IsActive = true;
            }
        }

        // pass killer-to-strike on to next killer in list
        killerToStrike++;
        if (killerToStrike >= murderers.Count)
        {
            killerToStrike = 0;
        }

        // finally, if killer to strike tonight is active
        // they determine to kill a roommate
        if (murderers[killerToStrike].GetComponent<Murderer>().IsActive)
            murderers[killerToStrike].GetComponent<Murderer>().DetermineToKill();
    }

    #endregion

    #region Private Methods

    // Called before the Start() method
    void Awake ()
    {
        // sets current AP to total AP
        currentAP = totalAP;

        // retrieves a reference to the Room Manager
        roomManager = Camera.main.GetComponent<RoomManager>();

        // gets text block component
        notifPopUp = GameObject.FindGameObjectWithTag("NotifPopUp").GetComponent<Text>();
        notifPopUp.text = "";

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
    }

    #endregion

}
