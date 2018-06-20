using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContinueButton : MonoBehaviour
{
    // Continue Button support
    GameManager gameManager;
    Text notifPopUp;
    Button continueButton;

    /// <summary>
    /// Called before Start Method
    /// </summary>
    private void Awake()
    {
        // Gets necessary components for text change
        gameManager = Camera.main.GetComponent<GameManager>();
        notifPopUp = GameObject.FindGameObjectWithTag("NotifPopUp").
            GetComponent<Text>();
        continueButton = GameObject.FindGameObjectWithTag("ContinueButton").
            GetComponent<Button>();

    }
    /// <summary>
    /// Continue Button press actions
    /// </summary>
    public void ContinueButtonPress()
    {
        notifPopUp.text = "";
        continueButton.gameObject.SetActive(false);
    }

}
