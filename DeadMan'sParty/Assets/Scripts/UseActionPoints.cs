using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseActionPoints : MonoBehaviour
{
    ActionPoints actionPoints;
    float currentAP;

	// Use this for initialization
	void Start ()
    {
        actionPoints = GetComponent<ActionPoints>();
        currentAP = actionPoints.CurrentAP;
	}
	
    public void Investigate()
    {
        //actionPoints.InvestigateRoom();
        --currentAP;
        Debug.Log(currentAP);

    }
	// Update is called once per frame
	void Update ()
    {

		
	}
}
