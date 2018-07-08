using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionPoints : MonoBehaviour
{
    #region Fields

    // stored as floats for change from AP to Time
    float totalAP = 7;          // total amount of Action Points
    float currentAP;            // current amount of Action Points

    Text actionButtonText;

    #endregion

    #region Properties

    // accessor to Current Action Points
    public float CurrentAP
    {
        get { return currentAP; }
        set { currentAP = value; }

    }
    // accesor to Total Action Points allowed
    public float TotalAP
    {
        get { return totalAP; }
        set { totalAP = value; }
    }
    #endregion

    #region PublicMethods

    /// <summary>
    /// Investigation Button Commands
    /// </summary>
    public void Investigate()
    {
        // Allows actions if currentAP is available
        if (currentAP >= 1)
        {
            --currentAP;
            Debug.Log(currentAP);
        }
        else
        {
            Debug.Log("No more actions available, end your day");
        }
    }

    public void UseActionPoints()
    {
        --currentAP;
    }

    /// <summary>
    /// Public Method for Investigation
    /// </summary>
    //public void InvestigateRoom()
    //{
    //    --currentAP;
    //}
    #endregion

    #region Methods

    /// <summary>
    /// Called before Start
    /// </summary>
    void Awake()
    {
        // grants access to Button Text
        actionButtonText = GameObject.Find("ActionButton").GetComponentInChildren<Text>();
    }
    // Use this for initialization
    void Start ()
    {
        // sets currentAP to total amount allowed
        currentAP = totalAP;
        // sets Action Button text
        actionButtonText.text = "Actions: " + currentAP;
	}
	

	// Update is called once per frame
	void Update ()
    {
        // Changes text based off of CurrentAP
        if (currentAP >= 1)
        {
            actionButtonText.text = "Actions: " + currentAP;
        }
        else
        {
            actionButtonText.text = "End Day";
        }

    }

    #endregion
}
