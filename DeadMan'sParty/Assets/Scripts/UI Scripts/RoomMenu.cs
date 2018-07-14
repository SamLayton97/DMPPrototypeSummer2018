using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomMenu : MonoBehaviour {

    public static bool MenuPopUp = false;
    public GameObject menuUI;
    Room room;
    Text RoomName;
    GameObject rm;

    /// <summary>
    /// Called before Start() method
    /// </summary>
    void Awake()
    {

    }
    // Use this for initialization
    void Start ()
    {
        // menuUI.SetActive(false);
    }

    // Update is called once per frame
    void Update ()
    {

        if (Input.GetKeyDown(KeyCode.E))
        {
            room = rm.GetComponent<Room>();
            Text RoomName = GameObject.FindGameObjectWithTag("RoomNameText").GetComponent<Text>();
            RoomName.text = room.GetRoomName;
            if (MenuPopUp)
            {
                PopDown();
            }
            else
            {
                PopUp();
            }
        }
	}

    void PopDown()
    {
        menuUI.SetActive(false);
        MenuPopUp = false;
    }

    void PopUp()
    {
        menuUI.SetActive(true);
        MenuPopUp = true;
    }
}
