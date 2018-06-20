using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    Text actionButtonText;
    GameManager gameManager;
    float currentActionPoints;

    /// <summary>
    /// Called before Start
    /// </summary>
    private void Awake()
    {
        // gets components for Text change in Action Button
        gameManager = Camera.main.GetComponent<GameManager>();
        actionButtonText = GameObject.Find("ActionButton").GetComponentInChildren<Text>();
    }
    // Use this for initialization
    void Start ()
    {
        currentActionPoints = gameManager.CurrentActionPoints;
        actionButtonText.text = "Actions: " + gameManager.CurrentActionPoints;
	}

    //// Update is called once per frame
    //void Update()
    //{
    //    if (currentActionPoints > 0)
    //    {
    //        actionButtonText.text = "Actions remaining: " + currentActionPoints;
    //    }
    //    else if (currentActionPoints <= 0)
    //    {
    //        actionButtonText.text = "End Day";
    //    }

    //}
}
