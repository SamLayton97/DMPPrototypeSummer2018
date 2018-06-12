using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContinueButton : MonoBehaviour
{
    GameManager gameManager;
    Text notifPopUp;
    Button continueButton;

    private void Awake()
    {
        gameManager = Camera.main.GetComponent<GameManager>();
        notifPopUp = GameObject.FindGameObjectWithTag("NotifPopUp").GetComponent<Text>();
        continueButton = GameObject.FindGameObjectWithTag("ContinueButton").GetComponent<Button>();

    }
    public void ContinueButtonPress()
    {
        notifPopUp.text = "";
        continueButton.gameObject.SetActive(false);
    }

}
