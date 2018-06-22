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
    // End-day / day counter support
    GameManager gameManager;
    Text dayCounterText;
    ActionPoints actionPoints;


    /// <summary>
    /// Called before Start() method
    /// </summary>
    void Awake()
    {
        // retrieve necessary objects from scene
        gameManager = Camera.main.GetComponent<GameManager>();
        dayCounterText = GameObject.FindGameObjectWithTag("DaysRemainingText").GetComponent<Text>();
        actionPoints = new ActionPoints();
    }

    // Use this for initialization
    void Start()
    {
        // set days remaining counter to initial value
        dayCounterText.text = "Days Left: " + gameManager.DaysRemaining;
    }

    /// <summary>
    /// Called when player clicks "End Day" button
    /// </summary>
    public void OnClickEndDayButton()
    {
        gameManager.EndDay();

        // resets actions points after ending day
        actionPoints.CurrentAP = actionPoints.TotalAP;


        // update number of days left
        if (gameManager.DaysRemaining != 0)
            dayCounterText.text = "Days Left: " + gameManager.DaysRemaining;
        else
            dayCounterText.text = "FINAL DAY";
        
    }
}
