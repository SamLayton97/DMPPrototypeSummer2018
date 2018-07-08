using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// UI which allows player to end day (turn)
/// and displays number of days remaining
/// </summary>
public class DayCounter : MonoBehaviour
{
    #region Fields
    // End-day / day counter support
    GameManager gameManager;
    Text dayCounterText;
    Text endDayText;
    #endregion

    #region PrivateMethods
    /// <summary>
    /// Called before Start() method
    /// </summary>
    void Awake()
    {
        // retrieve necessary objects from scene
        gameManager = Camera.main.GetComponent<GameManager>();
        dayCounterText = GameObject.FindGameObjectWithTag("DaysRemainingText").GetComponent<Text>();
        endDayText = GameObject.Find("ActionButton").GetComponentInChildren<Text>();
    }

    // Use this for initialization
    void Start()
    {
        // set days remaining counter to initial value
        dayCounterText.text = "Days Left: " + gameManager.DaysRemaining;
        endDayText.text = "Actions " + gameManager.CurrentAP;
    }

    /// <summary>
    /// Called every frame
    /// </summary>
    void Update()
    {
        // Allows for text update based on CurrentAP
        if (gameManager.CurrentAP > 0)
        {
            endDayText.text = "Actions " + gameManager.CurrentAP;
        }
        else
        {
            endDayText.text = "End Day";
        }
    }
    #endregion

    #region PublicMethods

    /// <summary>
    /// Called when player clicks "End Day" button
    /// </summary>
    public void OnClickEndDayButton()
    {
        gameManager.EndDay();

        // resets actions points after ending day
        gameManager.CurrentAP = gameManager.TotalAP;
        Debug.Log("Day ended, your AP has been reset to " + gameManager.CurrentAP + " AP");


        // update number of days left
        if (gameManager.DaysRemaining != 0)
            dayCounterText.text = "Days Left: " + gameManager.DaysRemaining;
        else
            dayCounterText.text = "FINAL DAY";
        
    }
    #endregion

}

